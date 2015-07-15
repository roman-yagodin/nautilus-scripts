#!/bin/bash

FILES="$@"  # selected filelist
BACKUP_DIR="./~magick"
OUTPUT_FILE_EXTENSION="jpg" # also output format for imagemagick

#SOURCE_FILE="$1" 
#SOURCE_FILE_EXTENSION=${SOURCE_FILE##*.}
#OUTPUT_FILE="${SOURCE_FILE_NAME}.${OUTPUT_FILE_EXTENSION}"

mkdir -p $BACKUP_DIR

for SOURCE_FILE in $FILES
do

    SOURCE_FILE_NAME=${SOURCE_FILE%.*}
    SOURCE_FILE_LC=${SOURCE_FILE,,} # lowercase   
    OUTPUT_FILE="${SOURCE_FILE_NAME}.${OUTPUT_FILE_EXTENSION}"
    OUTPUT_FILE_LC=${OUTPUT_FILE,,} # lowercase 

    cp -f --backup=t "$SOURCE_FILE" "$BACKUP_DIR"
    mv -f -T "$SOURCE_FILE" "$SOURCE_FILE_LC"

    # imagemagick: 
    convert "$SOURCE_FILE_LC" -interpolate filter -filter lanczos -resize 1024x1024\> \
                           -normalize -quality 85 -sampling-factor 1x1 \ 
                           -interlace Line +repage "$OUTPUT_FILE_LC"
        
    # generate thumbnails:
    PREVIEW_FILE="${SOURCE_FILE_NAME}_prev.${OUTPUT_FILE_EXTENSION}"
    PREVIEW_FILE_LC=${PREVIEW_FILE,,}

    WIDTH=$(identify -format "%[fx:w]" "${SOURCE_FILE_LC}")
    HEIGHT=$(identify -format "%[fx:h]" "${SOURCE_FILE_LC}")

    # Maximum and minumum
    MAX=${WIDTH}  
    MIN=${HEIGHT}
    WH=1
    if [${WIDTH} < ${HEIGHT}]   
    then
        MAX=${HEIGHT}
        MIN=${WIDTH}    
        WH=0
    fi
    
    # size factor   
    FACTOR1K=$(echo "scale=0; $MAX * 1000 / $MIN" | bc)
      
    if [ ${WH} = 1 ]
    then  
        if [ ${FACTOR1K} -gt 1333  ]
        then
            SIZE="x144"
        else
            SIZE="192"
        fi
    else
        if [ ${FACTOR1K} -gt 1333  ]
        then
            SIZE="192"  
        else
            SIZE="x256"
        fi
    fi

    # generate thumbnail image:
    convert "$OUTPUT_FILE_LC" -interpolate filter -filter lanczos -resize ${SIZE} \
                                  -sharpen 1x2 -quality 85 -sampling-factor 1x1 \
                                  -interlace Line +repage \
                                  "$PREVIEW_FILE_LC"

    # remove source file when copy is created
    if [ "${OUTPUT_FILE_LC}" != "${SOURCE_FILE_LC}" ] 
    then
        rm -f "${SOURCE_FILE_LC}"            
    fi 
done

