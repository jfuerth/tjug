#!/usr/bin/env bash

dir=$(cd $(dirname $0); pwd -P)
cd $dir

tries=5
if [ ! -z "$1" ]; then
  tries=$1
fi

echo "Performance for $(basename $dir)"
go version

echo "Building..."
export GOPATH=$dir
rm -f performance
time go build performance

echo "Running..."
for i in $(jot - 1 $tries); do
  time ./performance
done

echo "Done!"
