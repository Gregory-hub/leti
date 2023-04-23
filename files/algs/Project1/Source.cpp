#include<iostream>
#include<fstream>
#include<vector>
#include<algorithm>
#include<string>
#include<bitset>
#include<map>
#include<math.h>
using namespace std;

int bin_to_dec(string code) {
	bitset<8> bin(code);
	return (uint8_t)bin.to_ulong();
}
string dec_to_bin(int num) {
	bitset<8> bin(num);
	return bin.to_string();
}

int bin_to_dec32(string code) {
	bitset<32> bin(code);
	return (uint8_t)bin.to_ulong();
}
string dec32_to_bin(int num) {
	bitset<32> bin(num);
	return bin.to_string();
}

int bin_to_dec16(string code) {
	bitset<16> bin(code);
	return (int)bin.to_ulong();
}
/*string int_to_bin(int count) {
	string bin = dec_to_bin(count);
	string bin1 = "";
	while (bin1.length() < 32) {
		if (bin1.length() + bin.length() < 32) {
			bin1 += '0';
		}
		else
			bin1 += bin;
	}
	return bin1;
}*/

string bin_int_to_char(string bin_code) {
	int ind = 0;
	string code8 = "", char_code = "";
	char c = '/0';
	for (int i = 0; i < 4; i++) {
		code8 = "";
		for (int j = 0; j < 8; j++) {
			ind = i * 8 + j;
			code8 += bin_code[ind];
		}
		c = (char)(bin_to_dec(code8));
		char_code += c;
	}
	return char_code;
}

int char_to_int(string code) {
	int count;
	string bin = "";
	for (int i = 0; i < 4; i++) {
		//if (bin_to_dec(dec_to_bin((int)code[i])) != 0) {
		bin += dec_to_bin((int)code[i]);
		//}
	}
	count = bin_to_dec32(bin);
	return count;
}
//RLE

string RLE(string data) {
	unsigned l = 0;
	int count = 0;
	string coding_string = "";
	char tmp = data[0];
	while (l < data.length()) {
		while (data[l] == tmp) {
			count++;
			l++;
		}
		string bin_count = dec32_to_bin(count);
		coding_string += bin_int_to_char(bin_count);
		coding_string += tmp;
		if (l == data.length()) {
			break;
		}
		tmp = data[l];
		//l++;
		count = 0;
	}
	return coding_string;
}

string inverse_RLE(string code) {
	string data = "";
	int i = 0, count = 0;
	string count_code = "";
	while (i < code.length() / 5) {
		count_code = "";
		for (int j = 0; j < 4; j++) {
			count_code += code[i * 5 + j];
		}
		count = char_to_int(count_code);
		for (int j = 0; j < count; j++) {
			data += code[i * 5 + 4];
		}
		i++;
	}
	return data;
}

//BWT
string BWT(string data, int &index) {
	string s;
	vector<string> tab;
	tab.push_back(data);
	s = data;
	for (int i = 0; i < s.length() - 1; i++) {
		s += s[0];
		s.erase(0, 1);
		tab.push_back(s);
	}
	sort(tab.begin(), tab.end());
	auto ind = find(tab.begin(), tab.end(), data);
	index = ind - tab.begin();
	s = "";
	for (int i = 0; i < tab.size(); i++) {
		s += tab[i][tab[i].length() - 1];
	}
	return s;
}

string inverse_BWT(string code, int index) {
	const int c = code.length();
	vector<string> tab(c);
	string tmp = "";
	for (int i = 0; i < code.length(); i++) {
		tmp = "";
		for (int j = 0; j < code.length(); j++) {
			tmp = "";
			tmp += code[j];
			tab[j] = tmp + tab[j];
		}
		sort(tab.begin(), tab.end());
	}
	return tab[index];
}

//MFT
string MTF(string data, string& alph) {
	string codding_string = "";
	vector<char> alphabet;
	for (char c : data) {
		if (find(alphabet.begin(), alphabet.end(), c) == alphabet.end() || alphabet.empty()) {
			alphabet.push_back(c);
		}
	}
	sort(alphabet.begin(), alphabet.end());
	string str(alphabet.begin(), alphabet.end()), tmp = "";
	alph = str;
	for (char c : data) {
		int ind = str.find(c);
		string bin_count = dec32_to_bin(ind);
		codding_string += bin_int_to_char(bin_count);
		tmp = "";
		tmp += c;
		str.erase(ind, 1);
		tmp += str;
		str = tmp;
	}
	return codding_string;
}

