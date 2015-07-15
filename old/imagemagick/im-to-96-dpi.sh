#!/bin/bash

SELECTED_FILES="${@}"  # selected filelist
#BACKUP_DIR="./~magick"
#OUTPUT_FILE_EXTENSION="jpg" # also output format for imagemagick

#SOURCE_FILE="$1" 
#SOURCE_FILE_EXTENSION=${SOURCE_FILE##*.}
#OUTPUT_FILE="${SOURCE_FILE_NAME}.${OUTPUT_FILE_EXTENSION}"

#mkdir -p "${BACKUP_DIR}"

for SOURCE_FILE in ${SELECTED_FILES}
do
    SOURCE_FILE_EXTENSION=${SOURCE_FILE##*.}
    SOURCE_FILE_EXTENSION_LC=${SOURCE_FILE_EXTENSION,,}

    if [ "${SOURCE_FILE_EXTENSION_LC}" == "png" ]
    then 
        # Set density to 96 dpi 
        # CHECK: value of 37.802 is unbased!
        # TODO: value of -quality must be set to maximum PNG compression
        # TODO: integrate with build book / brand script
        convert "${SOURCE_FILE}" -units PixelsPerInch -density 37.802x37.802 +repage "${SOURCE_FILE}"
            
        # remove source file when copy is created
        #if [ "${OUTPUT_FILE_LC}" != "${SOURCE_FILE_LC}" ] 
        #then
        #    rm -f "${SOURCE_FILE_LC}"            
        #fi
    fi 
done

