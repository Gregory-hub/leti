#pragma once
#include "Broadcast_ad_Str.h"
// Функция для добавления нового узла в конец для сетки вещания
void appendBroadcast(Broadcast_ad** head_ref, const char* name, int duration, int minute, int hour);