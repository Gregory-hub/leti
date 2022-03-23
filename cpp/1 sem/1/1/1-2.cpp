// Автор: Новиков Г.В.
// Группа: 1302
// Дата начала: 16.09.2021
// Дата окончания: 
// Версия: 1.2.01
// 58/27
// Формулировка: 
// 1. P(x) = 99,09 * x^8 + 9,099 * x^5 – 9,909 * x
// 2. P(x) = 24,35 * x^7 + 83,174 * x^5 – 24,26 * x^3

#include <iostream>
#include <iomanip>

using namespace std;

int main() {

    cout << "Автор: Новиков Г.В.\n"
	"Группа: 1302\n"
	"Дата начала: 16.09.2021 \n"
	"Дата окончания: 17.01.2021\n"
	"Версия: 1.2.01\n"
	"Введите x(кроме 0): ";
	double x;
	cin >> x;

	cout << setprecision(10);
	// 58
	cout << endl << "P(x) = 99,09 * x^8 + 9,099 * x^5 – 9,909 * x" << endl;				// x * (x**4 * (99.09 * x**3 + 9.099) - 9.909)
	double p11 = 99.09 * x * x * x + 9.099;
	cout << "p1 = 99.09 * x^3 + 0.099 = " << p11 << endl;
	double p12 = x * x * x * x * p11 - 9.909;
	cout << "p2 = x^4 * P1 - 9.909 = " << p12 << endl;
	double p13 = x * p12;
	cout << "p3 = x * P2 = " << p13 << endl;
	double res58 = p13;
	cout << "P58 = " << res58 << endl << endl;

	// 27
	cout << "P(x) = 24,35 * x^7 + 83,174 * x^5 – 24,26 * x^3" << endl;			// x**3 * (x**2 * (24.35 * x**2 + 83.174) - 24.26)
	double p21 = 24.35 * x * x + 83.174;
	cout << "p1 = 24.35 * x^2 + 83.174 = " << p21 << endl;
	double p22 = p21 * x * x - 24.26;
	cout << "p2 = x^2 * P1 - 24.26 = " << p22 << endl;
	double p23 = p22 * x * x * x;
	cout << "p3 = x^3 * P2 = " << p23 << endl;
	double res27 = p23;
	cout << "P27 = " << res27 << endl << endl;

	cout << "Result: P58/P27 = " << res58 / res27 << endl;

	return 0;
}
