#!/bin/bash

BACKUP_DIR="./~magick"
SOURCE_FILE="$1" 
SOURCE_FILE_NAME=${SOURCE_FILE%.*}
#SOURCE_FILE_EXTENSION=${SOURCE_FILE##*.}
OUTPUT_FILE_EXTENSION="jpg"
OUTPUT_FILE="${SOURCE_FILE_NAME}.${OUTPUT_FILE_EXTENSION}"

mkdir -p $BACKUP_DIR

FILES="$@"
for F in $FILES
do
    cp -f --backup=t $SOURCE_FILE $BACKUP_DIR
    #convert $SOURCE_FILE -filter lanczos -interpolate filter -adaptive-resize 1024x1024 -normalize -quality 85 $OUTPUT_FILE
    convert $SOURCE_FILE -interpolate filter -filter lanczos -resize 1024x1024\>  -normalize -quality 85 -interlace Line +repage $OUTPUT_FILE
done

