// Автор: Новиков Г.В.
// Группа: 1302
// Дата начала: 16.09.2021
// Дата окончания: 
// Версия: 1.3.01
// 58/27
// Формулировка: 
// 1. P(x) = 99,09 * x^8 + 9,099 * x^5 – 9,909 * x
// 2. P(x) = 24,35 * x^7 + 83,174 * x^5 – 24,26 * x^3

#include <iostream>
#include <stdio.h>

using namespace std;

int main() {

    printf("Автор: Новиков Г.В.\n"
	"Группа: 1302\n"
	"Дата начала: 16.09.2021 \n"
	"Дата окончания: 17.01.2021\n"
	"Версия: 1.3.01\n"
	"Введите x(кроме 0): ");
	double x;
	scanf("%lf", &x);
	printf("%.3lf\n", x);

	// 58
	printf("P(x) = 99,09 * x^8 + 9,099 * x^5 – 9,909 * x\n");				// x * (x**4 * (99.09 * x**3 + 9.099) - 9.909)
	double p11 = 99.09 * x * x * x + 9.099;
	printf("p1 = 99.09 * x^3 + 0.099 = %.3lf\n", p11);
	double p12 = x * x * x * x * p11 - 9.909;
	printf("p2 = x^4 * P1 - 9.909 = %.3lf\n", p12);
	double p13 = x * p12;
	printf("p3 = x * P2 = %.3lf\n", p13);
	double res58 = p13;
	printf("P58 = %.3lf\n\n", res58);

	// 27
	printf("P(x) = 24,35 * x^7 + 83,174 * x^5 – 24,26 * x^3\n");			// x**3 * (x**2 * (24.35 * x**2 + 83.174) - 24.26)
	double p21 = 24.35 * x * x + 83.174;
	printf("p1 = 24.35 * x^2 + 83.174 = %.3lf\n", p21);
	double p22 = p21 * x * x - 24.26;
	printf("p2 = x^2 * P1 - 24.26 = %.3lf\n", p22);
	double p23 = p22 * x * x * x;
	printf("p3 = x^3 * P2 = %.3lf\n", p23);
	double res27 = p23;
	printf("P27 = %.3lf\n\n", res27);

	printf("Result: P58/P27 = %.3lf\n", res58 / res27);

	return 0;
}
