#include "AppendBroadcast.h"
#include "Broadcast_ad_Str.h"

void appendBroadcast(Broadcast_ad** head_ref, const char* name, int duration, int minute, int hour) {

    // Выделяем память
    auto* new_node = new Broadcast_ad();

    // Ставим указатель на первый элемент
    Broadcast_ad* last = *head_ref;

    // Записываем данные
    for (int i = 0; i < 30; i++) {
        new_node->name[i] = name[i];
    }

    new_node->duration = duration;
    new_node->hour = hour;
    new_node->minute = minute;

    // Следующий элемент NULL
    new_node->next = nullptr;

    // Если первый элемент NULL, то записываем в него данные
    if (*head_ref == nullptr) {
        *head_ref = new_node;
        return;
    }

    // Ищем последний элемент и двигаем на него указатель
    while (last->next != nullptr) {
        last = last->next;
    }

    // Записываем данные после последнего элемента
    last->next = new_node;
}