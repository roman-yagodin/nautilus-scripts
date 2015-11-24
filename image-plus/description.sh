#!/bin/bash

pushd

SOURCE_FILE="$1"
SOURCE_DIR="$PWD"
OUTPUT_FILE="$SOURCE_FILE.xml"

#FEXT=${1##*/}
FNAME=${1%.*}
ALT="$FNAME" 

echo "﻿<?xml version=\"1.0\" encoding=\"UTF-8\"?><ImageTextInfo xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Alt>$ALT</Alt><Title>$ALT</Title></ImageTextInfo>" >> "$OUTPUT_FILE"

popd
