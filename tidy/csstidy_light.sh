#!/bin/bash

SELECTED_FILES="${@}"  # selected filelist
BACKUP_DIR="./~csstidy"

mkdir -p "${BACKUP_DIR}"

for SOURCE_FILE in ${SELECTED_FILES}
do
    OUTPUT_FILE="${SOURCE_FILE}"
    
    cp -f --backup=t "${SOURCE_FILE}" "${BACKUP_DIR}"

    csstidy "${SOURCE_FILE}" --allow_html_in_templates=false --compress_colors=true --compress_font-weight=false --discard_invalid_properties=false --lowercase_s=false --preserve_css=true --remove_bslash=true --silent=false --sort_properties=false --sort_selectors=false --timestamp=false --merge_selectors=0 --case_properties=true --optimise_shorthands=2 --template=low "--remove_last_;=true" "${OUTPUT_FILE}"
    
done
