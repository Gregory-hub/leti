#include"Elem.h"
int main() {
	fstream f, f1;
	f.open("inp.txt", ios::in);
	Elem* head;
	head = new Elem;
	inp(head, f);
	f1.open("out.txt", ios::out);
	f1 << "введённый список:" << '\n';
	out(head, f1);
	head = obrabotka(head);
	f1 << "результат:" << '\n';
	out(head, f1);
	head = deleteList(head);
	f1 << "после удаления:" << '\n';
	out(head, f1);
}