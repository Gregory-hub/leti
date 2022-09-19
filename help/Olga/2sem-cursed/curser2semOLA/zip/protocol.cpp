#include "protocol.h"


void reset_protocol() {
	fstream file;
	file.open(PROTOCOL, ios::out);
	if (!file.is_open()) {
		perror("Error opening file");
		exit(1);
	}
	file.close();
}


void log_ad(fstream& file, Ad* ad) {
	file << ad->id << ':';
	for (int i = 0; i < strlen(ad->name); i++) {
		file << ad->name[i];
	}
	file << ':';
	file << ad->duration << "s:";
	file << ad->sequence << " -> ";
}


void log_broadcast_ad(fstream& file, Broadcast_ad* ad) {
	for (int i = 0; i < strlen(ad->name); i++) {
		file << ad->name[i];
	}
	file << ':';
	file << ad->duration << "s:";
	file << ad->hour << "h:" << ad->minute << "m -> ";
}


void log_channel(fstream& file, Channels* channel, int num) {
	if (num != 0) {
		file << "-> ";
	}
	else {
		file << "   ";
	}
	file << "'";
	for (int i = 0; i < strlen(channel->name); i++) {
		file << channel->name[i];
	}
	file << "': ";

	Ad* ad = channel->ads;
	while (ad != nullptr) {
		log_ad(file, ad);
		ad = ad->next;
	}
	file << "NULL" << endl;
}


void log_broadcast(fstream& file, Broadcast* broadcast, int num) {
	if (num != 0) {
		file << "-> ";
	}
	else {
		file << "   ";
	}
	file << "'";

	for (int i = 0; i < strlen(broadcast->name); i++) {
		file << broadcast->name[i];
	}
	file << "': ";

	Broadcast_ad* ad = broadcast->broadcast_ads;
	while (ad != nullptr) {
		log_broadcast_ad(file, ad);
		ad = ad->next;
	}
	file << "NULL" << endl;
}


void protocol(string text, Channels* channels) {
	fstream file;
	file.open(PROTOCOL, ios::app);
	if (!file.is_open()) {
		perror("Error opening file");
		exit(1);
	}

	file << text << endl;

	int num = 0;
	Channels* channels_head = channels;
	while (channels != nullptr) {
		log_channel(file, channels, num);
		channels = channels->next;
		num++;
	}
	channels = channels_head;

	if (channels != nullptr) {
		file << "-> NULL" << endl;
	}
	file << endl;

	file.close();
}


void protocol(string text, Broadcast* broadcast) {
	fstream file;
	file.open(PROTOCOL, ios::app);
	if (!file.is_open()) {
		perror("Error opening file");
		exit(1);
	}

	file << text << endl;

	int num = 0;
	Broadcast* broadcast_head = broadcast;
	while (broadcast != nullptr) {
		log_broadcast(file, broadcast, num);
		broadcast = broadcast->next;
		num++;
	}
	broadcast = broadcast_head;

	if (broadcast != nullptr) {
		file << "-> NULL" << endl;
	}
	file << endl;

	file.close();
}