string inverse_MFT(string code, string alphabet) {
	string data = "";
	int i = 0, count = 0;
	string count_code = "", tmp = "";
	while (i < code.length() / 4) {
		count_code = "";
		for (int j = 0; j < 4; j++) {
			count_code += code[i * 4 + j];
		}
		count = char_to_int(count_code);
		data += alphabet[count];
		tmp = "";
		tmp += alphabet[count];
		alphabet.erase(count, 1);
		tmp += alphabet;
		alphabet = tmp;
		i++;
	}
	return data;

}

//LZ78
struct LZ78_node {
	char symbol = 0;
	int num = 0;
	vector<LZ78_node*> children;
	LZ78_node* parent = NULL;
	LZ78_node(char symbol, int num, LZ78_node* parent) {
		this->symbol = symbol;
		this->num = num;
		this->parent = parent;
	}
	LZ78_node* find_child(char key, bool& is_search) {
		is_search = false;
		for (int i = 0; i < children.size(); i++) {
			if (children[i]->symbol == key) {
				is_search = true;
				return children[i];
			}
			else
				return NULL;
		}
	}
	LZ78_node* find_child(int key, bool& is_search) {
		is_search = false;
		for (int i = 0; i < children.size(); i++) {
			if (children[i]->num == key) {
				is_search = true;
				return children[i];
			}
			else
				return NULL;
		}
	}
};

LZ78_node* find(LZ78_node* root, int num) {
	LZ78_node* tmp = root, *tmp1 = NULL ;
	bool is_search = true;
	if (tmp->num == num) {
		return tmp;
	}
	else {
		tmp1 = tmp->find_child(num, is_search);
		if (is_search) {
			return tmp1;
		}
		else {
			for (int i = 0; i < tmp->children.size(); i++) {
				tmp1 = find(tmp->children[i], num);
				if (tmp1 != NULL) {
					return tmp1;
				}
			}
		}
	}
	return NULL;
}
string LZ78(string data) {
	string code = "", tmps = "";
	int i = 0, j = 1;
	LZ78_node* root = new LZ78_node('/0', 0, NULL) ;
	LZ78_node* tmp = root, *tmp1 = NULL;
	bool is_search = true;
	string bin_count = "";
	while (i < data.length()) {
		tmp1 = tmp->find_child(data[i], is_search);
		if (tmp1 != NULL) {
			i++;
			if (i == data.length()) {
				bin_count = dec32_to_bin(tmp->num);
				code += bin_int_to_char(bin_count);
				code += data[i - 1];
			}
			tmp = tmp1;
		}	
		else {
			bin_count = dec32_to_bin(tmp->num);
			code += bin_int_to_char(bin_count);
			code += data[i];
			tmp->children.push_back(new LZ78_node(data[i], j, tmp));
			i++;
			j++;
			tmp = root;
		}
	}
	return code;
}

void invert_string(string& data) {
	char tmp = '/0';
	int count = data.length() - 1;
	for (int i = 0; i < data.length() / 2; i++) {
		tmp = data[i];
		data[i] = data[count - i];
		data[count - i] = tmp;
	}
}
string LZ78_decompres(string code) {
	string data = "", tmps = "", tmps1 = "";
	LZ78_node* root = new LZ78_node('\0', 0, NULL);
	int i = 0, j = 1;
	bool is_search;
	int count = 0;
	string count_code = "";
	LZ78_node* tmp = NULL, *tmp1 = NULL;
	while (i < code.length() / 5) {
		tmps1 = "";
		tmps1 += code[i * 2];
		count_code = "";
		for (int j = 0; j < 4; j++) {
			count_code += code[i * 5 + j];
		}
		count = char_to_int(count_code);
		tmp1 = find(root, count);
		tmps = "";
		tmp = tmp1;
		while (tmp != root) {
			tmps += tmp->symbol;
			tmp = tmp->parent;
		}
		invert_string(tmps);
		data += tmps;
		data += code[i * 5 + 4];
		tmp1->children.push_back(new LZ78_node(code[i * 5 + 4], j, tmp1));
		j++;
		i++;
	}
	return data;
}

