#include <iomanip>
#include "fstream"
#include "Write_broadcast.h"

using namespace std;

void write_broadcast(Broadcast* broadcast_struct, const char* filename) {
    setlocale(LC_ALL, "Russian");
    ofstream writefile(filename, ios_base::trunc);
    Broadcast* p = broadcast_struct;

    while (p->next != nullptr) {
        writefile << "--------------------------------" << endl;
        writefile << "�������� ������: " << p->name << endl;
        writefile << "--------------------------------" << endl;
        int minute_count = 0; // ������� �����
        while (p->broadcast_ads) {
            // ���������� ��������� ������� � ����
            writefile << "����� ������ ������: " << setfill('0') << setw(2) << p->broadcast_ads->hour << ":"
                << setfill('0')
                << setw(2) << p->broadcast_ads->minute << " �������� ������: " << p->broadcast_ads->name <<
                " ������������: " << p->broadcast_ads->duration << " ce�."
                << endl;
            minute_count++;
            p->broadcast_ads = p->broadcast_ads->next;
        }
        // ���� ������� ������� ������ 59 �����, �� ������ ���������� ���� ��������
        if (minute_count < 59) {
            writefile << "���������� ���� ��������..." << endl;
        }
        p = p->next;
    }

    writefile.close();
}