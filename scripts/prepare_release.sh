#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "$0")/.." && pwd)
cd "$ROOT_DIR"

echo "Running tests..."
dotnet test --configuration Release

echo "Building and packing the library..."
dotnet build src/AllegroApi/AllegroApi.csproj -c Release
dotnet pack src/AllegroApi/AllegroApi.csproj -c Release -o ./nupkg

echo "Package(s) created in: $(pwd)/nupkg"

echo
echo "Next steps for release (manual):"
echo "  - Inspect the packages in ./nupkg/"
echo "  - Push release to NuGet.org: dotnet nuget push ./nupkg/*.nupkg --api-key <API_KEY> --source https://api.nuget.org/v3/index.json"
echo "  - Create Git tag and push: git tag -a v<version> -m 'Release v<version>' && git push origin v<version>"
