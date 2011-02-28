CC=gcc
CFLAGS=-std=c99

performance: alist.o coordinate.o gamestate.o main.o memory.o move.o object.o
	gcc $(CFLAGS) *.o -o performance

clean:
	rm -f *.o
	rm -f performance
