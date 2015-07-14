#!/bin/bash

SELECTED_FILES="${@}"  # selected filelist
OUTPUT_FILE_EXTENSION="jpg" # also output format for imagemagick

for SOURCE_FILE in $SELECTED_FILES
do

    SOURCE_FILE_NAME=${SOURCE_FILE%.*}
    OUTPUT_FILE="${SOURCE_FILE_NAME}_news.${OUTPUT_FILE_EXTENSION}"

    # generate thumbnail image:
    convert "$SOURCE_FILE" \
            -interpolate filter -filter lanczos -resize x113 \
            -quality 92 -sampling-factor 1:1:1 \
            -interlace line +repage \
            "$OUTPUT_FILE"
            #nearest-neighbor
            #sharpen 1x1 - only on final save!
done