//AC

void set_probablity(string data, map<char, double>& probablity, string& alph) {
	double l = (double)data.length();
	double pr = (double)1 / l;
	for (char c : data) {
		if (probablity[c] * 8 == 0) {
			alph += c;
		}
		probablity[c] ++;
	}
	for (char c : alph) {
		probablity[c] /= l;
	}
}

int find_index(string alph, char c) {
	int i = 0;
	for (i = 0; i < alph.length(); i++) {
		if (alph[i] == c) {
			break;
		}
	}
	return i;
}

void set_interval(map<char, double> probablity, string alph, vector<double>& a, vector<double>& b) {
	a.push_back(0);
	b.push_back(probablity[alph[0]]);
	
	for (int i = 1; i < alph.length() - 1; i++) {
		a.push_back(b[i - 1]);
		b.push_back(a[i] + probablity[alph[i]]);
	}
	a.push_back(b[alph.length() - 2]);
	b.push_back(1);
}

vector<double> /*string*/ AC(string data, int& length, string alph, vector<double> a, vector<double> b) {
	length = data.length();
	vector<double> code;
	double low = 0, low_pr = 0;
	double pow = 1, pow_pr = 0;
	long double l = 0, p = 0;
	int j = 0;
	int i = 0;
	bool is_first = true;
	double range = 0;
	char c = 0;
	for (int i = 0; i < data.length(); i++) {
		if (i == 37) {
			i = 37;
		}
		c = data[i];
		j = find_index(alph, c);
		low_pr = low;
		pow_pr = pow;
		range = pow_pr - low_pr;
		low = low_pr + a[j] * range;
		pow = low_pr + b[j] * range;
		double k;
		//is_first = ((low == low_pr && j == 0) || (pow == pow_pr && j == alph.length() - 1));
		if (j == 0) {
			if (pow == pow_pr || pow == low) {
				code.push_back(pow_pr);
				low = 0;
				pow = 1;
				i--;
			}
		}
		if (j == alph.length() - 1) {
			if (low == low_pr || pow == low) {
				code.push_back(pow_pr);
				low = 0;
				pow = 1;
				i--;
			}
		}
		if (j > 0 && j < alph.length() - 1) {
			if (low == low_pr || pow == pow_pr || pow == low) {
				code.push_back(pow_pr);
				low = 0;
				pow = 1;
				i--;
			}
		}
	}
	code.push_back(pow);
	return code;
}

string AC_decomrex(vector<double> code, string alph, vector<double> a, vector<double> b, int length) {
	double low = 0, low_pr = 0, low_tmp = 0;
	double pow = 1, pow_pr = 0, pow_tmp = 0;
	int j = 0;
	int i = 0;
	string data = "";
	double value = code[0];
	int k = 1;
	char c = 0;
	for (int i = 0; i < length; i++) {
		if (i == 115) {
			i = 115;
		}
		low_pr = low;
		pow_pr = pow;
		int j = 0;
		int count = 0;
		bool is_wrong = false;
		for (j = 0; j < alph.length(); j++) {
			low = low_pr + a[j] * (pow_pr - low_pr);
			pow = low_pr + b[j] * (pow_pr - low_pr);
			if (low != pow) {
				if (low < value && pow >= value) {
					c = alph[j];
					low_tmp = low;
					pow_tmp = pow;
					count++;
				}
			}
			else {
				if (low == pow) {
					if (pow <= value) {
						c = alph[j];
						low_tmp = pow;
						pow_tmp = pow;
					}
					break;
				}
			}
		}
		low = low_tmp;
		pow = pow_tmp;
		j = find_index(alph, c);
		/*low = low_pr + a[j] * (pow_pr - low_pr);
		pow = low_pr + b[j] * (pow_pr - low_pr);*/
		if (j == 0) {
			if (pow == pow_pr || low == pow) {
				value = code[k];
				k++;
				low = 0;
				pow = 1;
				i--;
			}
			else {
				data += alph[j];
			}
		}
		if (j == alph.length() - 1) {
			if (low == low_pr || low == pow) {
				value = code[k];
				k++;
				low = 0;
				pow = 1;
				i--;
			}
			else {
				data += alph[j];
			}
		}
		if (j > 0 && j < alph.length() - 1) {
			if (low == low_pr || pow == pow_pr || low == pow) {
				value = code[k];
				k++;
				low = 0;
				pow = 1;
				i--;
			}
			else {
				data += alph[j];
			}
		}
		
	}
	return data;
}

