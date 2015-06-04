#!/usr/bin/env bash

dir=$(cd $(dirname $0); pwd -P)
cd $dir

tries=5
if [ ! -z "$1" ]; then
  tries=$1
fi

echo "Performance for $(basename $dir)"
jjs -fv </dev/null

echo "Building..."
time cat ../js/coordinate.js ../js/move.js ../js/gamestate.js ../js/main.js \
    | sed s/console.log/print/ \
    > nashorn.js

echo "Running..."
for i in $(jot - 1 $tries); do
  time jjs nashorn.js
done

echo "Done!"
