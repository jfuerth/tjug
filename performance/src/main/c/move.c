/*
 *  move.c
 *
 *  Created by Jonathan Fuerth on 2010-01-05.
 */

#include "move.h"
#include <stdlib.h>
#include <stdio.h>

move_t *move_new(coord_t *from, coord_t *jumped, coord_t *to) {
	move_t *m = malloc(sizeof(move_t));
	if (m == NULL) {
		perror("Failed to allocate move");
		exit(1);
	}
	m->from = from;
	m->jumped = jumped;
	m->to = to;
	
	return m;
}

void move_free(move_t *m) {
	free(m);
}

int move_cmp(move_t *lhs, move_t *rhs) {
	if (lhs == rhs)
		return 0;
	if (lhs == NULL)
		return -1;
	
	int diff;
	
	diff = coord_cmp(lhs->from, rhs->from);
	if (diff != 0) {
		return diff;
	}
	
	diff = coord_cmp(lhs->jumped, rhs->jumped);
	if (diff != 0) {
		return diff;
	}
	
	diff = coord_cmp(lhs->to, rhs->to);
	if (diff != 0) {
		return diff;
	}
	
	return 0;
}

void move_print(move_t *move) {
	printf("r%dh%d -> r%dh%d -> r%dh%d\n",
		   move->from->row,   move->from->hole,
		   move->jumped->row, move->jumped->hole,
		   move->to->row,     move->to->hole); 
}

