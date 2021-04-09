#!/bin/bash
set -x

# Prep Folders
mkdir -p package/debian/sentinel/usr/share/Sentinel

# Copy Sentinel to package location
cp -rv Publish/* package/debian/sentinel/usr/share/Sentinel/


# Setup File Permissions
chmod -R 755 ./package/debian/sentinel/DEBIAN