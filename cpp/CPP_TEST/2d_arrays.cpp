#include <iostream>

using namespace std;


void printarr2d(int** arr, int m, int n) {
    for (int i = 0; i < m; i++) {
        for (int j = 0; j < n; j++) {
            cout << arr[i][j] << ' ';
        }
        cout << endl;
    }
}


int main(int argc, char const *argv[])
{
    int* arr[5];
    for (int i = 0; i < 5; i++) {
        int arr_mini[3] = {1, 2, 3};
        arr[i] = arr_mini;
    }
    arr[0][2] = 8;
    printarr2d(arr, 5, 3);
    return 0;
}
