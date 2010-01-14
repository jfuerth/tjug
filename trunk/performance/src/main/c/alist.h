/*
 *  alist.h
 *
 *  Created by Jonathan Fuerth on 2010-01-04.
 *
 * Provides the subset of Java's ArrayList features which
 * are required for the performance tests.
 */

#ifndef __ALIST_H__
#define __ALIST_H__

typedef struct alist {
	int size;
	int capacity;
	void **entries;
} alist_t;

alist_t *alist_new();
alist_t *alist_new_sized(int initial_capacity);
alist_t *alist_new_copy(alist_t *src);
void alist_free(alist_t *list);

void alist_add(alist_t *list, void *item);
void alist_remove_at(alist_t *list, int idx);

/* Returns 1 if item was removed; 0 if item was not found */
int alist_remove(alist_t *list, void *item, int (*comparator)(void *, void *));

void alist_remove_last(alist_t *list);
void *alist_get(alist_t *list, int idx);
int alist_contains(alist_t *list, void *item, int (*comparator)(void *, void *));
int alist_index_of(alist_t *list, void *item, int (*comparator)(void *, void *));
int alist_size(alist_t *list);
int alist_is_empty(alist_t *list);

#endif
