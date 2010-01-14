/*
 *  move.h
 *
 *  Created by Jonathan Fuerth on 2010-01-05.
 */

#ifndef __MOVE_H__
#define __MOVE_H__

#include "coordinate.h"

typedef struct move {
	coord_t *from;
	coord_t *jumped;
	coord_t *to;
} move_t;

move_t *move_new(coord_t *from, coord_t *jumped, coord_t *to);
void move_free(move_t *m);

int move_cmp(move_t *lhs, move_t *rhs);
void move_print(move_t *move);

#endif
