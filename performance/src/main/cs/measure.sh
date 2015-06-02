#!/usr/bin/env bash

dir=$(cd $(dirname $0); pwd -P)
cd $dir

tries=5
if [ ! -z "$1" ]; then
  tries=$1
fi

echo "Performance for $(basename $dir)"
mono --version

echo "Building..."
rm -f Performance.exe
time mcs -out:Performance.exe *.cs

echo "Running..."
for i in $(jot - 1 $tries); do
  time mono Performance.exe
done

echo "Done!"