//PPM

double get_a(vector<pair<char, int>> freq, char c) {
	double first_prob = 0;
	int sum = 0;
	int freq_all = 0;
	for (auto i : freq) {
		if (i.first == c) {
			break;
		}
		sum += i.second;
		
	}
	for (auto i : freq) {
		freq_all += i.second;
	}
	first_prob = (double)sum / (double)freq_all;
	return first_prob;
}

double get_b(vector<pair<char, int>> freq, char c) {
	double second_prob = 0;
	int sum = 0;
	int freq_all = 0;
	for (auto i : freq) {
		if (i.first == c) {
			sum += i.second;
			break;
		}
		sum += i.second;
	}
	for (auto i : freq) {
		freq_all += i.second;
	}
	second_prob = (double)sum / (double)freq_all;
	return second_prob;
}

struct CM {
	string context;
	vector<pair<char, int>> freq;
	CM() {
		context = "";
		pair<char, int> a;
		a.first = '\0';
		a.second = 1;
		freq.push_back(a);
	}
	CM(string context) {
		this->context = context;
		pair<char, int> a;
		a.first = '\0';
		a.second = 1;
		freq.push_back(a);
	}
};

CM cm_negative1;
CM cm_0;
vector<CM> cm_1;
vector<CM> cm_2;

int find_model(string context_r, vector<CM> CM_List) {
	bool is_find = false;
	int index = 0;
	for (int i = 0; i < CM_List.size(); i++) {
		if (CM_List[i].context == context_r) {
			index = i;
			is_find = true;
			break;
		}
	}
	if (is_find) {
		return index;
	}
	else {
		return -1;
	}
}

int find_symbol(char symbol, CM context_mod) {
	int index = 0;
	bool is_find = false;
	for (int i = 0; i < context_mod.freq.size(); i++) {
		if (context_mod.freq[i].first == symbol) {
			index = i;
			is_find = true;
			break;
		}
	}
	if (is_find)
		return index;
	else
		return -1;
}

bool PPM_modeling(string context, char symbol, int order,  double& a, double& b, bool& is_end, bool& is_model) {
	int index_symbol = 0;
	int index_model = 0;
	switch (order) {
	case -1:
		is_model = true;
		index_symbol = find_symbol(symbol, cm_negative1);
		if (index_symbol == cm_negative1.freq.size() - 1) {
			is_end = true;
		}
		a = get_a(cm_negative1.freq, symbol);
		b = get_b(cm_negative1.freq, symbol);
		return false;
	case 0:
		is_model = true;
		index_symbol = find_symbol(symbol, cm_0);
		if (index_symbol < 0) {
			a = get_a(cm_0.freq, '\0');
			b = get_b(cm_0.freq, '\0');
			pair<char, int> p;
			p.first = symbol;
			p.second = 1;
			cm_0.freq.push_back(p);
			return true;
		}
		else {
			if (index_symbol == cm_0.freq.size() - 1) {
				is_end = true;
			}
			cm_0.freq[index_symbol].second++;
			a = get_a(cm_0.freq, symbol);
			b = get_b(cm_0.freq, symbol);
			return false;
		}
	case 1:
		index_model = find_model(context, cm_1);
		if (index_model < 0) {
			cm_1.push_back(CM(context));
			pair<char, int> p;
			p.first = symbol;
			p.second = 1;
			cm_1[cm_1.size() - 1].freq.push_back(p);
			is_model = false;
			return false;
		}
		else {
			is_model = true;
			index_symbol = find_symbol(symbol, cm_1[index_model]);
			if (index_symbol < 0) {
				a = get_a(cm_1[index_model].freq, '\0');
				b = get_b(cm_1[index_model].freq, '\0');
				pair<char, int> p;
				p.first = symbol;
				p.second = 1;
				cm_1[index_model].freq.push_back(p);
				return true;
			}
			else {
				if (index_symbol == cm_1[index_model].freq.size() - 1) {
					is_end = true;
				}
				cm_1[index_model].freq[index_symbol].second++;
				a = get_a(cm_1[index_model].freq, symbol);
				b = get_b(cm_1[index_model].freq, symbol);
				return false;
			}
		}
	case 2:
		index_model = find_model(context, cm_2);
		if (index_model < 0) {
			cm_2.push_back(CM(context));
			pair<char, int> p;
			p.first = symbol;
			p.second = 1;
			cm_2[cm_2.size() - 1].freq.push_back(p);
			is_model = false;
			return true;
		}
		else {
			is_model = true;
			index_symbol = find_symbol(symbol, cm_2[index_model]);
			if (index_symbol < 0) {
				a = get_a(cm_2[index_model].freq, '\0');
				b = get_b(cm_2[index_model].freq, '\0');
				pair<char, int> p;
				p.first = symbol;
				p.second = 1;
				cm_2[index_model].freq.push_back(p);
				return true;
			}
			else {
				if (index_symbol == cm_2[index_model].freq.size() - 1) {
					is_end = true;
				}
				cm_1[index_model].freq[index_symbol].second++;
				a = get_a(cm_2[index_model].freq, symbol);
				b = get_b(cm_2[index_model].freq, symbol);
				return false;
			}
		}
	default:
		return false;
	}
	return false;
}

