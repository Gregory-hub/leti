#include <iostream>
#include <fstream>
#include <sstream>
#include <iomanip>
#include <math.h>


using namespace std;


struct ArrSize {
    int n = 0;
    const int m = 2;
};


struct Point {
    double x;
    double y;
};


struct Intersection : Point {
    bool exists = true;
    bool whole_x = false;
    bool whole_y = false;
    bool whole_line = false;
};


struct Line {
    // normal: y = ax + b
    // vertical: x = b
    double a;
    double b;
    bool is_vertical = false;
    bool exists = true;
};


ArrSize read_size(string filename) {
    ifstream file;
    file.open(filename + ".txt");
    if (!file.is_open()) {
        perror("Cannot open file");
    }

    ArrSize size;

    string line;
    while (getline(file, line) && !line.empty()) {
        size.n++;

        stringstream ss(line);
        string x;
        int line_els_count = 0;
        while (getline(ss, x, ' ')) {
            line_els_count++;
        }

        if (line_els_count != 2) {
            cerr << "A point must have 2 coords" << endl;
            file.close();
            exit(1);
        }
    }

    file.close();
    return size;
}


double read_radius(string filename) {
    ifstream file;
    file.open(filename + ".txt");
    if (!file.is_open()) {
        perror("Cannot open file");
    }

    double radius;
    string line;
    while (getline(file, line) && !line.empty()) {}

    getline(file, line);
    stringstream ss(line);
    string x;
    int line_els_count = 0;
    while (getline(ss, x, ' ')) {
        line_els_count++;
    }

    if (line_els_count != 1) {
        cerr << "Radius line must contain only one number - radius" << endl;
        file.close();
        exit(1);
    }

    radius = stod(line);

    file.close();
    return radius;
}


Point* get_points(string filename, Point* arr, ArrSize size) {
    ifstream file;
    file.open(filename + ".txt");
    if (!file.is_open()) {
        perror("Cannot open file");
    }

    string line;
    string num;
    for (int i = 0; i < size.n; i++) {
        getline(file, line);
        stringstream ss(line);
        Point point;

        getline(ss, num, ' ');
        point.x = stod(num);
        getline(ss, num, ' ');
        point.y = stod(num);

        arr[i] = point;
    }

    file.close();
    return arr;
}


Line* get_lines(Point* points, Line* lines, int n) {
    int lines_i = 0;
    for (int i = 0; i < n; i++) {
        for (int j = i + 1; j < n; j++) {
            Line line;

            if (points[i].x == points[j].x && points[i].y == points[j].y) {
                line.exists = false;
                line.a = points[i].x;
                line.b = points[i].y;
            } else if (points[i].x == points[j].x) {
                line.is_vertical = true;
                line.b = points[i].x;
            } else {
                line.a = (points[i].y - points[j].y) / (points[i].x - points[j].x);
                line.b = points[i].y - line.a * points[i].x;
            }

            lines[lines_i] = line;
            lines_i++;
        }
    }
    return lines;
}


Intersection* get_intersections(Line* lines, Intersection* intersections, int n) {
    int intersections_i = 0;
    for (int i = 0; i < n; i++) {
        for (int j = i + 1; j < n; j++) {
            Intersection point;
            if (!lines[i].exists || !lines[j].exists) {
                point.exists = false;

            } else if(lines[i].is_vertical && lines[j].is_vertical) {       // both lines are x = b 
                if (lines[i].b == lines[j].b) {                             // same vertical line
                    point.x = lines[i].b;
                    point.whole_y = true;
                    point.whole_line = true;
                } else {                                                    // parallel lines
                    point.exists = false;
                }

            } else if (lines[i].is_vertical) {                              // one vertical line
                point.x = lines[i].b;
                point.y = lines[j].a * lines[i].b + lines[j].b;

            } else if(lines[j].is_vertical) {                               // one vertical line
                point.x = lines[j].b;
                point.y = lines[i].a * lines[j].b + lines[i].b;

            } else if (lines[i].a == lines[j].a) {
                if (lines[i].b == lines[j].b) {
                    point.whole_line = true;                                // same line
                } else {
                    point.exists = false;                                   // parallel line
                }

            } else if (lines[i].a == 0 && lines[j].a == 0) {                // y = b
                if (lines[i].b == lines[j].b) {                             // same horizonral line
                    point.y = lines[i].b;
                    point.whole_x = true;
                    point.whole_line = true;
                } else {                                                    // parallel lines
                    point.exists = false;
                }

            } else {
                point.x = (lines[j].b - lines[i].b) / (lines[i].a - lines[j].a);
                point.y = lines[i].a * point.x + lines[i].b;
            }
            point.x = roundf(point.x * 1000000) / 1000000;
            point.y = roundf(point.y * 1000000) / 1000000;

            intersections[intersections_i] = point;
            intersections_i++;
        }
    }
    return intersections;
}


void printarr(Point* arr, ArrSize size) {
    for (int i = 0; i < size.n; i++) {
        cout << arr[i].x << ' ' << arr[i].y << endl;
    }
}


void printarr(Line* arr, int n) {
    for (int i = 0; i < n; i++) {
        if (!arr[i].exists) {
            cout << "Line does not exist" << endl;
        } else if (arr[i].is_vertical) {
            cout << setprecision(4) << "x = " << arr[i].b << endl;
        } else {
            cout << setprecision(4) << "y = " << arr[i].a << "x + " << arr[i].b << endl;
        }
    }
}


void printarr(Intersection* arr, int n) {
    for (int i = 0; i < n; i++) {
        if (!arr[i].exists) {
            cout << "Point does not exist" << endl;
        } else if (arr[i].whole_line) {
            if (arr[i].whole_x) {
                cout << "y = " << arr[i].y << " x = (-inf, +inf)" << endl;
            } else if (arr[i].whole_y) {
                cout << "y = (-inf, +inf) x = " << arr[i].x << endl;
            } else {
                cout << "y = (-inf, +inf) x = (-inf, +inf)" << endl;
            }

        } else {
            cout << setprecision(4) << arr[i].x << ' ' << arr[i].y << endl;
        }
    }
}


int main(int argc, char const *argv[]) {
    cout << "Author: Novikov G. \n"
            "Group: 1302 \n"
            "Start date: 3.12.2021 \n"
            "End date: .12.2021 \n"
            "Cursed work \n" << endl;

    string filename = "in";
    ArrSize size = read_size(filename);
    double radius = read_radius(filename);

    Point init_points[size.n];
    get_points(filename, init_points, size);

    int number_of_lines = size.n * (size.n - 1) / 2;
    Line lines[number_of_lines];
    get_lines(init_points, lines, size.n);
    printarr(lines, number_of_lines);

    int number_of_intersections = number_of_lines * (number_of_lines - 1) / 2;
    Intersection intersectons[number_of_intersections];
    get_intersections(lines, intersectons, number_of_lines);
    printarr(intersectons, number_of_intersections);

    cout << endl << radius << endl;

    return 0;
}
