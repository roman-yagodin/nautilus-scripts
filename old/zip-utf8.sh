#!/bin/bash
pushd
#path="${1%/*}"
#fname="${1##*/}" 
#zip -r -0 -UN=UTF8 "$fname.zip" "$fname"
# > ziplog.txt
SELECTED_FILES="${@}"  # selected filelistBACKUP_DIR="./~magick"
BACKUP_DIR="./~zip"

mkdir -p "${BACKUP_DIR}"

for SOURCE_FILE in ${SELECTED_FILES}
do
    #echo "${SOURCE_FILE}"
    zip -r -0 -UN=UTF8 "${SOURCE_FILE}.zip" "${SOURCE_FILE}"
    mv -f "${SOURCE_FILE}" "${BACKUP_DIR}/${SOURCE_FILE}"
done

popd

#read
