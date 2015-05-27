#CC=gcc
CC=clang
CFLAGS=-std=c99 -O3 -funroll-all-loops -fomit-frame-pointer
#CFLAGS=-std=c99 -fast

performance: alist.o coordinate.o gamestate.o main.o memory.o move.o object.o
	$(CC) $(CFLAGS) *.o -o performance

clean:
	rm -f *.o
	rm -f performance
