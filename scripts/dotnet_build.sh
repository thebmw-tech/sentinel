#!/bin/bash
set -xe

source ./scripts/version.sh

dotnet publish -p:Version=${VERSION} -o Publish