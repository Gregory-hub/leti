#include <iostream>
#include <fstream>
#include <iomanip>

using namespace std;


struct ArrSize {
    int n;
    int m;
};


struct Index_2d {
    int i;
    int j;
};


struct Rect {
    Index_2d lt;
    Index_2d rt;
    Index_2d lb;
    Index_2d rb;
};


ArrSize get_arr_size(string filename) {
    fstream file;
    file.open(filename, ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
    }

    ArrSize size;
    size.n = 0;
    size.m = 0;

    string line;
    while (getline(file, line)) {
        size.m++;
    }

    file.clear();
    file.seekg(0);

    getline(file, line);
    for (char x: line) size.n++;

    file.close();

    return size;
}


int** read_array(string filename, int n, int m) {
    fstream file;
    file.open("in1.txt", ios::in);
    if (!file.is_open()) {
        perror("Error opening file");
    }

    int** arr = new int*[m];
    int i = 0;
    string line;
    while (getline(file, line)) {
        int* row = new int[n];
        int j = 0;
        for(char x: line) {
            row[j] = (int)(x - '0');
            j++;
        }
        arr[i] = row;
        i++;
    }
    return arr;
}


Index_2d find_rect(int** rect_map, int n, int m) {
    Index_2d index;
    index.i = -1;
    index.j = -1;

    for (int i = 0; i < m; i++) {
        for (int j = 0; j < n; j++) {
            if (rect_map[i][j] == 1) {
                index.i = i;
                index.j = j;
                return index;
            }
        }
    }

    return index;
}


Rect get_rect(int** rect_map, int n, int m, Index_2d index) {
    Rect rect;
    rect.lt = index;

    int min_h = n + 1;
    int i = index.i;
    int j = index.j;
    while (j < n && rect_map[i][j] == 1) {
        int i = index.i;
        while (i < m && rect_map[i][j] == 1) {
            i++;
        }
        if (i - index.i < min_h) min_h = i - index.i;
        j++;
    }

    rect.lb.i = index.i + min_h - 1;
    rect.lb.j = index.j;
    rect.rt.i = index.i;
    rect.rt.j = j - 1;
    rect.rb.i = index.i + min_h - 1;
    rect.rb.j = j - 1;

    return rect;
}


void delete_rect(int** rect_map, Rect rect) {
    for (int i = rect.lt.i; i < rect.lb.i + 1; i++) {
        for (int j = rect.lt.j; j < rect.rt.j + 1; j++) {
            rect_map[i][j] = 0;
        }
    }
}


void print_rect(Rect rect, int rect_number, bool erase = false) {
    fstream file;
    if (erase) {
        file.open("out.txt", ios::out);
    } else {
        file.open("out.txt", ios::app);
    }
    if (!file.is_open()) {
        perror("Cannot open file 'out.txt'");
    }

    cout << "Rect " << rect_number << ": (" << rect.lt.i << ' ' << rect.lt.j << ") (" << rect.rt.i << ' ' << rect.rt.j << ')' << endl;
    cout << setw(9) << "(" << rect.lb.i << ' ' << rect.lb.j << ") (" << rect.rb.i << ' ' << rect.rb.j << ')' << endl;

    file << "Rect " << rect_number << ": (" << rect.lt.i << ' ' << rect.lt.j << ") (" << rect.rt.i << ' ' << rect.rt.j << ')' << endl;
    file << setw(9) << "(" << rect.lb.i << ' ' << rect.lb.j << ") (" << rect.rb.i << ' ' << rect.rb.j << ')' << endl;

    file.close();
}


void print_rect_number(int rect_number, bool erase = false) {
    fstream file;
    if (erase) {
        file.open("out.txt", ios::out);
    } else {
        file.open("out.txt", ios::app);
    }
    if (!file.is_open()) {
        perror("Cannot open file 'out.txt'");
    }

    cout << "Number of rectangles: " << rect_number << endl;
    file << "Number of rectangles: " << rect_number << endl;

    file.close();
}


void delete_matrix(int** arr, int size) {
    for (int i = 0; i < size; i++) {
        delete[] arr[i];
    }
}


// Rectangles can touch each other 
// Part of rectangle cannot be part of another rectangle
// Not rectangles:
// '+'-forms(3 rects)
// 'T'-forms(2 rects)
int main(int argc, char const *argv[]) {
    cout << "Author: Novikov G. \n"
    "Group: 1302 \n"
    "Start date: 21.11.2021 \n"
    "End date: 22.11.2021 \n"
    "Version: 5.1.01 \n" << endl;

    string filename = "in1.txt";
    ArrSize size = get_arr_size(filename);
    int n = size.n;
    int m = size.m;
    int** rect_map = read_array("in1.txt", n, m);

    int rect_count = 0;

    while (find_rect(rect_map, n, m).i >= 0) {
        Index_2d rect_index = find_rect(rect_map, n, m);
        Rect rect = get_rect(rect_map, n, m, rect_index);
        delete_rect(rect_map, rect);
        if (rect_count == 0) {
            print_rect(rect, rect_count + 1, true);
        } else {
            print_rect(rect, rect_count + 1);
        }
        rect_count++;
    }

    print_rect_number(rect_count);

    delete_matrix(rect_map, m);
    return 0;
}
