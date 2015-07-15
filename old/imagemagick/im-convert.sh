#!/bin/bash

SOURCE_FILE="${1}"  # selected file
OUTPUT_FILE="__${SOURCE_FILE}"
    
convert "${SOURCE_FILE}" \
         -auto-level \
         -quality 100 \
         -sampling-factor 1:1:1 \
         -interlace Line \
         +repage \
         "${OUTPUT_FILE}"
