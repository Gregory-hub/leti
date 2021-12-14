#include <iostream>
#include <fstream>
#include <sstream>
#include <iomanip>
#include <math.h>


using namespace std;


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


struct Circle {
    Point center;
    double radius;


    bool belongs(Point point) {
        return (pow(point.x - center.x, 2) + pow(point.y - center.y, 2) <= pow(radius, 2));
    }


    int count_points_inside(Intersection* points, int n) {
        int count = 0;

        for (int i = 0; i < n; i++) {
            Point point = points[i];
            if (belongs(point)) count++;
        }

        return count;
    }
};


struct Line {
    // normal: y = ax + b
    // vertical: x = b
    double a;
    double b;
    bool is_vertical = false;
    bool exists = true;
};


int read_size(string filename) {
    ifstream file;
    file.open(filename + ".txt");
    if (!file.is_open()) {
        perror("Cannot open file");
        exit(1);
    }

    int n = 0;

    string line;
    while (getline(file, line) && !line.empty()) {

        stringstream ss(line);
        string x;
        int line_els_count = 0;
        while (getline(ss, x, ' ')) {
            line_els_count++;
        }

        if (line_els_count == 2) {
            n++;
        }
    }

    file.close();
    return n;
}


double read_radius(string filename) {
    ifstream file;
    file.open(filename + ".txt");
    if (!file.is_open()) {
        perror("Cannot open file");
        exit(1);
    }

    double radius;
    string line;
    while (getline(file, line) && !line.empty()) {}
    while (getline(file, line) && line.empty()) {}

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


Point* get_points(string filename, Point* arr, int n) {
    ifstream file;
    file.open(filename + ".txt");
    if (!file.is_open()) {
        perror("Cannot open file");
        exit(1);
    }

    int i = 0;
    string line;
    string num;
    while (getline(file, line) && !line.empty()) {

        Point point;
        stringstream ss(line);
        string num;
        int line_els_count = 0;
        while (getline(ss, num, ' ')) {
            line_els_count++;
        }

        if (line_els_count != 2) {
            cerr << "get_points: Point line must contain 2 numbers" << endl;
            file.close();
            exit(1);

        }

        ss.str(line);
        ss.clear();
        getline(ss, num, ' ');
        point.x = stoi(num);
        getline(ss, num, ' ');
        point.y = stoi(num);

        arr[i] = point;
        i++;
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


void get_circles(Point* init_points, int n, double radius, Circle* circles) {
    Circle circle;
    circle.radius = radius;
    for (int i = 0; i < n; i++) {
        circle.center = init_points[i];
        circles[i] = circle;
    }
}


int get_max_point_count(Circle* circles, int n, Intersection* intersections, int intersection_n) {
    int max_point_count = 0;
    for (int i = 0; i < n; i++) {
        int point_count = circles[i].count_points_inside(intersections, intersection_n);
        if (point_count > max_point_count) {
            max_point_count = point_count;
        }
    }
    return max_point_count;
}


int get_number_of_max_circles(Circle* circles, int n, Intersection* intersections, int intersection_n) {
    int max_point_count = get_max_point_count(circles, n, intersections, intersection_n);
    int number_of_circles = 0;

    for (int i = 0; i < n; i++) {
        int point_count = circles[i].count_points_inside(intersections, intersection_n);
        if (point_count == max_point_count) {
            number_of_circles++;
        }
    }

    return number_of_circles;
}


Circle* find_max_points_circles(Circle* circles, int n, Intersection* intersections, int intersection_n, int max_point_count) {
    int j = 0;
    Circle* circles_max = new Circle[n];
    int number_of_max_circles = get_number_of_max_circles(circles, n, intersections, intersection_n);
    for (int i = 0; i < n; i++) {
        if (circles[i].count_points_inside(intersections, intersection_n) == max_point_count) {
            circles_max[j] = circles[i];
            j++;
        }
    }

    return circles_max;
}


void printarr(Point* arr, int n) {
    for (int i = 0; i < n; i++) {
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


void printarr(Circle* arr, int n) {
    for (int i = 0; i < n; i++) {
        cout << "Circle(" << arr[i].center.x << "; " << arr[i].center.x << "), radius = " << arr[i].radius << endl;
    }
}


int main(int argc, char const *argv[]) {
    cout << "Author: Novikov G. \n"
            "Group: 1302 \n"
            "Start date: 3.12.2021 \n"
            "End date: 8.12.2021 \n"
            "Cursed work \n" << endl;

    const string filename = "in";
    int n = read_size(filename);
    double radius = read_radius(filename);

    Point* init_points = new Point[n];
    get_points(filename, init_points, n);
    printarr(init_points, n);

    int number_of_lines = n * (n - 1) / 2;
    Line* lines = new Line[number_of_lines];
    get_lines(init_points, lines, n);
    printarr(lines, number_of_lines);

    int number_of_intersections = number_of_lines * (number_of_lines - 1) / 2;
    Intersection* intersections = new Intersection[number_of_intersections];
    get_intersections(lines, intersections, number_of_lines);
    printarr(intersections, number_of_intersections);

    Circle* circles = new Circle[n];
    get_circles(init_points, n, radius, circles);
    printarr(circles, n);

    cout << endl;
    cout << "Points: " << n << endl;
    cout << "Lines: " << number_of_lines << endl;
    cout << "Intersections: " << number_of_intersections << endl;
    cout << endl << "Radius: " << radius << endl;

    int max_point_count = get_max_point_count(circles, n, intersections, number_of_intersections);
    Circle* circles_max = find_max_points_circles(circles, n, intersections, number_of_intersections, max_point_count);
    printarr(circles_max, get_number_of_max_circles(circles, n, intersections, number_of_intersections));

    delete[] init_points;
    delete[] lines;
    delete[] intersections;
    delete[] circles;

    return 0;
}
