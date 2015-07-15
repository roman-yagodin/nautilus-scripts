#!/bin/bash -vi

echo "== Enter name for a new article:"
read ARTICLE
BRAND="redhound"
BOOKS_PATH="${PWD}"
BRAND_PATH="/media/Data/Publican/Brands/publican-${BRAND}"

#BRAND_PATH="/media/Data/Publican/Brands/publican-${BRAND}"
#BOOKS_PATH="/media/Data/Publican/Books"

cd "${BOOKS_PATH}"
publican create --name "${ARTICLE}" --type article --brand "${BRAND}" --lang ru-RU
cd "${ARTICLE}/ru-RU"
mkdir -p "extras"
mkdir -p "files"
ln --symbolic "${BRAND_PATH}/ru-RU" "${BOOKS_PATH}/${ARTICLE}/ru-RU/Common_Content"

read # Press any key to quit...

