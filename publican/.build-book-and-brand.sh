#!/bin/bash -i

BRAND="redhound"

BOOK_PATH="${PWD}"

BRAND_PATH="/media/Data/Publican/Brands/publican-${BRAND}"
#BOOKS_PATH="/media/Data/Publican/Books"

export FOP_HYPHENATION_PATH="/usr/share/publican/fop/hyph/fop-hyph.jar"

cd "${BRAND_PATH}"
publican build --formats xml --langs all --publish
publican install_brand --path "/usr/share/publican/Common_Content"

cd "${BOOK_PATH}"
#publican build --formats pdf,html,html-single,html-desktop --langs ru-RU

publican clean

# Convert ODF formulas to svg
# TODO: parameter marshaling to csrun
export PUBLICAN_ODF_PATH="${BOOK_PATH}/ru-RU/images"
/home/redhound/.local/share/nautilus-scripts/publican/Odf-to-Svg.cs
unset PUBLICAN_ODF_PATH

cd "${BOOK_PATH}"

publican build --formats pdf --langs ru-RU
#evince "${BOOK_PATH}/tmp/ru-RU/pdf/Documentation-0.1-New_Book-ru-RU.pdf"

# TODO: copy results to Collection dir
#cp -f "${BOOK_PATH}/tmp/ru-RU/pdf/" "${BOOK_PATH}/../../Collection"

echo "Press any key to quit..."
read # Press any key to quit...