int find_symbol(char c, vector<pair<char, int>> freq) {
	for (int i = 0; i < freq.size(); i++) {
		if (freq[i].first == c) {
			return i;
		}
	}
}

void PPM_initializetion(string data) {
	for (char c : data) {
		if (find_symbol(c, cm_negative1.freq) < 0) {
			pair<char, int> p;
			p.first = c;
			p.second = 1;
			cm_negative1.freq.push_back(p);
		}
	}
}

vector<double> /*string*/ PPM_codding(string data, vector<pair<char, int>>& alph) {
	PPM_initializetion(data);
	alph = cm_negative1.freq;
	int length = data.length();
	vector<double> code;
	double low = 0, low_pr = 0;
	double pow = 1, pow_pr = 0;
	long double l = 0, p = 0;
	bool is_end = false;
	int i = 0;
	bool is_first = true;
	double range = 0;
	char c = 0;
	int order = 0;
	string tmp = "";
	string context = "";
	bool is_esc = false;
	bool is_model = false;
	for (int i = 0; i < data.length(); i++) {
		c = data[i]; 
		tmp += c;
		if (i != 0) {
			order = min((int)(tmp.length() - 1), 2);
		}
		else {
			order = -1;
			pair<char, int> p;
			p.first = c;
			p.second = 1;
			cm_0.freq.push_back(p);
		}
		for (int j = order; j >= -1; j--) {
			context = "";
			low_pr = low;
			pow_pr = pow;
			range = pow_pr - low_pr;
			double a;
			double b;
			for (int k = j; k > 0; k--) {
				context += tmp[tmp.length() - k - 1];
			}
			is_esc = PPM_modeling(context, c, j, a, b, is_end, is_model);
			if (!is_model) {
				continue;
			}
			else if (is_esc) {
				low = low_pr + a * range;
				pow = low_pr + b * range;
				//is_first = ((low == low_pr && j == 0) || (pow == pow_pr && j == alph.length() - 1));
				if (is_esc) {
					if (pow == pow_pr || pow == low) {
						code.push_back(pow_pr);
						low = 0;
						pow = 1;
					}
				}
				else if (is_end) {
					if (low == low_pr || pow == low) {
						code.push_back(pow_pr);
						low = 0;
						pow = 1;
					}
				}
				else {
					if (low == low_pr || pow == pow_pr || pow == low) {
						code.push_back(pow_pr);
						low = 0;
						pow = 1;
					}
				}
				continue;
			}
			else {
				low = low_pr + a * range;
				pow = low_pr + b * range;
				//is_first = ((low == low_pr && j == 0) || (pow == pow_pr && j == alph.length() - 1));
				if (is_esc) {
					if (pow == pow_pr || pow == low) {
						code.push_back(pow_pr);
						low = 0;
						pow = 1;
						i--;
					}
				}
				else if (is_end) {
					if (low == low_pr || pow == low) {
						code.push_back(pow_pr);
						low = 0;
						pow = 1;
						i--;
					}
				}
				else {
					if (low == low_pr || pow == pow_pr || pow == low) {
						code.push_back(pow_pr);
						low = 0;
						pow = 1;
						i--;
					}
				}
				break;
			}
		}
	}
	code.push_back(pow);
	return code;
}

