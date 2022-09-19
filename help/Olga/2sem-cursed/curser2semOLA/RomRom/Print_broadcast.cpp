#include <iostream>
#include <iomanip>

#include "Print_broadcast.h"

using namespace std;

Broadcast* print_broadcast(Broadcast* p_begin) {
    Broadcast* p = p_begin; // Запоминаем указатель на первый элемент структуры
    // Печатаем до последнего символа
    while (p->next != nullptr) {
        cout << "--------------------------------" << endl;
        cout << "Название канала: " << p->name << endl;
        cout << "--------------------------------" << endl;
        int minute_count = 0;
        Broadcast_ad* b_ads = p->broadcast_ads;
        while (p->broadcast_ads) {
            cout << "Время начала ролика: " << setfill('0') << setw(2) << p->broadcast_ads->hour << ":"
                << setfill('0')
                << setw(2) << p->broadcast_ads->minute << " Название ролика: " << p->broadcast_ads->name <<
                " Длительность: " << p->broadcast_ads->duration << " ceк."
                << endl;
            minute_count++;
            p->broadcast_ads = p->broadcast_ads->next; // переходим на следующий ролик
        }
        p->broadcast_ads = b_ads;
        if (minute_count < 59) {
            cout << "Трансляция была прервана..." << endl;
        }
        p = p->next; // Переходим на следующий канал
    }
    return p; // возвращаем указатель на первый элемент
}