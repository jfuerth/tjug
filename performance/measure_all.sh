#!/usr/bin/env bash

tries=5
if [ ! -z "$1" ]; then
  tries=$1
fi

echo "Measuring each language ${tries} times"

for language in src/main/*; do
  $language/measure.sh $tries | awk "{print \"${language}: \" \$0}"
done

