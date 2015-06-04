/*
 *  gamestate.c
 *  
 * Created by Jonathan Fuerth on 2010-01-04.
 *
 * Copyright (c) 2010, Jonathan Fuerth
 * 
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in
 *       the documentation and/or other materials provided with the
 *       distribution.
 *     * Neither the name of Jonathan Fuerth nor the names of other
 *       contributors may be used to endorse or promote products derived
 *       from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#include <stdlib.h>
#include <stdio.h>
#include "alist.h"
#include "gamestate.h"
#include "coordinate.h"

gamestate_t gamestate_new(int rows, coord_t empty_hole) {
	gamestate_t gs;
	memzero(&gs, sizeof(gs));

	gs.rowcount = rows;

	for (int row = 1; row <= rows; row++) {
		for (int hole = 1; hole <= row; hole++) {
			coord_t *peg = coord_new(row, hole);
			if (coord_cmp(peg, empty_hole) != 0) {
				coord_array_add(gs.occupied_holes, peg);
			}
		}
	}
	
	return gs;
}

gamestate_t gamestate_apply_move(gamestate_t gs, move_t move) {
	gamestate_t newgs = gs;

//	printf("Copied game state:\n");
//	gamestate_print(newgs);

	if (!coord_list_remove(newgs.occupied_holes, move.from)) {
		printf("Move is not consistent with game state: 'from' hole was unoccupied.\n");
		exit(1);
	}
//	printf("index of jumped hole in list: %d\n",
//		   alist_index_of(newgs->occupied_holes, move->jumped, coord_cmp));
	if (!alist_remove(newgs.occupied_holes, move.jumped)) {
		printf("Move is not consistent with game state: jumped hole was unoccupied.\n");
		printf("Old game state:\n");
		gamestate_print(gs);
		printf("Attempted move: ");
		move_print(move);
		printf("New game state ('from' hole should now be empty):\n");
		gamestate_print(newgs);
		exit(1);
	}
	if (coord_list_contains(newgs.occupied_holes, move.to)) {
		printf("Move is not consistent with game state: 'to' hole was occupied.\n");
		exit(1);
	}
	if (move.to.row > newgs.rowcount || move.to.row < 1) {
		printf("Move is not legal because the 'to' hole does not exist: row %d, hole %d\n",
			   move->to->row, move->to->hole);
	}
	coord_list_add(newgs->occupied_holes, move->to);
	
	return newgs;
}

int gamestate_legal_moves(gamestate_t gs, move_t[] target, int target_size) {
	move *legalMoves = target;
	for (int i = 0; i < gs.num_occupied_holes; i++) {
		coord_t c = gs.occupied_holes[i];
		
		move_t possibleMoves[MAX_POSSIBLE_MOVES]
		coord_possible_moves(c, gs->rowcount);
		for (int j = 0; j < possibleMoves->size; j++) {
			move_t *m = alist_get(possibleMoves, j);
			if (alist_contains(gs->occupied_holes, m->jumped, coord_cmp) && !alist_contains(gs->occupied_holes, m->to, coord_cmp)) {
				alist_add(legalMoves, m);
			}
		}
	}
	return legalMoves;
}

int gamestate_pegs_remaining(gamestate_t *gs) {
	return gs->occupied_holes->size;
}

void gamestate_print(gamestate_t *gs) {
	printf("Game with %d pegs:\n", gamestate_pegs_remaining(gs));
	for (int row = 1; row <= gs->rowcount; row++) {
		int indent = gs->rowcount - row;
		for (int i = 0; i < indent; i++) {
			printf(" ");
		}
		for (int hole = 1; hole <= row; hole++) {
			coord_t *this_coord = coord_new(row, hole);
			if (alist_contains(gs->occupied_holes, this_coord, coord_cmp)) {
				printf(" *");
			} else {
				printf(" O");
			}
			coord_free(this_coord);
		}
		printf("\n");
	}
}

