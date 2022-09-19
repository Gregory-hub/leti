#include "AppendBroadcast.h"
#include "Broadcast_ad_Str.h"

void appendBroadcast(Broadcast_ad** head_ref, const char* name, int duration, int minute, int hour) {

    // �������� ������
    auto* new_node = new Broadcast_ad();

    // ������ ��������� �� ������ �������
    Broadcast_ad* last = *head_ref;

    // ���������� ������
    for (int i = 0; i < 30; i++) {
        new_node->name[i] = name[i];
    }

    new_node->duration = duration;
    new_node->hour = hour;
    new_node->minute = minute;

    // ��������� ������� NULL
    new_node->next = nullptr;

    // ���� ������ ������� NULL, �� ���������� � ���� ������
    if (*head_ref == nullptr) {
        *head_ref = new_node;
        return;
    }

    // ���� ��������� ������� � ������� �� ���� ���������
    while (last->next != nullptr) {
        last = last->next;
    }

    // ���������� ������ ����� ���������� ��������
    last->next = new_node;
}