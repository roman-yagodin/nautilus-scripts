#!/bin/sh

SOURCE_FILE="$1" 
OUTPUT_FILE=$SOURCE_FILE
BACKUP_DIR="./.magick"

mkdir -p $BACKUP_DIR
cp -f $SOURCE_FILE $BACKUP_DIR
#convert $SOURCE_FILE -filter lanczos -interpolate filter -adaptive-resize 1024x1024 -normalize -quality 85 $OUTPUT_FILE
convert $SOURCE_FILE -filter lanczos -adaptive-resize 1024x1024 -normalize -quality 85 -interlace None $OUTPUT_FILE

