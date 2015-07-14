#!/bin/bash

SELECTED_FILES="${@}"  # selected filelist
BACKUP_DIR="./~magick"
OUTPUT_FILE_EXTENSION="jpg" # also output format for imagemagick

#SOURCE_FILE="$1" 
#SOURCE_FILE_EXTENSION=${SOURCE_FILE##*.}
#OUTPUT_FILE="${SOURCE_FILE_NAME}.${OUTPUT_FILE_EXTENSION}"

mkdir -p "${BACKUP_DIR}"

for SOURCE_FILE in ${SELECTED_FILES}
do

    SOURCE_FILE_NAME=${SOURCE_FILE%.*}
    SOURCE_FILE_LC=${SOURCE_FILE,,} # lowercase   
    OUTPUT_FILE="${SOURCE_FILE_NAME}.${OUTPUT_FILE_EXTENSION}"
    OUTPUT_FILE_LC=${OUTPUT_FILE,,} # lowercase 

    cp -f --backup=t "${SOURCE_FILE}" "${BACKUP_DIR}"
    mv -f -T "${SOURCE_FILE}" "${SOURCE_FILE_LC}"

    # imagemagick: 
    # -sampling-factor 1x1
    convert "${SOURCE_FILE_LC}" -auto-orient -interpolate filter -filter lanczos \
                           -normalize -quality 92 -sampling-factor 1:1:1 \
                           -interlace Line +repage "${OUTPUT_FILE_LC}"
        
    # remove source file when copy is created
    if [ "${OUTPUT_FILE_LC}" != "${SOURCE_FILE_LC}" ] 
    then
        rm -f "${SOURCE_FILE_LC}"            
    fi 
done

