/*
 *  object.c
 *
 *  Created by Jonathan Fuerth on 2010-01-12.
 */

#include "object.h"
#include <stdlib.h>
#include <stdio.h>

object_t *obj_create(void *data,
					int (*compare)(void *lhs, void *rhs),
					void (*destroy)(void *data)) {
	object_t *object = malloc(sizeof(object_t));
	if (object == NULL) {
		printf("Unable to allocate an object header\n");
		exit(1);
	}
	object->refcount = 1;
	object->compare = compare;
	object->destroy = destroy;
	object->data = data;
	
	return object;
}

// increments given object's reference count
void obj_retain(object_t *object) {
	if (object->refcount <= 0) {
		printf("Reference error: attempt to retain an invalid or freed object\n");
		exit(1);
	}
	object->refcount = object->refcount + 1;
}

// decrements given object's reference count, freeing the
// object_t and its contained object if refcount becomes 0
void obj_release(object_t *object) {
	if (object->refcount <= 0) {
		printf("Reference error: attempt to release an invalid or freed object\n");
		exit(1);
	}
	object->refcount = object->refcount - 1;
	if (object->refcount == 0) {
		object->destroy(object->data);
		object->data = NULL;
		free(object);
	}
}

int obj_compare(object_t *lhs, object_t *rhs) {
	if (lhs->compare != rhs->compare) {
		printf("Null comparisons not allowed (lhs=%p, rhs=%p)\n", lhs, rhs);
		exit(1);
	}
	if (lhs->compare != rhs->compare) {
		printf("Illegal comparison attempted (objects have different compare functions)\n");
		exit(1);
	}
	return lhs->compare(lhs->data, rhs->data);
}
