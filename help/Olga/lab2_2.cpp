/*ФИО: Романова Ольга Валерьевна
*Группа: 1302
*Дата начала:30.09.21
*Дата окончания:21.10.21
*Версия: 2.1.01
*/
#include <iostream>
#include <math.h>

using namespace std;


int main()
{
	setlocale(LC_ALL, "ru");

	cout << "Romanova Olga Valerievna \nGroup: 1302\nStart date:30.09.21\nEnd date: 21.10.21 \nVersion: 2.1.01 \nFormulation: \n";
	cout << " __________________" << endl;
	cout << "/                 /" << endl;
	cout << " |              |" << endl;
	cout << "  |              |" << endl;
	cout << "/                 / " << endl;
	cout << "-------------------" << endl;

	cout << "Введите x1 для точек a, b и x2 для точек c, d:" << endl;
	double x_ab, x_cd;
	cin >> x_ab >> x_cd;

	cout << "Введите высоту фигуры:" << endl;
	double y_ac;
	cin >> y_ac;

	double r = abs(y_ac / 4);

	cout << "Введите x и y точки:" << endl;
	double x, y;
	cin >> x >> y;

	double t = 2 * r + abs(x_ab - x_cd);

	if (y_ac < 0) {
		y = -y;
		y_ac = -y_ac;
	}

	if (x_ab > x_cd) {
		x = -x;
		x_ab = -x_ab;
		x_cd = -x_cd;
	}
	x = x - x_ab + r;
	x = fmod(x, t);
	if (x < 0) {
		x += t;
	} else if (x == 0 && y < y_ac / 2) {
		x += t;
	}

	x_ab = r;
	x_cd = t - r;

	cout << "x_ab: " << x_ab << endl;
	cout << "x_cd: " << x_cd << endl;
	cout << "y_ac: " << y_ac << endl;
	cout << "t: " << t << endl;
	cout << "x: " << x << endl;
	cout << "y: " << y << endl;

	bool success = false;
	if ((pow(x - r, 2) + pow(y - (y_ac - y_ac / 4), 2)) <= pow(r, 2)) {
		cout << "1 cond" << endl;
		success = true;
	} else if ((pow(x - t + r, 2) + pow(y - y_ac / 4, 2)) <= pow(r, 2)) {
		cout << "2 cond" << endl;
		success = true;
	} else if ((x >= x_ab && x <= x_cd && y <= y_ac && y >= 0) && !((pow(x - r, 2) + pow(y - y_ac / 4, 2)) < pow(r, 2)) && !((pow(x - t + r, 2) + pow(y - (y_ac - y_ac / 4), 2)) < pow(r, 2))) {
		cout << "3 cond" << endl;
		success = true;
	}
	
	if (success) {
		cout << "Принадлежит" << endl;
	} else {
		cout << "Не принадлежит" << endl;
	}
}
