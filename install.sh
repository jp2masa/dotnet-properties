#!/usr/bin/env bash

dotnet tool uninstall --global dotnet-properties

dotnet build dotnet-properties.sln -c Release
dotnet tool install --global --add-source artifacts/Release/nupkg/ --version 0.2.0-* dotnet-properties
