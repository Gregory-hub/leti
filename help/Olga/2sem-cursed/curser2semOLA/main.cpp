#include "Channels_Str.h" // ��������� ��� �������� �������
#include "Ad_Str.h" // ��������� ��� ��������� �������
#include "Broadcast_Str.h" // ��������� ��� ����� �������

#include "Create_channels.h" // ������� ��� �������� ������ ������� �� �����
#include "Create_total_structure.h" // ������� ��� �������� ������ ��������� ������� � �������
#include "Print_broadcast.h" // ������� ��� ������ ����� �������
#include "Create_broadcast_structure.h" // ������� ��� �������� ����� �������
#include "Write_broadcast.h"
#include "protocol.h"

using namespace std;

#define CHANNEL_LIST "channels.txt" // ����� ����� � ��������
#define ADS_LIST "ads.txt" // ����� ����� � ���������� ��������
#define TOTAL_LIST "broadcast.txt" // ����� �������� ����� �������


int main() {
    setlocale(LC_ALL, "Russian");
    cout << "--------------------------------" << endl;
    cout << "\n������������ ������ �������:   \n" << endl;
    reset_protocol();

    // ������� ��������� � ��������
    Channels* channels_list = create_channels(CHANNEL_LIST);
    protocol("CHANNELS", channels_list);

    // ��������� � ��������� � �������� ��������� ������
    create_total_structure(channels_list, ADS_LIST);
    protocol("CHANNELS WITH ADS", channels_list);

    // ������� ����� �������, ��������� ��������� � �������� � ���������� ��������, � ����� ���������� ���������
    // �� ���������������� ��������������� �����������.
    // �������������� ���������:
    // 1. ��������� �����, �� ������� ���������� ���������� �������
    // 2. ��������� ������, � ������� ���������� ���������� �������
    // 3. ��������� ���, � ������� �� ������������� ������� �������
    Broadcast* broadcast_struct = create_broadcast_structure(channels_list);
    protocol("BROADCAST", broadcast_struct);

    // ������������� ������������ ����� �������
    print_broadcast(broadcast_struct);

    // ���������� ������������ ����� ������� � ����
    write_broadcast(broadcast_struct, TOTAL_LIST);

    // ������� ���������
    delete channels_list;
    delete broadcast_struct;

    return 0;
}

