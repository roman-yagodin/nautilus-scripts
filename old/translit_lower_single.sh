#!/bin/bash

SOURCE_FILE="${1}"  # selected filelist
#SELECTED_FILES="${NAUTILUS_SCRIPT_SELECTED_FILE_PATHS}"
BACKUP_DIR="./~translit"

mkdir -p "${BACKUP_DIR}"
#for SOURCE_FILE in ${SELECTED_FILES}
#do
#    echo "${SOURCE_FILE}" >> log.txt
    cp -f --backup=t "${SOURCE_FILE}" "${BACKUP_DIR}"
  
    TRS=`echo "${SOURCE_FILE}" | sed "y/абвгдезийклмнопрстуфхцыэ/abvgdeziyklmnoprstufhcye/"`
    TRS=`echo "${TRS}" | sed "y/АБВГДЕЗИЙКЛМНОПРСТУФХЦЫЭ№ /ABVGDEZIYKLMNOPRSTUFHCYEN_/"`
    TRS="${TRS//ч/ch}"
    TRS="${TRS//Ч/Ch}" 
    TRS="${TRS//ш/sh}"
    TRS="${TRS//Ш/Sh}"
    TRS="${TRS//ё/yo}"
    TRS="${TRS//Ё/Yo}"
    TRS="${TRS//ж/zh}"
    TRS="${TRS//Ж/Zh}"
    TRS="${TRS//щ/sch}"
    TRS="${TRS//Щ/Sch}"
    TRS="${TRS//ю/yu}"
    TRS="${TRS//Ю/Yu}"
    TRS="${TRS//я/ya}"
    TRS="${TRS//Я/Ya}"
    TRS="${TRS//ъ/}"
    TRS="${TRS//ъ/}"
    TRS="${TRS//ь/}"
    TRS="${TRS//Ь/}"

    # remove repeating _
    TRS="${TRS//__/_}"
    TRS="${TRS//__/_}"

    # lowercase
    TRS="${TRS,,}"
    # replace original file
    mv -f "${SOURCE_FILE}" "${TRS}"
#done