bool PPM_decomp_modeling(string context, double value, int order, int index, double& low, double& pow, char& c, bool& is_model, bool& is_search) {
	int index_symbol = 0;
	int index_model = 0;
	double a = 0;
	double b = 0;
	switch (order) {
	case -1:
		is_model = true;
		is_search = false;
		c = cm_negative1.freq[index].first;
		a = get_a(cm_negative1.freq, c);
		b = get_b(cm_negative1.freq, c);
		pow = low + b * (pow - low);
		low = low + a * (pow - low);
		if (low < value && pow >= value) {
			is_search = true;
		}
		return false;
	case 0:
		is_model = true;
		is_search = false;
		c = cm_0.freq[index].first;
		a = get_a(cm_0.freq, c);
		b = get_b(cm_0.freq, c);
		pow = low + b * (pow - low);
		low = low + a * (pow - low);
		if (low < value && pow >= value) {
			is_search = true;
			if (index = 0) {
				return true;
			}
			else {
				return false;
			}
		}
	case 1:
		index_model = find_model(context, cm_1);
		if (index_model < 0) {
			cm_1.push_back(CM(context));
			is_model = false;
			is_search = false;
			return false;
		}
		else {
			is_model = true;
			is_search = false;
			c = cm_1[index_model].freq[index].first;
			a = get_a(cm_0.freq, c);
			b = get_b(cm_0.freq, c);
			pow = low + b * (pow - low);
			low = low + a * (pow - low);
			if (low < value && pow >= value) {
				is_search = true;
				if (index = 0) {
					return true;
				}
				else {
					return false;
				}
			}
		}
	case 2:
		index_model = find_model(context, cm_1);
		if (index_model < 0) {
			cm_2.push_back(CM(context));
			is_model = false;
			is_search = false;
			return false;
		}
		else {
			is_model = true;
			is_search = false;
			c = cm_2[index_model].freq[index].first;
			a = get_a(cm_0.freq, c);
			b = get_b(cm_0.freq, c);
			pow = low + b * (pow - low);
			low = low + a * (pow - low);
			if (low < value && pow >= value) {
				is_search = true;
				if (index = 0) {
					return true;
				}
				else {
					return false;
				}
			}
		}
	default:
		return false;
	}
	return false;
}

// string PPM_decompres(vector<double> code, vector<pair<char, int>>& alph) {
// 	cm_negative1.freq = alph;
// 	double low = 0, low_pr = 0, low_tmp = 0;
// 	double pow = 1, pow_pr = 0, pow_tmp = 0;
// 	int j = 0;
// 	int i = 0;
// 	string data = "";
// 	string context = "";
// 	double value = code[0];
// 	int k = 1;
// 	char c = 0;
// 	int order = 0;
// 	bool is_search = false;
// 	bool is_model = false;
// 	bool is_esc = false;
// 	while (k < code.size()) {
// 		low_pr = low;
// 		pow_pr = pow;
// 		int j = 0;
// 		int count = 0;
// 		bool is_wrong = false;
// 		if (i != 0) {
// 			order = min((int)(data.length() - 1), 2);
// 		}
// 		else {
// 			order = -1;
// 		}
// 		for (j = order; j <= -1; j--) {
// 			for (int i = j; i > 0; i--) {
// 				context += data[data.length() - k - 1];
// 			}
// 			switch (j) {
// 			case -1:
// 				i = 0;
// 				for (i = 0; i < cm_negative1.freq.size(); i++) {
// 					is_esc = PPM_decomp_modeling(context, value, j, i, low, pow, c, is_model, is_search);
// 					if (is_search) {
// 						break;
// 					}
// 				}
				
// 			case 0:
				
// 			case 1:
				
// 			case 2:
				
