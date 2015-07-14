#!/bin/bash

SELECTED_FILES="${@}"  # selected filelist
OUTPUT_FILE_EXTENSION="jpg" # also output format for imagemagick

for SOURCE_FILE in $SELECTED_FILES
do

    SOURCE_FILE_NAME=${SOURCE_FILE%.*}
    OUTPUT_FILE="${SOURCE_FILE_NAME}_prev.${OUTPUT_FILE_EXTENSION}"

    WIDTH=$(identify -format "%[fx:w]" "${SOURCE_FILE}")
    HEIGHT=$(identify -format "%[fx:h]" "${SOURCE_FILE}")

    # Maximum and minumum dimension
    MAX=${WIDTH}  
    MIN=${HEIGHT}
    WH=1
    if [ ${WIDTH} < ${HEIGHT} ]   
    then
        MAX=${HEIGHT}
        MIN=${WIDTH}    
        WH=0
    fi
    
    # dimension factor   
    FACTOR1000=$(echo "scale=0; $MAX * 1000 / $MIN" | bc)
      
    if [ ${WH} = 1 ]
    then  
        if [ ${FACTOR1000} -gt 1333  ]
        then
            SIZE="x144"
        else
            SIZE="192"
        fi
    else
        if [ ${FACTOR1000} -gt 1333  ]
        then
            SIZE="192"  
        else
            SIZE="x256"
        fi
    fi

    # generate thumbnail image:
    convert "$SOURCE_FILE" \
            -interpolate filter -filter lanczos -resize ${SIZE} \
            -quality 92 -sampling-factor 1:1:1 \
            -interlace line +repage \
            "$OUTPUT_FILE"
            #nearest-neighbor
            #sharpen 1x1 - only on final save!
done

