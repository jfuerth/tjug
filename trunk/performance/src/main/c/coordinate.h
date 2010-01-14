/*
 *  coordinate.h
 *  
 *
 *  Created by Jonathan Fuerth on 2010-01-04.
 */

#ifndef __COORDINATE_H__
#define __COORDINATE_H__

#include "alist.h"

typedef struct coord {
	int row;
	int hole;
} coord_t;

coord_t *coord_new(int row, int hole);
void coord_free(coord_t *c);

int coord_cmp(coord_t *lhs, coord_t *rhs);

/*
 * Returns a list of move_t which enumerates the possible moves from
 * this coordinate. The caller is responsible for freeing both the
 * returned list and all the items in it.
 */
alist_t *coord_possible_moves(coord_t *c, int rowCount);

#endif

