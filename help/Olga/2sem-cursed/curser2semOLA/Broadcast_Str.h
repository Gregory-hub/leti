#pragma once
#include "Broadcast_ad_Str.h"

// ��������� ��� ����� �������
struct Broadcast {
    char name[30];

    Broadcast_ad* broadcast_ads;

    Broadcast* next;
};