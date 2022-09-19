#include "Create_broadcast_structure.h"
#include "Generator_Str.h"
#include "Gen_add_parameter.h"
#include "Ad_Str.h"
#include "random"
#include "AppendBroadcast.h"

Broadcast* create_broadcast_structure(Channels* p_begin) {
    setlocale(LC_ALL, "Russian");
    Channels* p = p_begin;

    // �������������� ���������, � ������� ����� ���������� �������
    Broadcast* b_begin;
    b_begin = nullptr;
    Broadcast* b;
    // �������� ������
    b_begin = new Broadcast();
    b = b_begin;
    b->next = nullptr;

    // ���������� �������������� ���������, ���������� ��� ������������ �������
    Generator rand_param = gen_add_parameter(p);
    int random_hour = rand_param.random_hour;

    while (p != nullptr) {
        // ������������ �������� �������
        int name_count = 0;
        for (char t : p->name) {
            b->name[name_count] = t;
            name_count++;
        }

        int position_count = 0;

        int ads_count = 0;

        int ads_priority = 0;

        Ad* a = p->ads; // ���������� ������ ������ ��������� �������

        int ads_arr[30];

        // ������ ��� ������� ������ � ������������ ���������� ������� � ������ ������, ����� ������� ����� � ������������ �����������
        while (p->ads != nullptr) {

            if (p->ads->sequence > ads_priority) {
                ads_priority = p->ads->sequence;
            }
            ads_arr[ads_count] = p->ads->id;
            ads_count++;

            p->ads = p->ads->next;
        }
        p->ads = a;


        int broadcasting_count = 0;

        int priority = ads_priority;

        int current_id = 0;

        // ���� ��������������� ����� ��������� � �������, �� ����������� ����������� �� �������
        int iter_count = rand_param.random_channel == p->id ? rand_param.random_minute : 60;

        if (ads_count == 1) {
            appendBroadcast(&(b->broadcast_ads), p->ads->name, p->ads->duration, position_count, random_hour);
        }

        while (broadcasting_count < iter_count && p->ads != nullptr && ads_count > 1) {
            int rand_ads = ads_arr[rand() % ads_count];

            // ���� ��������� ����� �� ������� ����� �������� � ��� ��������� ������ ����������� � ������� id ������ �� ����� �����������
            if (rand_ads == p->ads->id && priority > p->ads->sequence && current_id != rand_ads) {
                // ���������� ������ ������� � ����� � ������� �������
                appendBroadcast(&(b->broadcast_ads), p->ads->name, p->ads->duration, position_count, random_hour);

                priority = p->ads->sequence;
                current_id = p->ads->id;
                broadcasting_count++;
                position_count++;
            }
            
            p->ads = p->ads->next;
            if (p->ads == nullptr) {
                priority = ads_priority;
                p->ads = a;
            }
        }
        if (p->next != nullptr) {
            b->next = new Broadcast();
            b = b->next;
        }
        p = p->next;
    }

    return b_begin;
}