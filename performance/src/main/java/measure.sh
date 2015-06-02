#!/usr/bin/env bash

dir=$(cd $(dirname $0); pwd -P)
cd $dir

tries=5
if [ ! -z "$1" ]; then
  tries=$1
fi

echo "Performance for $(basename $dir)"
java -version

echo "Building..."
rm -rf target
mkdir target
time javac -d target ca/tjug/talks/performance/SolutionFinder.java

echo "Running..."
for i in $(jot - 1 $tries); do
  time java -cp target ca.tjug.talks.performance.SolutionFinder
done

echo "Done!"
