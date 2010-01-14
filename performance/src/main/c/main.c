#include <stdio.h>
#include <sys/time.h>

#include "alist.h"
#include "coordinate.h"
#include "gamestate.h"

long gamesPlayed;

// list contains lists which contain move_t*
static alist_t *solutions;

static struct timeval startTime;
static struct timeval endTime;

static void search(gamestate_t *gs, alist_t *moveStack) {
	if (gamestate_pegs_remaining(gs) == 1) {
//		printf("Found a winning sequence. Final state:\n");
//		gamestate_print(gs);
		
		alist_add(solutions, alist_new_copy(moveStack));
		
		gamesPlayed++;
		
		return;
	}
	
	alist_t *legalMoves = gamestate_legal_moves(gs);
	
	if (alist_is_empty(legalMoves)) {
		gamesPlayed++;
		return;
	}
	
	for (int i = 0; i < legalMoves->size; i++) {
		move_t *m = alist_get(legalMoves, i);
		gamestate_t *nextState = gamestate_apply_move(gs, m);
		alist_add(moveStack, m);
		search(nextState, moveStack);
		alist_remove_last(moveStack);
	}
}

static long diff_usec(struct timeval start, struct timeval end) {
	time_t secs = end.tv_sec - start.tv_sec;
	suseconds_t usecs = end.tv_usec - start.tv_usec;
	
	return (long) (secs * 1000000L) + usecs;
}

static void run() {
	solutions = alist_new();
	
	gettimeofday(&startTime, NULL);
	
	gamestate_t *gs = gamestate_new(5, coord_new(3, 2));
	search(gs, alist_new());
	
	gettimeofday(&endTime, NULL);
	
	printf("Games played:    %6ld\n", gamesPlayed);
	printf("Solutions found: %6d\n", solutions->size);
	printf("Time elapsed:    %6ldms\n", diff_usec(startTime, endTime) / 1000);
}

int main (int argc, const char * argv[]) {
	run();
    return 0;
}
