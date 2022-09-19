#include "Channels_Str.h"
#include "Generator_Str.h"

using namespace std;

Generator gen_add_parameter(Channels* p_begin) {
    // ��������� ��������������� �����, ��� ����������� ����, � ������� ����� ����������� �������
    random_device rd;
    mt19937 mt(rd());
    // ���������� ���, � ������� ����� ������������� �������
    uniform_int_distribution<int> dist(0, 23.0);

    // ��������� ������, � ������� ���������� ���������� �������
    uniform_int_distribution<int> minute(1, 59);

    int random_hour = dist(mt);

    int random_minute = minute(mt);

    // ������� ���������� ������� � ���������
    int channel_count = 0;
    while (p_begin->next != nullptr) {
        p_begin = p_begin->next;
        channel_count++;
    }

    // ���������� id ������, �� ������� ���������� ���������� �������
    uniform_int_distribution<int> channel(1, channel_count);

    int random_channel = channel(mt);

    struct Generator Gen_param;

    // ���������� ��������������� ������ � ���������
    Gen_param.random_channel = random_channel;
    Gen_param.random_minute = random_minute;
    Gen_param.random_hour = random_hour;

    return Gen_param;
}