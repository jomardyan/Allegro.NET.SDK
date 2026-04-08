#!/usr/bin/env python3
import re
import os
from pathlib import Path
import json

ROOT = Path('/workspaces/OfferManager')
SWAGGER = ROOT / 'API DOC' / 'swagger.yaml'
CLIENTS_DIR = ROOT / 'src' / 'AllegroApi' / 'Clients'
MODELS_DIR = ROOT / 'src' / 'AllegroApi' / 'Models'

# read swagger paths
swagger_paths = []
with SWAGGER.open('r', encoding='utf-8') as f:
    for line in f:
        if line.startswith('  /'):
            path = line.strip().rstrip(':')
            swagger_paths.append(path)

# parse clients for http calls and string literals
client_files = list(CLIENTS_DIR.glob('*.cs'))
implemented_entries = {}  # impl_path -> list of entries

# regex patterns
literal_re = re.compile(r'["\'](/[^"\']+)["\']')
httpcall_re = re.compile(r'_httpClient\.(GetAsync|PostAsync|PutAsync|DeleteAsync|GetRawAsync|PostAsync)<([^>]*)>\s*\(\s*["\'](/[^"\']+)["\']')
# also PostAsync with single type (PostAsync<TRequest, TResponse> or PostAsync<TRequest>) - handle both
httpcall_general_re = re.compile(r'_httpClient\.(GetAsync|PostAsync|PutAsync|DeleteAsync|GetRawAsync)\s*<([^>]*)>\s*\(\s*')
public_method_re = re.compile(r'public\s+System\.Threading\.Tasks\.Task(?:<[^>]+>)?\s+([A-Za-z0-9_]+)\s*\(')

# scan files
for cf in client_files:
    text = cf.read_text(encoding='utf-8')
    lines = text.splitlines()
    for i, line in enumerate(lines):
        # if this line contains an _httpClient call, the path literal may be on this or subsequent lines
        if '_httpClient.' in line:
            # try to extract the http method and any generic type args on this line
            call_match = re.search(r'_httpClient\.(GetAsync|PostAsync|PutAsync|DeleteAsync|GetRawAsync)\s*(?:<([^>]*)>)?\s*\(', line)
            if call_match:
                call = call_match.group(1)
                generic = (call_match.group(2) or '').strip()
                types = [t.strip() for t in generic.split(',')] if generic else []
                # scan current and next few lines for the path literal
                path = None
                for j in range(i, min(i+6, len(lines))):
                    lit_match = literal_re.search(lines[j])
                    if lit_match:
                        path = lit_match.group(1)
                        path_line = j+1
                        break
                # find method name by scanning up to 40 lines above
                method_name = None
                for j in range(i, max(i-40, -1), -1):
                    mm = public_method_re.search(lines[j])
                    if mm:
                        method_name = mm.group(1)
                        break
                entry = {
                    'client': cf.name,
                    'file': str(cf),
                    'line': (path_line if path is not None else i+1),
                    'http_call': call,
                    'generic_types': types,
                    'method': method_name,
                }
                if path:
                    implemented_entries.setdefault(path, []).append(entry)
                else:
                    # record the call location if path not found inline
                    implemented_entries.setdefault(f'CALL_AT_{cf.name}:{i+1}', []).append(entry)
        # also detect literals anywhere for later mapping
        for m2 in literal_re.finditer(line):
            lit = m2.group(1)
            implemented_entries.setdefault(lit, [])

# Normalize function for path keys
def normalize(p):
    return re.sub(r'\{[^}]+\}', '{id}', p)

# Map swagger paths to implementations
mapping = {}
implemented_count = 0
missing = []
for sp in swagger_paths:
    norm_sp = normalize(sp)
    found = []
    # try exact normalized match
    for impl_path, entries in implemented_entries.items():
        if normalize(impl_path) == norm_sp:
            # only consider entries that have metadata (dict) or simple literal
            if isinstance(entries, list):
                # entries could be list of dicts or list of placeholder occurrences
                # unify representation
                for e in entries:
                    if isinstance(e, dict):
                        found.append(e)
                    else:
                        found.append({'client':'(literal)', 'file':'(literal occurrence)', 'line':None, 'http_call':None, 'generic_types':[], 'method':None})
    if found:
        mapping[sp] = {'implemented': True, 'implementations': found}
        implemented_count += 1
    else:
        mapping[sp] = {'implemented': False, 'implementations': []}
        missing.append(sp)

