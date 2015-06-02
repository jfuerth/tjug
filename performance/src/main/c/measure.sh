#!/usr/bin/env bash

dir=$(cd $(dirname $0); pwd -P)
cd $dir

tries=5
if [ ! -z "$1" ]; then
  tries=$1
fi

echo "Performance for $(basename $dir)"
make version

echo "Building..."
make clean
time make

echo "Running..."
for i in $(jot - 1 $tries); do
  time ./performance
done

echo "Done!"
