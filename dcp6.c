#include "stdio.h"
#include "stdint.h"

/*
An XOR linked list is a more memory efficient doubly linked list. 
Instead of each node holding next and prev fields, it holds a field named both, 
which is an XOR of the next node and the previous node. 
Implement an XOR linked list; it has an add(element) which adds the element to the end, 
and a get(index) which returns the node at index.

If using a language that has no pointers (such as Python), 
you can assume you have access to get_pointer and dereference_pointer 
functions that converts between nodes and memory addresses.
*/

typedef struct {
	int payload;
	void* both;
} Node;

Node* head = NULL;
Node* tail = NULL;

void add(Node* node) {
	if (head == NULL) {
		node->both = NULL;
		head = node;
		tail = node;
	}
	else {
		tail->both = (void*)((uintptr_t)(tail->both) ^ (uintptr_t)node);
		node->both = tail;
		
		tail = node;
	}
}

Node* get(int index) {
	uintptr_t prev = 0;
	Node* cur = head;
	Node* next = NULL;
	int i = 0;
	
	if (index < 0) {
		return NULL;
	}
	
	while (cur != NULL && i < index) {
		next = (Node*)(prev ^ (uintptr_t)(cur->both));
		prev = (uintptr_t)cur;
		cur = next;
		++i;
	}
	
	return cur;
}

int main() {
	int nodeCnt = 10;
	int i;
	Node* node;
	
	for (i = 0; i < nodeCnt; ++i) {
		node = (Node*)malloc(sizeof(Node));
		node->payload = i;
		node->both = NULL;
		
		add(node);
	}
	
	for (i = -10; i < nodeCnt + 10; ++i) {
		node = get(i);
		if (node != NULL) {
			printf("%d ", node->payload);
		}
		else {
			printf("\nNode not found...");
		}
	}
	
	return 0;
}