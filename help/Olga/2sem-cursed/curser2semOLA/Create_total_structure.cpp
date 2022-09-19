#include "Channels_Str.h"
#include "Create_channels.h"
#include "Ad_Str.h"


using namespace std;


bool is_number(string num) {
	if (num.length() == 0) return false;
	for (int i = 0; i < num.length(); i++) {
		if (!isdigit(num[i]) && !(i == 0 && num[i] == '-')) {
			return false;
		}
	}
    return true;
}


Ad* read_one_ad(stringstream& ss) {
    Ad* ad = new Ad();
    ad->next = nullptr;

    string id_str = "";
    string name = "";
    string duration_str = "";
    string sequence_str = "";

    getline(ss, id_str, ':');
    if (is_number(id_str) && stoi(id_str) > 0) {
        ad->id = stoi(id_str);
    }

    getline(ss, name, ':');
	if (name != "") {
		for (int i = 0; i < name.length(); i++) {
			ad->name[i] = name[i];
		}
	}
	else {
		cerr << "Error: invalid data" << endl;
		exit(1);
	}
    
    getline(ss, duration_str, ':');
    if (is_number(duration_str) && stoi(duration_str) > 0) {
        ad->duration = stoi(duration_str);
    }

    getline(ss, sequence_str, ':');
    if (is_number(sequence_str) && stoi(sequence_str) > 0) {
        ad->sequence = stoi(sequence_str);
    }

    return ad;
}


void add_ad_to_channel(Channels* channels, Ad* ad) {
    Ad* new_ad = new Ad();
    new_ad->id = ad->id;
    for (int i = 0; i < strlen(ad->name); i++) {
        new_ad->name[i] = ad->name[i];
    }
    new_ad->duration = ad->duration;
    new_ad->sequence = ad->sequence;
    new_ad->next = nullptr;

    if (channels->ads == nullptr) {
        channels->ads = new_ad;
    }
    else {
        Ad* ad_head = channels->ads;
        while (channels->ads->next != nullptr) {
            channels->ads = channels->ads->next;
        }
        channels->ads->next = new_ad;
        channels->ads = ad_head;
    }
}


void create_total_structure(Channels* channels, const char* filename) {
    ifstream file;
    file.open(filename);
    if (!file.is_open()) {
        perror("Error opening file");
        exit(1);
    }

    Channels* channels_head = channels;

	string channel_num_str = "";
	int channel_num = -1;
    string line;
    while (getline(file, line)) {
		stringstream ss(line);
		Ad* ad = read_one_ad(ss);

        while (getline(ss, channel_num_str, ',')) {
            if (channel_num_str == "") break;
            if (!(is_number(channel_num_str) && stoi(channel_num_str) > 0)) {
				cerr << "Error: invalid data" << endl;
				exit(1);
            }
            channel_num = stoi(channel_num_str);
            while (channels != nullptr) {
                if (channel_num == channels->id) {
                    add_ad_to_channel(channels, ad);
                    break;
                }
                channels = channels->next;
            }
			channels = channels_head;
        }
        delete ad;
    }

	file.close();
}
