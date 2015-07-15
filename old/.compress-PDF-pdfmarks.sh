#! /bin/bash

# Extract metadata from the original PDF
pdfinfo "$1" | sed -e 's/^ *//;s/ *$//;s/ \{1,\}/ /g' -e 's/^/  \//' -e '/CreationDate/,$d' -e 's/$/)/' -e 's/: / (/' > .pdfmarks
sed -i '1s/^ /[/' .pdfmarks
sed -i '/:)$/d' .pdfmarks
echo "  /DOCINFO pdfmark" >> .pdfmarks
