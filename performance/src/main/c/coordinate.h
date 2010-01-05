/*
 *  coordinate.h
 *  
 *
 *  Created by Jonathan Fuerth on 2010-01-04.
 */

#ifndef __COORDINATE_H__
#define __COORDINATE_H__

typedef struct coord {
	int row;
	int hole;
} coord_t;

coord_t *new_coord(int row, int hole);
void free_coord(coord_t *c);
int compare_coords(coord_t *lhs, coord_t *rhs);
	
#endif

