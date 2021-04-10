#!/bin/bash
set -xe

source ./scripts/version.sh

dotnet publish --no-restore -p:Version=${VERSION} -o Publish