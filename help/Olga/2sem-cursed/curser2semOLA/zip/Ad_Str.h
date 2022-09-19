#pragma once
// Структура для рекламных роликов
struct Ad {
    int id;
    char name[30];
    int duration;
    int sequence;

    Ad* next;
};