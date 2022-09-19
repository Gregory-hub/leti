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
        writefile << "Название канала: " << p->name << endl;
        writefile << "--------------------------------" << endl;
        int minute_count = 0; // Счетчик минут
        while (p->broadcast_ads) {
            // Записываем параметры роликов в файл
            writefile << "Время начала ролика: " << setfill('0') << setw(2) << p->broadcast_ads->hour << ":"
                << setfill('0')
                << setw(2) << p->broadcast_ads->minute << " Название ролика: " << p->broadcast_ads->name <<
                " Длительность: " << p->broadcast_ads->duration << " ceк."
                << endl;
            minute_count++;
            p->broadcast_ads = p->broadcast_ads->next;
        }
        // Если вещание длиться меньше 59 минут, то значит трансляция была прервана
        if (minute_count < 59) {
            writefile << "Трансляция была прервана..." << endl;
        }
        p = p->next;
    }

    writefile.close();
}