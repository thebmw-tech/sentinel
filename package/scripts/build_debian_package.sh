#!/bin/bash
set -xe

BASE_VERSION=`cat ./package/VERSION`

if [ -z $CI ]; then
  echo "We currently don't support building outside of CI Pipeline"
  exit 1
fi

VERSION=${BASE_VERSION}.${CI_PIPELINE_IID}-${CI_COMMIT_SHORT_SHA}

# Copy Sentinel to package location
cp -rv Publish/* package/debian/sentinel/usr/lib/Sentinel/


# Setup File Permissions
chmod -R 755 ./package/debian/sentinel/DEBIAN

# Calculate Checksums For Package
CURRENT_DIR=$PWD
cd ./package/debian/sentinel
find * -type f -exec md5sum {} \; > DEBIAN/md5sum
cd $CURRENT_DIR

# Calculate app size
PRODUCT_SIZE=$(du -s ./package/debian/sentinel | awk '{print $1}')

# Configure control script
sed -i -e 's/__VERSION__/'${VERSION}'/g' ./package/debian/sentinel/DEBIAN/control
sed -i -e 's/__PRODUCT_SIZE__/'${PRODUCT_SIZE}'/g' ./package/debian/sentinel/DEBIAN/control

# Do the build
dpkg-deb --build ./package/debian/sentinel ./package/debian/sentinel_${VERSION}_all.deb