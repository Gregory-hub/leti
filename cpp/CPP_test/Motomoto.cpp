#include <iostream>
#include <chrono>
#include <thread>
#include <stdio.h>
#include <cstdlib>

using namespace std;


class Female {
    private:
        double weigth;
    public:
        string name;
        double width;
        double heigth;
        double length;
        Female(string n, double wei, double wid, double hei, double len) {
            name = n;
            weigth = wei;
            width = wid;
            heigth = hei;
            length = len;
        };
};


class MotoMoto {
    public:
        double width;
        double heigth;
        double length;
        double weigth;
        MotoMoto(double wei, double wid, double hei, double len) {
            weigth = wei;
            width = wid;
            heigth = hei;
            length = len;
        };

        void breed(Female female) {
            cout << "Moto Moto is breeding with " << female.name << "..." << endl;
            srand(2000);
            int random_time = rand() % 2000;
            for (int i = 0; i < 3; i++) {
                this_thread::sleep_for(chrono::milliseconds(random_time));
                cout << "..." << endl;
            }
            this_thread::sleep_for(chrono::milliseconds(random_time));
            cout << female.name << " Finished" << endl;
        }
};


int main(int argc, char const *argv[])
{
    Female angela = Female("Angela", 834.9, 2.3, 1.5, 1.9);
    Female lizzy = Female("Lizzy", 1040, 3.4, 1.1, 2.1);
    Female bob = Female("Bob", 590, 2.1, 1.8, 1.6);
    MotoMoto moto_moto = MotoMoto(2312, 4.32, 2.7, 7.9);

    moto_moto.breed(angela);
    moto_moto.breed(lizzy);
    moto_moto.breed(bob);
    return 0;
}
