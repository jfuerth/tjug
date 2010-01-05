#include "coordinate.h"
#include <stdlib.h>

coord_t *new_coord(int row, int hole) {
	coord_t *c = malloc(sizeof(coord_t));
	if (c == NULL) return NULL;
	c->row = row;
	c->hole = hole;
	return c;
}

void free_coord(coord_t *c) {
	free(c);
}

int compare_coords(coord_t *lhs, coord_t *rhs) {
	if (lhs->row < rhs->row) return -1;
	if (lhs->row > rhs->row) return 1;
	
	if (lhs->hole < rhs->hole) return -1;
	if (lhs->hole > rhs->hole) return 1;
	
	return 0;
}
