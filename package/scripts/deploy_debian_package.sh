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
elif [ ! -z "$NIGHTLY" ]; then
  COMPONENT="nightly"
else
  COMPONENT="develop"
fi

# Fix private key permissions
chmod 600 ${REPO_SERVER_KEY}

BASE_SSH_ARGS="-i ${REPO_SERVER_KEY} -o StrictHostKeyChecking=no -o UserKnownHostsFile=/dev/null"
SSH_CONNECTION="${REPO_SERVER_USER}@${REPO_SERVER_HOST}"

scp ${BASE_SSH_ARGS} ${NEW_DEB_PACKAGE} ${SSH_CONNECTION}:/tmp/${BASE_PACKAGE}
ssh ${BASE_SSH_ARGS} ${SSH_CONNECTION} reprepro -b /var/www/html/repos/debian -C ${COMPONENT} includedeb misty /tmp/${BASE_PACKAGE}
ssh ${BASE_SSH_ARGS} ${SSH_CONNECTION} rm /tmp/${BASE_PACKAGE}