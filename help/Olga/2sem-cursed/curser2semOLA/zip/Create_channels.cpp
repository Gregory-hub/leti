#include "Create_channels.h"


using namespace std;


Channels* read_one_channel(string line) {
	stringstream ss(line);
    string id_str = "";
    string name = "";
	getline(ss, id_str, ':');
	if (id_str == "") return nullptr;
	getline(ss, name);

	Channels* new_channel = new Channels();
	bool is_number = true;
	if (id_str.length() == 0) is_number = false;
	for (int i = 0; i < id_str.length(); i++) {
		if (!isdigit(id_str[i]) && !(i == 0 && id_str[i] == '-')) {
			is_number = false;
		}
	}

	if (is_number && stoi(id_str) > 0) {
		new_channel->id = stoi(id_str);
	}
	else {
		cerr << "Error: invalid data" << endl;
		exit(1);
	}
	if (name != "") {
		for (int i = 0; i < name.length(); i++) {
			new_channel->name[i] = name[i];
		}
	}
	else {
		cerr << "Error: invalid data" << endl;
		exit(1);
	}
	return new_channel;
}


Channels* create_channels(const char* filename) {
    ifstream file;
    file.open(filename);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    string line;
	getline(file, line);
	if (line == "") return nullptr;

	Channels* channel_head = read_one_channel(line);
    Channels* channel = channel_head;
    channel->next = nullptr;

	Channels* new_channel = nullptr;

    while (getline(file, line)) {
		new_channel = read_one_channel(line);

        channel->next = new_channel;
        channel = new_channel;
        channel->next = nullptr;
    }
    
	file.close();
	return channel_head;
}