# also detect implemented endpoints that are not present in swagger (extra)
extra_impl = []
for impl_path in implemented_entries.keys():
    # if normalized impl path not in normalized swagger set, list as extra
    norm_impl = normalize(impl_path)
    if not any(normalize(sp) == norm_impl for sp in swagger_paths):
        # skip ones that are not API paths (e.g., local file paths) - heuristics: start with '/'
        if impl_path.startswith('/'):
            extra_impl.append(impl_path)

# Build report content
report_lines = []
report_lines.append('# Endpoint Implementation Map\n')
report_lines.append(f'*Total swagger endpoints: {len(swagger_paths)}*')
report_lines.append(f'*Implemented endpoints (normalized match): {implemented_count}*')
report_lines.append(f'*Missing endpoints: {len(missing)}*')
report_lines.append('')

# Summarize by category (first path segment)
from collections import defaultdict
by_category = defaultdict(lambda: {'total':0,'implemented':0,'missing':0,'paths':[]})
for sp in swagger_paths:
    seg = sp.split('/')[1] if sp.startswith('/') else '(root)'
    by_category[seg]['total'] += 1
    if mapping[sp]['implemented']:
        by_category[seg]['implemented'] += 1
    else:
        by_category[seg]['missing'] += 1
    by_category[seg]['paths'].append(sp)

report_lines.append('## Summary by Category\n')
for cat, data in sorted(by_category.items()):
    report_lines.append(f'- **{cat}**: {data["implemented"]}/{data["total"]} implemented, {data["missing"]} missing')
report_lines.append('')

# Detailed Missing list
report_lines.append('## Missing Endpoints (detailed)\n')
for sp in missing:
    # find recommended owner from path
    seg = sp.split('/')[1] if sp.startswith('/') else '(root)'
    # owner suggestion mapping
    owner = None
    if seg == 'fulfillment': owner = 'FulfillmentClient'
    elif seg == 'offers': owner = 'ListingClient'
    elif seg == 'sale':
        # refine
        if 'badge' in sp: owner = 'BadgesClient'
        elif 'classified' in sp: owner = 'ClassifiedsClient'
        elif 'offer-additional-services' in sp: owner = 'OfferAdditionalServicesClient'
        elif 'offer-attachments' in sp: owner = 'OfferAttachmentsClient'
        elif 'dispute-attachments' in sp: owner = 'DisputeAttachmentsClient'
        elif 'offer-classifieds-packages' in sp: owner = 'ClassifiedsClient'
        else: owner = 'SaleExtensionsClient'
    elif seg == 'charity': owner = 'MiscellaneousClient / CharityClient'
    else: owner = '(assign)'
    report_lines.append(f'- `{sp}` — suggested owner: **{owner}**')
report_lines.append('')

# Detailed Implemented mapping (limited to those that matched swagger)
report_lines.append('## Implemented Endpoints (sample, normalized match)\n')
count_sample = 0
for sp, info in mapping.items():
    if info['implemented']:
        # list the implementations
        report_lines.append(f'- `{sp}`')
        for impl in info['implementations']:
            method = impl.get('method') or '(unknown)'
            client = impl.get('client') or '(unknown client)'
            file = impl.get('file')
            line = impl.get('line')
            types = ','.join(impl.get('generic_types') or [])
            report_lines.append(f'  - Implemented in `{client}` (method: `{method}`) at `{file}:{line}` — types: `{types}`')
        count_sample += 1
        if count_sample >= 200:
            break
report_lines.append('')

# extras
if extra_impl:
    report_lines.append('## Extra endpoints found in clients but not present in swagger (normalized)\n')
    for e in extra_impl:
        report_lines.append(f'- `{e}`')
    report_lines.append('')

# Save report
out_md = ROOT / 'IMPLEMENTATION_ENDPOINT_MAP.md'
out_md.write_text('\n'.join(report_lines), encoding='utf-8')

# also emit JSON map for programmatic use
out_json = ROOT / 'IMPLEMENTATION_ENDPOINT_MAP.json'
json.dump({'mapping': mapping, 'missing': missing, 'summary': {'swagger_total': len(swagger_paths), 'implemented': implemented_count, 'missing': len(missing)}}, out_json.open('w', encoding='utf-8'), indent=2)

print(f'Report written to {out_md} — {len(swagger_paths)} swagger endpoints, {implemented_count} implemented, {len(missing)} missing')
print('Missing endpoints (first 50):')
for sp in missing[:50]:
    print(' -', sp)
