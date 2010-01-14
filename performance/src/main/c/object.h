/*
 *  object.h
 *
 *  Created by Jonathan Fuerth on 2010-01-12.
 */

#ifndef __OBJECT_H__
#define __OBJECT_H__

typedef struct object {
	int refcount;                          // number of retained references
	int (*compare)(void *lhs, void *rhs);  // function to compare this.obj to another.obj
	void (*destroy)(void *data);           // frees data and anything data references
	void *data;                            // the object state itself
} object_t;

// creates a new object that references data with
// an initial reference count of 1
object_t *obj_create(void *data,
					int (*compare)(void *lhs, void *rhs),
					void (*destroy)(void *data));

// increments given object's reference count
void retain(object_t *object);

// decrements given object's reference count, freeing the
// object_t and its contained object 
void release(object_t *object);

int obj_compare(object_t *lhs, object_t *rhs);

#endif
