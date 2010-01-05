/*
 *  gamestate.h
 *  
 *
 *  Created by Jonathan Fuerth on 2010-01-04.
 */

#ifndef __GAMESTATE_H__
#define __GAMESTATE_H__

typedef struct gamestate {
	int rowcount;
	alist_t occupied_holes;
} gamestate_t;

#endif
