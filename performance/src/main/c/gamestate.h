/*
 *  gamestate.h
 *  
 *
 *  Created by Jonathan Fuerth on 2010-01-04.
 */

#ifndef __GAMESTATE_H__
#define __GAMESTATE_H__

#include "alist.h"
#include "coordinate.h"
#include "move.h"

typedef struct gamestate {
	int rowcount;
	alist_t *occupied_holes;
} gamestate_t;

gamestate_t *gamestate_new(int rows, coord_t *empty_hole);
void gamestate_free(gamestate_t *gs);

gamestate_t *gamestate_apply_move(gamestate_t *gs, move_t *move);
alist_t *gamestate_legal_moves(gamestate_t *gs);
int gamestate_pegs_remaining(gamestate_t *gs);
void gamestate_print(gamestate_t *gs);

#endif
