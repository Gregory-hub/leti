#include "Create_channels.h"
#include <iostream>
#include <fstream>

using namespace std;

Channels* create_channels(const char* filename) {
    setlocale(LC_ALL, "Russian");
    // Открываем файл
    ifstream readfile(filename, ios::in);
    // Читаем все символы, включая пробелы
    readfile >> std::noskipws;

    Channels* p_begin = nullptr;
    Channels* p = nullptr;
    // Выделяем память
    p_begin = new Channels(); // Выделяем память под первую ячейку структуры
    p = p_begin;
    p->next = nullptr;

    char id[30]{};

    if (!readfile) {
        cout << "Ошибка открытия файла" << endl;
        return reinterpret_cast<Channels*>(-1);
    }
    else {
        while (true) {
            readfile.getline(id, 30); // Читаем файл построчно

            if (!readfile.eof()) {
                unsigned int limit1 = 0;
                unsigned int limit2 = 0;

                size_t i = 0;

                for (auto x : id) { // Ищем ограничители, с помощью которых разделены данные
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

                p->id = int(channel_id) - 48; // Конвертируем символы в числа

                for (int n = 0; n < 30; n++) {
                    p->name[n] = name[n];
                }

                p->next = new Channels(); // Выделяем память на следующий элемент
                p = p->next; // Переходим к следующему элементу

                p->next = nullptr;
            }
            else break;
        }

        readfile.close();
        return p_begin;
    }
}