#pragma once
#include "imports.h"
// —труктура дл€ рекламных роликов в сетке вещани€
struct Broadcast_ad {
    char name[30];
    int duration;
    int hour;
    int minute;

    Broadcast_ad* next;
};