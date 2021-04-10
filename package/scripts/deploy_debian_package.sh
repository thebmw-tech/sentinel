#!/bin/bash
set -xe

NEW_DEB_PACKAGE=$(ls package/debian/*.deb)
BASE_PACKAGE=`basename ${NEW_DEB_PACKAGE}`

if [[ "$CI_COMMIT_BRANCH" =~ ^release\/ ]]; then
  COMPONENT="release"
elif [[ "$CI_COMMIT_BRANCH" =~ ^hotfix\/ ]]; then
  COMPONENT="hotfix"
elif [ "$CI_COMMIT_BRANCH" == "master" ]; then
  COMPONENT="main"
elif [ -z "$NIGHTLY" ]; then
  COMPONENT="nightly"
else
  COMPONENT="develop"
fi


scp -i ${REPO_SEVER_KEY} ${NEW_DEB_PACKAGE} ${REPO_SERVER_USER}@${REPO_SERVER_HOST}:/tmp/${BASE_PACKAGE}
ssh -i ${REPO_SEVER_KEY} ${REPO_SERVER_USER}@${REPO_SERVER_HOST} reprepro -b /var/www/html/repos/debian -C ${COMPONENT} includedeb misty /tmp/${BASE_PACKAGE}