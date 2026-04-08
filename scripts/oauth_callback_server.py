#!/usr/bin/env python3
"""
Simple OAuth callback server to capture the authorization code.
Usage: python3 oauth_callback_server.py
Listens on http://localhost:5000/callback and prints the 'code' query param to stdout.
"""
import http.server
import socketserver
import urllib.parse
import sys

PORT = 5000

class CallbackHandler(http.server.BaseHTTPRequestHandler):
    def do_GET(self):
        parsed = urllib.parse.urlparse(self.path)
        if parsed.path != '/callback':
            self.send_response(404)
            self.end_headers()
            self.wfile.write(b'Not found')
            return
        query = urllib.parse.parse_qs(parsed.query)
        code = query.get('code', [None])[0]
        state = query.get('state', [None])[0]
        self.send_response(200)
        self.send_header('Content-type', 'text/html')
        self.end_headers()
        if code:
            self.wfile.write(b'<html><body><h1>Authorization complete</h1><p>You can close this window.</p></body></html>')
            print(code)
            sys.stdout.flush()
        else:
            self.wfile.write(b'<html><body><h1>No code received</h1></body></html>')

    def log_message(self, format, *args):
        return  # silence logs

if __name__ == '__main__':
    with socketserver.TCPServer(('127.0.0.1', PORT), CallbackHandler) as httpd:
        print(f'Listening on http://127.0.0.1:{PORT}/callback')
        sys.stdout.flush()
        httpd.handle_request()
