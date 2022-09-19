#include <iostream>
#include <iomanip>

#include "Print_broadcast.h"

using namespace std;

Broadcast* print_broadcast(Broadcast* p_begin) {
    Broadcast* p = p_begin; // ���������� ��������� �� ������ ������� ���������
    // �������� �� ���������� �������
    while (p->next != nullptr) {
        cout << "--------------------------------" << endl;
        cout << "�������� ������: " << p->name << endl;
        cout << "--------------------------------" << endl;
        int minute_count = 0;
        Broadcast_ad* b_ads = p->broadcast_ads;
        while (p->broadcast_ads) {
            cout << "����� ������ ������: " << setfill('0') << setw(2) << p->broadcast_ads->hour << ":"
                << setfill('0')
                << setw(2) << p->broadcast_ads->minute << " �������� ������: " << p->broadcast_ads->name <<
                " ������������: " << p->broadcast_ads->duration << " ce�."
                << endl;
            minute_count++;
            p->broadcast_ads = p->broadcast_ads->next; // ��������� �� ��������� �����
        }
        p->broadcast_ads = b_ads;
        if (minute_count < 59) {
            cout << "���������� ���� ��������..." << endl;
        }
        p = p->next; // ��������� �� ��������� �����
    }
    return p; // ���������� ��������� �� ������ �������
}