// 			default:
// 				return false;
// 			}
// 			if (low != pow) {
// 				if (low < value && pow >= value) {
// 					c = alph[j];
// 					low_tmp = low;
// 					pow_tmp = pow;
// 					count++;
// 				}
// 			}
// 			else {
// 				if (low == pow) {
// 					if (pow <= value) {
// 						c = alph[j];
// 						low_tmp = pow;
// 						pow_tmp = pow;
// 					}
// 					break;
// 				}
// 			}
// 		}
// 		low = low_tmp;
// 		pow = pow_tmp;
// 		j = find_index(alph, c);
// 		/*low = low_pr + a[j] * (pow_pr - low_pr);
// 		pow = low_pr + b[j] * (pow_pr - low_pr);*/
// 		if (j == 0) {
// 			if (pow == pow_pr || low == pow) {
// 				value = code[k];
// 				k++;
// 				low = 0;
// 				pow = 1;
// 				i--;
// 			}
// 			else {
// 				data += alph[j];
// 			}
// 		}
// 		if (j == alph.length() - 1) {
// 			if (low == low_pr || low == pow) {
// 				value = code[k];
// 				k++;
// 				low = 0;
// 				pow = 1;
// 				i--;
// 			}
// 			else {
// 				data += alph[j];
// 			}
// 		}
// 		if (j > 0 && j < alph.length() - 1) {
// 			if (low == low_pr || pow == pow_pr || low == pow) {
// 				value = code[k];
// 				k++;
// 				low = 0;
// 				pow = 1;
// 				i--;
// 			}
// 			else {
// 				data += alph[j];
// 			}
// 		}

// 	}
// 	return data;
// }

int main() {
	int l = 0;
	string input = "hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud hamuud! hamud hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud hamuud! hamud hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud hamuud! hamud hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud habibi hamuud! hamud hamuud! hamud ";
	fstream f, f1;
	/*f.open("in.txt", ios::out);
	for (int i = 0; i < 17; i++) {
		f << input;
	}
	input = "";
	f.close();
	f1.open("in.txt", ios::in);
	f1 >> input;*/
	/*map<char, int> freq;
	for (char c : input) {
		freq[c]++;
	}*/
	// vector<double, allocator<double>> answer;
	 string answer1 = "";
	vector<double> answer; 
	// vector<pair<char, int>> alph;
  	// answer = PPM_codding(input, alph);
	int lenght = 0;
	map<char, double> probablity;
	string alph = "";
	set_probablity(input, probablity, alph);
	vector<double> a, b;
	set_interval(probablity, alph, a, b);
	answer = AC(input, lenght, alph, a, b);
	//for (int i = 0; i < answer.size(); i++) cout << answer[i];
	//cout << endl;
	answer1 += AC_decomrex(answer, alph, a, b, lenght);
	cout << answer1 << endl;
	// bool right = false;
	// if (answer1 == input) {
	// 	right = true;
	// }
	// int i = 0;
	// for (i = 0; i < input.length(); i++) {
	// 	if (answer1[i] != input[i])
	// 		break;
	// }
	// cout << i << '\n';
	//answer1 = AC_decomrex(answer, freq, b);
	/*string answer = LZ78(input);
	cout << "LZ78" << '\n';
	cout << answer << '\n';
	string answer1 = inverse_LZ78(answer);
	cout << answer1 << '\n';*/
	//cin >> input;
	/*string alphabet = "";
	string answer = MTF(input, alphabet);
	string answer1 = "";
	int i = 0, count = 0;
	string count_code = "";
	while (i < answer.length() / 4) {
		count_code = "";
		for (int j = 0; j < 4; j++) {
			count_code += answer[i * 4 + j];
		}
		count = char_to_int(count_code);
		answer1 += to_string(count);
		i++;
	}
	cout << answer1 << '\n';
	answer = inverse_MFT(answer, alphabet);
	cout << answer;
	int index = 0;
	cout << "RLE: " << '\n';
	answer = RLE(input);
	answer1 = "";
	count_code = "";
	i = 0;
	count = 0;
	while (i < answer.length() / 5) {
		count_code = "";
		for (int j = 0; j < 4; j++) {
			count_code += answer[i * 5 + j];
		}
		count = char_to_int(count_code);
		answer1 += to_string(count);
		answer1 += answer[i * 5 + 4];
		i++;
	}
	cout << answer1 << '\n';
	answer = inverse_RLE(answer);
	cout << answer << '\n';*/
	/*cout << "BWT: " << '\n';
	int index = 0;
	answer = BWT(input, index);
	cout << answer << ' ' << index << '\n';
	string data = inverse_BWT(answer, index);
	cout << data << '\n';
	/*cout << "RLE: " << '\n';
	answer = RLE(answer);
	cout << answer << '\n';*/
	return 0;
}