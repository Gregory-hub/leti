#include <iostream>

using namespace std;

int main(int argc, char const *argv[])
{
    enum Value {
        A = 'A', B, C
    };
    
    Value val = C;

    int success = (val == A || val == B) ? 41 : 78;
    cout << success << endl;

    return 0;
}

