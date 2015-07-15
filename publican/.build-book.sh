#!/bin/bash -i

BRAND="redhound"
BOOK_PATH="${PWD}"

export FOP_HYPHENATION_PATH="/usr/share/publican/fop/hyph/fop-hyph.jar"

cd "${BOOK_PATH}"
publican clean
publican build --formats pdf --langs ru-RU
#publican build --formats pdf,html,html-single,html-desktop --langs ru-RU
#evince "${BOOK_PATH}/tmp/ru-RU/pdf/Documentation-0.1-New_Book-ru-RU.pdf"

read # Press any key to quit...


