#pragma once
// ��������� ��� ��������� �������
struct Ad {
    int id;
    char name[30];
    int duration;
    int sequence;

    Ad* next;
};