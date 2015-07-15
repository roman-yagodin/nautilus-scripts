#!/bin/bash

pushd

XSLTPROC_PATH="/opt/serna-free-4.3/bin/xsltproc"
XSL_STYLESHEET="/opt/serna-free-4.3/xml/stylesheets/docbook-xsl-1.75.2/xhtml/docbook-my.xsl"
CSS_FILE="default-big.css"
SOURCE_DIR="$PWD"
SOURCE_FILE="${1##*/}" 
OUTPUT_DIR="$SOURCE_DIR/release"
XSL_OUTPUT_FILE="$OUTPUT_DIR/$SOURCE_FILE.~xsl"
AWK_OUTPUT_FILE="$OUTPUT_DIR/$SOURCE_FILE.~awk"
OUTPUT_FILE="$OUTPUT_DIR/$SOURCE_FILE.html"
LOG="build.log"
#SOURCE_DIR="${1%/*}"

mkdir "$OUTPUT_DIR"
echo "Transforming $SOURCE_FILE with $XSL_STYLESHEET" > $LOG

mkdir "$OUTPUT_DIR/css" >> $LOG
mkdir "$OUTPUT_DIR/images" >> $LOG

cp -f "$SOURCE_DIR/../common/css/$CSS_FILE" "$OUTPUT_DIR/css/default.css" >> $LOG
cp -f -r "$SOURCE_DIR/images/" "$OUTPUT_DIR" >> $LOG

"$XSLTPROC_PATH" --xinclude -o \
   "$XSL_OUTPUT_FILE" "$XSL_STYLESHEET" "$SOURCE_DIR/$SOURCE_FILE" >> $LOG

# postprocessing using awk
cat "$XSL_OUTPUT_FILE" | awk '{gsub(/programlisting/,"programlisting brush: c-sharp;")}; 1' > "$AWK_OUTPUT_FILE"

mv -f "$AWK_OUTPUT_FILE" "$OUTPUT_FILE"
mv -f "$LOG" "$OUTPUT_DIR"

#exec nautilus "$OUTPUT_DIR"
#exec sensible-browser "$OUTPUT_FILE"
exec firefox "$OUTPUT_FILE"
popd
