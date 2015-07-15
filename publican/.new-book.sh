#!/bin/bash -vi

echo "== Enter name for a new book:"
read BOOK
BRAND="redhound"
BOOKS_PATH="${PWD}"
BRAND_PATH="/media/Data/Publican/Brands/publican-${BRAND}"

#BRAND_PATH="/media/Data/Publican/Brands/publican-${BRAND}"
#BOOKS_PATH="/media/Data/Publican/Books"

cd "${BOOKS_PATH}"
publican create --name "${BOOK}" --brand "${BRAND}" --lang ru-RU
cd "${BOOK}/ru-RU"
mkdir -p "extras"
mkdir -p "files"
ln --symbolic "${BRAND_PATH}/ru-RU" "${BOOKS_PATH}/${BOOK}/ru-RU/Common_Content"

read # Press any key to quit...

