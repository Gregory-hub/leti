#include <iostream>

using namespace std;


void printarr(int* array, int n) {
    // O(n)
    for (int i = 0; i < n; i++) {
        cout << array[i] << ' ';
    }
    cout << endl;
}


int binsearch(const int* array, int n, int value) {
    // O(log(n))
    int left = 0;
    int right = n - 1;
    int mid = n / 2;
    while (array[mid] != value && left <= right) {
        if (array[mid] < value) {
            left = mid + 1;
        } else {
            right = mid - 1;
        }
        mid = (right + left) / 2;
    }

    if (left <= right) {
        return mid;
    } else {
        return -1;
    }
}


int* selection_sort(int* array, int n) {
    // O(n^2)
    for (int i = 0; i < n; i++) {
        int min_i = i;
        for (int j = i; j < n - i; j++) {
            if (array[j] < array[min_i]) min_i = j;
        }
        swap(array[i], array[min_i]);

        int max_i = n - 1 - i;
        for (int j = i + 1; j < n - i; j++) {
            if (array[j] > array[max_i]) max_i = j;
        }
        swap(array[n - i - 1], array[max_i]);
    }
    return array;
}


int* bubble_sort(int* array, int n) {
    // O(n^2)
    for (int i = 0; i < n - 1; i++) {
        bool swapped = false;
        for (int j = 0; j < n - 1; j++) {
            if (array[j] > array[j + 1]) {
                swap(array[j], array[j + 1]);
                swapped = true;
            }
        }
        if (!swapped) break;
    }
    return array;
}


int* direction_change_b_sort(int* array, int n) {
    // O(n^2)
    for (int i = 0; i < n / 2; i++) {
        bool swapped = false;
        for (int j = i; j < n - 1 - i; j++) {
            if (array[j] > array[j + 1]) {
                swap(array[j], array[j + 1]);
                swapped = true;
            }
        }
        for (int j = n - 1 - i; j > i; j--) {
            if (array[j] < array[j - 1]) {
                swap(array[j], array[j - 1]);
                swapped = true;
            }
        }
        if (!swapped) break;
    }
    return array;
}


int* insertion_sort(int* array, int n) {
    // O(n^2)
    if (n <= 1) return array;
    for (int i = 1; i < n; i++) {
        int temp = array[i];
        for (int j = i - 1; j >= 0; j--) {
            if (array[j] > temp) {
                array[j + 1] = array[j];
                if (j == 0) array[j] = temp;
            } else {
                array[j + 1] = temp;
                break;
            }
        }
    }
    return array;
}


int* insertion_bin_sort(int* array, int n) {
    // O(n^2)
    if (n <= 1) return array;
    for (int i = 1; i < n; i++) {
        int temp = array[i];
        int left = 0;
        int right = i - 1; 
        int mid = right / 2;
        while (left <= right) {
            if (array[mid] < temp) {
                left = mid + 1;
            } else {
                right = mid - 1;
            }
            mid = (left + right) / 2;
        }

        if (temp > array[mid]) mid += 1;
        for (int j = i - 1; j >= mid; j--) {
            array[j + 1] = array[j];
        }
        array[mid] = temp;
    }
    return array;
}


int main(int argc, char const *argv[]) {
    const int n = 6;
    int array[n] = {11, 11, 201, 3, 23, 14};
    // cout << selection_sort(array, n) << endl;
    printarr(insertion_bin_sort(array, n), n);
    return 0;
}
