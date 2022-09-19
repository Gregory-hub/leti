#include "del.h"


void del(Ad* ads) {
	if (ads == nullptr) return;
	del(ads->next);
	delete ads;
}


void del(Broadcast_ad* ads) {
	if (ads == nullptr) return;
	del(ads->next);
	delete ads;
}


void del(Broadcast* b) {
	if (b == nullptr) return;
	del(b->next);
	delete b;
}


void del(Channels* c) {
	if (c == nullptr) return;
	del(c->next);
	delete c;
}

