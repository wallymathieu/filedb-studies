#!/usr/bin/env bash

set -eu
set -o pipefail

cd `dirname $0`

dotnet restore
dotnet test Tests
dotnet test CoreFsTests
