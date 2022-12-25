#include <iostream>
#include <stack>
#include <iomanip>
#include <string>

using namespace std;

class TypeOf {
public:
	char type;
	double value;

	TypeOf() {};
	~TypeOf() {};
};

bool isOperator(char x) 
{
	switch (x) 
	{
	case '+':
	case '-':
	case '/':
	case '*':
	case '^':
		return true;
	}
	return false;
}

bool isOperand(char x)
{
	return (x >= '0' && x <= '10');
}

string preToInf(string pre_exp) 
{
	stack<string> s;

	int length = pre_exp.size();

	for (int i = length - 1; i >= 0; i--) {

		if (pre_exp[i] == ' ') { continue; }


		if (isOperator(pre_exp[i])) {


			string op1 = s.top();   s.pop();
			string op2 = s.top();   s.pop();


			string temp = "(" + op1 + pre_exp[i] + op2 + ")";


			s.push(temp);
		}

		else {
			int j = i;
			string temp;
			while (1) 
			{
				if (pre_exp[j] == '-') { continue; }
				if (pre_exp[j] == '+') { continue; }
				if (pre_exp[j] == '*') { continue; }
				if (pre_exp[j] == '/') { continue; }
				if (pre_exp[j] == ' ') { break; }
				temp += pre_exp[j];
				j--;
			}
			reverse(temp.begin(), temp.end());
			s.push(temp);


			if (i - j > 1) { i = j + 1; }
		}
	}
	return s.top();
}


string postToInf(string exp)
{
	stack<string> s;

	for (int i = 0; exp[i] != '\0'; i++)
	{
		if (exp[i] == ' ') { continue; }

		if (isOperand(exp[i]))
		{
			int j = i;
			string temp;
			while (1) 
			{
				if (exp[j] == '-') { continue; }
				if (exp[j] == '+') { continue; }
				if (exp[j] == '*') { continue; }
				if (exp[j] == '/') { continue; }
				if (exp[j] == ' ') { break; }
				temp += exp[j];
				j++;
			}

			if (j - i > 1) { i = j - 1; }
			s.push(temp);
		}

		else
		{
			string op1 = s.top();
			s.pop();
			string op2 = s.top();
			s.pop();
			s.push("(" + op2 + exp[i] +
				op1 + ")");
		}
	}


	return s.top();
}


int Priority(char Oper) 
{

	if (Oper == '^') { return 3; }

	if (Oper == '+' or Oper == '-') {
		return 1;
	}

	if (Oper == '*' or Oper == '/') {
		return 2;
	}
	return 0;
}


bool Calc(stack <TypeOf>& Numbers, stack <TypeOf>& Operations, TypeOf& huy) {
	double A, B;
	double result;
	A = Numbers.top().value;
	Numbers.pop();
	switch (Operations.top().type)
	{
	case '^':

		B = Numbers.top().value;
		Numbers.pop();
		result = pow(B, A);
		huy.type = '0';
		huy.value = result;
		Numbers.push(huy);
		Operations.pop();
		break;

	case '+':

		B = Numbers.top().value;

		Numbers.pop();
		result = A + B;
		huy.type = '0';
		huy.value = result;
		Numbers.push(huy);
		Operations.pop();
		break;

	case '-':

		B = Numbers.top().value;
		Numbers.pop();
		result = B - A;
		huy.type = '0';
		huy.value = result;
		Numbers.push(huy);
		Operations.pop();
		break;
	case '*':

		B = Numbers.top().value;
		Numbers.pop();
		result = A * B;
		huy.type = '0';
		huy.value = result;
		Numbers.push(huy);
		Operations.pop();
		break;

	case '/':

		B = Numbers.top().value;
		if (A == 0) { cerr << "ошибка"; return false; }
		Numbers.pop();
		result = B / A;
		huy.type = '0';
		huy.value = result;
		Numbers.push(huy);
		Operations.pop();
		break;

	default:
		return false;
		break;
	}
	return true;
}

void greetings() {
	cout << setw(60) << "Калькулятор " << endl;
	cout << setw(62) << "Гордиенко Михаил" << endl;
}

int main() 
{
	setlocale(LC_ALL, "Russian");
	stack <TypeOf> numbers;
	stack <TypeOf> operations;
	bool sign = 1;
	char ch;
	double value;
	string chh = "";
	greetings();
	cout << endl << "Ввод: "; getline(cin, chh, '\n'); cout << endl;

	if (chh[0] == '+' or chh[0] == '-' or chh[0] == '*' or chh[0] == '/' or chh[0] == '^')
	{
		chh = preToInf(chh);
	}
	if (chh[chh.size() - 1] == '+' or chh[chh.size() - 1] == '-' or chh[chh.size() - 1] == '*' or chh[chh.size() - 1] == '/')
	{
		chh = postToInf(chh);
	}

	TypeOf symb;


	for (int i = 0; i < chh.size(); i++) {
		ch = chh[i];


		if (ch == '\n') { break; }

		if (ch == ' ') { continue; }

		if (ch >= '0' and ch <= '9' or (ch == '-' and sign == 1)) {
			int j = i;
			string temp;
			while (j != chh.size()) {
				if (chh[j] == '\n') { break; }
				if (chh[j] == '+') { break; }
				if (chh[j] == '-') { break; }
				if (chh[j] == ')') { break; }
				if (chh[j] == '(') { break; }
				if (chh[j] == '^') { break; }
				if (chh[j] == '*') { break; }
				if (chh[j] == '/') { break; }

				temp += chh[j];
				j++;
			}
			if (j - i > 1) { i = j - 1; }
			value = strtod(temp.c_str(), NULL);

			symb.type = '0';
			symb.value = value;
			numbers.push(symb);
			sign = 0;
			continue;
		}

		if (ch == '^' or ch == '+' or ch == '-' and sign == 0 or ch == '*' or ch == '/') 
		{

			if (operations.size() == 0) 
			{
				symb.type = ch;
				symb.value = 0;
				operations.push(symb);

				continue;
			}

			if (operations.size() != 0 && Priority(ch) > Priority(operations.top().type)) {
				symb.type = ch;
				symb.value = 0;
				operations.push(symb);
				continue;
			}
			if (operations.size() != 0 && Priority(ch) <= Priority(operations.top().type)) {
				cout << numbers.top().value << endl;
				if (Calc(numbers, operations, symb) == false) 
				{
					return 0;
				}
				i--;
				continue;
			}
		}

		if (ch == 'p') {
			if (chh[i + 1] == 'i') 
			{
				symb.type = '0';
				numbers.push(symb);
				sign = 0;
				i++;
				continue;
			}
		}

		if (ch == 40) {

			symb.type = ch;
			symb.value = 0;
			operations.push(symb);
			continue;
		}

		if (ch == 41) {

			while (operations.top().type != '(') {
				if (Calc(numbers, operations, symb) == false) 
				{
					return 0;
				}

			}
			operations.pop();
		}

		else 
		{
			cout << "некорркетный ввод" << endl;
			return 0;
		}
	}
	while (operations.size() != 0) {

		if (Calc(numbers, operations, symb) == false) 
		{
			return 0;
		}

	}
	cout << "ответ: " << numbers.top().value << endl;
	return 0;
}