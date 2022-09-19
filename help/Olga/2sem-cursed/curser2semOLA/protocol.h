#pragma once
#include <string>
#include <fstream>
#include "Broadcast_ad_Str.h"
#include "Broadcast_Str.h"
#include "Ad_Str.h"
#include "Channels_Str.h"

#define PROTOCOL "protocol.txt"

using namespace std;

void protocol(string text, Channels* channels = nullptr);
void protocol(string text, Broadcast* broadcast = nullptr);
void reset_protocol();

