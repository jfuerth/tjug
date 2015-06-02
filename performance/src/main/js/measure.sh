#!/usr/bin/env bash

dir=$(cd $(dirname $0); pwd -P)
cd $dir

tries=5
if [ ! -z "$1" ]; then
  tries=$1
fi

echo "Performance for $(basename $dir)"
echo "Node.js $(node -v)"

echo "Building..."
time cat coordinate.js move.js gamestate.js main.js > nodejs.js

echo "Running..."
for i in $(jot - 1 $tries); do
  time node nodejs.js
done

echo "Done!"
