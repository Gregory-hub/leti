#include "Channels_Str.h" // Структура для создания каналов
#include "Ad_Str.h" // Структура для рекламных роликов
#include "Broadcast_Str.h" // Структура для сетки вещания

#include "Create_channels.h" // Функция для создания списка каналов из файла
#include "Create_total_structure.h" // Функция для создания списка рекламных роликов в каналах
#include "Print_broadcast.h" // Функция для печати сетки вещания
#include "Create_broadcast_structure.h" // Функция для создания сетки вещания
#include "Write_broadcast.h"
#include "protocol.h"

using namespace std;

#define CHANNEL_LIST "channels.txt" // адрес файла с каналами
#define ADS_LIST "ads.txt" // адрес файла с рекламными роликами
#define TOTAL_LIST "broadcast.txt" // адрес итоговой сетки вещания


int main() {
    setlocale(LC_ALL, "Russian");
    cout << "--------------------------------" << endl;
    cout << "\nДемонстрация работы каналов:   \n" << endl;
    reset_protocol();

    // Создаем структуру с каналами
    Channels* channels_list = create_channels(CHANNEL_LIST);
    protocol("CHANNELS", channels_list);

    // Добавляем в структуру с каналами рекламные ролики
    create_total_structure(channels_list, ADS_LIST);
    protocol("CHANNELS WITH ADS", channels_list);

    // Создаем сетку вещания, используя структуру с каналами и рекламными роликами, а также используем структуру
    // со сгенирированными дополнительными параметрами.
    // Дополнительные параметры:
    // 1. Случайный канал, на котором произойдет прерывание вещания
    // 2. Случайная минута, в которую произойдет прерывание вещания
    // 3. Случайный час, в который мы демонстрируем вещание роликов
    Broadcast* broadcast_struct = create_broadcast_structure(channels_list);
    protocol("BROADCAST", broadcast_struct);

    // Распечатываем получившуюся сетку вещания
    print_broadcast(broadcast_struct);

    // Записываем получившуюся сетку вещания в файл
    write_broadcast(broadcast_struct, TOTAL_LIST);

    // Удаляем структуры
    delete channels_list;
    delete broadcast_struct;

    return 0;
}

