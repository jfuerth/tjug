CC=clang
LLVM_PROFDATA=/Library/Developer/CommandLineTools/usr/bin/llvm-profdata
CFLAGS=-std=c99 -Ofast ${COPTS}

performance: alist.o coordinate.o gamestate.o main.o memory.o move.o object.o
	$(CC) $(CFLAGS) *.o -o performance

cooked.profdata:
	rm -f performance *.o
	COPTS=-fprofile-instr-generate make performance
	./performance
	$(LLVM_PROFDATA) merge -o cooked.profdata *.profraw

performance-pgo: cooked.profdata
	rm -f performance *.o
	COPTS=-fprofile-instr-use=cooked.profdata make performance
	mv performance performance-pgo

clean:
	rm -f *.o performance performance-* *.profraw *.profdata

version:
	$(CC) --version
