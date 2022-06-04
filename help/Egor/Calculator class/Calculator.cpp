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
		double add(double a, double b) {
			return a + b;
		}

		double subtract(double a, double b) {
			if (b == 0) {
				throw "Division by zero exception";
			}
			return a - b;
		}

		double multiply(double a, double b) {
			return a * b;
		}

		double divide(double a, double b) {
			if (b == 0) throw "Division by zero exception";
			return a / b;
		}

	public:
		Result calculate(string expr) {
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
			return result;
		}
};


int main() {
	// input: <number> <'+', '-', '*' or '/'> <number>
	// to exit type "exit"
	Calc calc;

	string expr;
	getline(cin, expr);

	while (expr != "exit") {
		Result result = calc.calculate(expr);
		if (result.valid) {
			cout << result.value << endl;
		}
		getline(cin, expr);
	}

	return 0;
}

