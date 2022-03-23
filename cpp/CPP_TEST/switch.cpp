#include <iostream>

using namespace std;

int main(int argc, char const *argv[])
{
    int a = 3;
    switch(a) {
        case 1: cout << "one" << endl; 
                break;
        case 2: cout << "two" << endl; 
                break;
        case 3: cout << "three" << endl; 
                break;
        default: cout << "I dont know that number" << endl;
    }
    return 0;
}
