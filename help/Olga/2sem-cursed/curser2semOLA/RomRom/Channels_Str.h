#pragma once
// ��������� ��� �������� �������
struct Channels {
    int id;
    char name[30];

    struct Ad* ads;

    Channels* next;
};