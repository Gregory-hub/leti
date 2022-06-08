#include "Calc.h"


double Calc::add(double a, double b) {
    return a + b;
}

double Calc::subtract(double a, double b) {
    return a - b;
}

double Calc::multiply(double a, double b) {
    return a * b;
}

double Calc::divide(double a, double b) {
    if (b == 0) throw "Division by zero exception";
    return a / b;
}

Result Calc::calculate(string expr) {
	// input: <number> <'+', '-', '*' or '/'> <number>
	// to exit type "exit"

    Result result;
    stringstream ss(expr);
    string a_str;
    string b_str;
    string sign;

    getline(ss, a_str, ' ');
    getline(ss, sign, ' ');
    getline(ss, b_str, ' ');

    string cock;
    getline(ss, cock);
    if (cock != "") {
        cerr << "Invalid expression" << endl;
        result.valid = false;
        return result;
    }

    double a;
    double b;
    try {
        a = stod(a_str);
        b = stod(b_str);
    } 
    catch (invalid_argument) {
        cerr << "Invalid expression" << endl;
        result.valid = false;
        return result;
    }

    if (sign == "+") {
        result.value = add(a, b);
        result.valid = true;
    }
    else if (sign == "-") {
        result.value = subtract(a, b);
        result.valid = true;
    }
    else if (sign == "*") {
        result.value = multiply(a, b);
        result.valid = true;
    }
    else if (sign == "/") {
        try {
            result.value = divide(a, b);
            result.valid = true;
        }
        catch (const char* msg) {
            cerr << msg << endl;
        }
    }
    if (result.valid) cout << result.value << endl;
    return result;
}

