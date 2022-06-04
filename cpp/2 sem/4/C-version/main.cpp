#include"AllList.h"
int main() {
	setlocale(LC_ALL, "RUS");
	fstream f, f1, f2, f3;
	f.open("in.txt", ios::in);
	f1.open("in1.txt", ios::in);
	f2.open("in2.txt", ios::in);
	f3.open("out.txt", ios::out);
	AllList Ts;
	Input(f,f1, f2, &Ts);
	f3 << "введённый текст:" << '\n';
	Output(f3, Ts);
	f3 << "результат:" << '\n';
	int ch;
	int ch1, fst, lst;
	ch1 = 0;
	fst = 0;
	lst = 0;
	int l = maxlengthStr(Ts.T2);
   	ch = obrabotka(Ts);
	if (ch < 0)
		f3 << "вхождения списка L1 в список L  не найдены или их невозможно заменить на спискок L2" << '\n';
	else
		OutputB(f3, Ts.T1);
	Delete(&Ts);
	f3 << "после удаления:" << '\n';
	Output(f3, Ts);
	return 0;
}