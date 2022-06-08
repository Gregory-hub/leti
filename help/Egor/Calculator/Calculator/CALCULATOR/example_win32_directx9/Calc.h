#include <iostream>
#include <string>
#include <sstream>


using namespace std;


struct Result {
	double value = NULL;
	bool valid = false;
};


class Calc {
private:
    double add(double a, double b);
    double subtract(double a, double b);
    double multiply(double a, double b);
    double divide(double a, double b);

public:
    Result calculate(string expr);
};

