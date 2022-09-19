#include <iostream>
#include <fstream>

#include "Channels_Str.h"
#include "Ad_Str.h"

using namespace std;

void* create_total_structure(Channels* p_begin, const char* filename) {
    setlocale(LC_ALL, "Russian");
    // Открываем файл
    ifstream readfile(filename, ios::in);
    // Читаем все символы, включая пробелы
    readfile >> std::noskipws;

    Channels* p = p_begin;

    char id[30]{};

    if (!readfile) {
        cout << "Ошибка открытия файла" << endl;
        return reinterpret_cast<Channels*>(-1);
    }
    else {
        while (true) {
            readfile.getline(id, 30);

            if (!readfile.eof()) {
                unsigned int limit1[5]{};
                unsigned int limit2 = 0;

                size_t i = 0;

                int two_point_count = 0;

                // Ищем порядковые номера ограничителей
                for (auto x : id) {

                    if (id[i] == ':') {
                        limit1[two_point_count] = i;
                        two_point_count++;
                    }
                    if (id[i] == ';') {
                        limit2 = i;
                        break;
                    }
                    i++;
                }

                char ads_id[2];

                char name[30]{};

                char duration[2]{};

                char sequence[2]{};

                char channels_id_arr[4]{};

                int sequence_count = 0;

                int channels_id_count = 0;

                int name_count = 0;

                int duration_count = 0;

                int ads_id_count = 0;

                // Ищем подстроки и записываем их в массив
                for (int k = 0; k < limit2; k++) {
                    if (k < limit1[0]) {
                        ads_id[ads_id_count] = id[k];
                        ads_id_count++;
                    }
                    if (k > limit1[0] && k < limit1[1]) {
                        name[name_count] = id[k];
                        name_count++;
                    }
                    if (k > limit1[1] && k < limit1[2]) {
                        duration[duration_count] = id[k];
                        duration_count++;
                    }
                    if (k > limit1[2] && k < limit1[3]) {
                        if (id[k] != ',') {
                            channels_id_arr[channels_id_count] = id[k];
                            channels_id_count++;
                        }
                    }
                    if (k > limit1[3] && k < limit2) {
                        sequence[sequence_count] = id[k];
                        sequence_count++;
                    }
                }

                int convert_id;

                // Конвертируем символы в числа
                if (ads_id[1]) {
                    convert_id = (int(ads_id[0]) - 48) * 10 + int(ads_id[1]) - 48;
                }
                else {
                    convert_id = int(ads_id[0]) - 48;
                }

                int convert_duration = (int(duration[0]) - 48) * 10 + int(duration[1] - 48);

                int convert_sequence = (int(sequence[0]) - 48) * 10 + int(sequence[1] - 48);

                for (char t : channels_id_arr) {
                    while (p != nullptr) {
                        if (int(t) - 48 == p->id) {
                            struct Ad* temp, * a;
                            temp = new Ad(); // Выделяем память для вложенной структуры
                            a = p->ads; // Сохранение указателя на следующий узел
                            p->ads = temp; // Предыдущий узел указывает на создаваемый
                            temp->id = convert_id; // Сохранение поля данных добавляемого узла
                            temp->duration = convert_duration;

                            int word_count = 0;

                            for (char s : name) {
                                temp->name[word_count] = s;
                                word_count++;
                            }

                            temp->sequence = convert_sequence;
                            temp->next = a;  // Созданный узел указывает на следующий элемент
                        }
                        p = p->next;
                    }
                    p = p_begin;
                }
            }
            else break;
        }
        readfile.close();
    }
    return nullptr;
}