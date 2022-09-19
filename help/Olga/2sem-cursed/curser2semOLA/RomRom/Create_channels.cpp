#include "Create_channels.h"
#include <iostream>
#include <fstream>

using namespace std;

Channels* create_channels(const char* filename) {
    setlocale(LC_ALL, "Russian");
    // ��������� ����
    ifstream readfile(filename, ios::in);
    // ������ ��� �������, ������� �������
    readfile >> std::noskipws;

    Channels* p_begin = nullptr;
    Channels* p = nullptr;
    // �������� ������
    p_begin = new Channels(); // �������� ������ ��� ������ ������ ���������
    p = p_begin;
    p->next = nullptr;

    char id[30]{};

    if (!readfile) {
        cout << "������ �������� �����" << endl;
        return reinterpret_cast<Channels*>(-1);
    }
    else {
        while (true) {
            readfile.getline(id, 30); // ������ ���� ���������

            if (!readfile.eof()) {
                unsigned int limit1 = 0;
                unsigned int limit2 = 0;

                size_t i = 0;

                for (auto x : id) { // ���� ������������, � ������� ������� ��������� ������
                    if (id[i] == ':') {
                        limit1 = i;
                    }
                    if (id[i] == ';') {
                        limit2 = i;
                    }
                    i++;
                }

                char channel_id;

                char name[30]{};

                int name_count = 0;

                for (int k = 0; k < limit2; k++) {
                    if (k < limit1) {
                        channel_id = id[k];
                    }
                    if (k > limit1 && k < limit2) {
                        name[name_count] = id[k];
                        name_count++;
                    }
                }

                p->id = int(channel_id) - 48; // ������������ ������� � �����

                for (int n = 0; n < 30; n++) {
                    p->name[n] = name[n];
                }

                p->next = new Channels(); // �������� ������ �� ��������� �������
                p = p->next; // ��������� � ���������� ��������

                p->next = nullptr;
            }
            else break;
        }

        readfile.close();
        return p_begin;
    }
}