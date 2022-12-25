#include <iostream>
#include <string>
#include <stack>
#include <vector>
#include <stdlib.h>
#include <algorithm>
#include <math.h>


using namespace std;


struct Node
{
    string data;
    Node* left = nullptr;
    Node* right = nullptr;
};

bool is_digit(char c)
{
    return (c >= '0' && c <= '9');
}

bool is_number(string num)
{
    // check if number is float (has no more than one '.' and '-' at index 0 and the rest are digits)
    if (num.length() == 0) return false;
    int i = 0;
    if (num[0] == '-') {
        if (num.length() == 1) return false;
        i++;
    }
    int dots_count = 0;
    for (; i < num.length(); i++) {
        if (num[i] == '.') {
            if (dots_count > 0) return false;
            dots_count++;
        }
        else if (!is_digit(num[i])) return false;
    }
    return true;
}

bool is_operator(string s)
{
    if (s.length() != 1) return false;
    char c = s[0];
    return (c == '+' || c == '-' || c == '*' || c == '/' || c == '^');
}

bool is_bracket(string s)
{
    if (s.length() != 1) return false;
    char c = s[0];
    return (c == '(' || c == ')');
}

Node* create_node(int data)
{
    Node* tmp = new Node;
    tmp->left = nullptr;
    tmp->right = nullptr;
    tmp->data = data;
    return tmp;
};

int find_closing_bracket(const vector<string>& expr, int opening_br_i) {
    int level = 1;
    int i = 0;
    while (i != opening_br_i) {
        if (expr[i] == "(") level++;
        i++;
    }

    i++;
    while (i < expr.size() && level != 0) {
        if (expr[i] == "(") level++;
        if (expr[i] == ")") level--;
        i++;
    }

    if (level != 0) return -1;

    return i - 1;
}

Node* tree_from_prefix(vector<string>& expr) {
    if (expr.size() < 3) return nullptr;

    Node* root = new Node;
    root->data = expr.front();
	expr.erase(expr.begin());

    if (is_number(expr.front())) {
        Node* left = new Node;
        left->data = expr.front();
		expr.erase(expr.begin());
        root->left = left;
    }
    else if (is_operator(expr.front())) {
        root->left = tree_from_prefix(expr);
    }

    if (is_number(expr.front())) {
        Node* right = new Node;
        right->data = expr.front();
		expr.erase(expr.begin());
        root->right = right;
    }
    else if (is_operator(expr.front())) {
        root->right = tree_from_prefix(expr);
    }

    return root;
}

Node* tree_from_postfix(vector<string>& expr) {
    if (expr.size() < 3) return nullptr;

    Node* root = new Node;
    root->data = expr.back();
    expr.pop_back();

    if (is_number(expr.back())) {
        Node* right = new Node;
        right->data = expr.back();
		expr.pop_back();
        root->right = right;
    }
    else if (is_operator(expr.back())) {
        root->right = tree_from_postfix(expr);
    }

    if (is_number(expr.back())) {
        Node* left = new Node;
        left->data = expr.back();
		expr.pop_back();
        root->left = left;
    }
    else if (is_operator(expr.back())) {
        root->left = tree_from_postfix(expr);
    }

    return root;
}

Node* tree_from_infix(vector<string>& expr) {
    Node* root = nullptr;

    switch (expr.size())
    {
    case 0:
        return nullptr;
    case 1:
        root = new Node;
        root->data = expr.front();
        return root;
    case 3:
        root = new Node;
        root->data = expr[1];
        root->left = new Node;
        root->left->data = expr.front();
        root->right = new Node;
        root->right->data = expr.back();
        return root;
    default:
        break;
    }

    if (!is_number(expr[0]) && !is_bracket(expr[0])) {
        cerr << "Invalid expression" << endl;
        exit(1);
    }

    if (is_number(expr[0]) && is_operator(expr[1]) && expr[2] == "(") {
        int closing_br_i = find_closing_bracket(expr, 2);

        vector<string> inner_expr = vector<string>(expr.begin() + 3, expr.begin() + closing_br_i);
        vector<string> further_expr;
        if (expr.size() > closing_br_i + 1) {
            further_expr = vector<string>(expr.begin() + closing_br_i + 1, expr.end());
        }

        if (further_expr.size() == 0) {
            root = new Node;
            root->data = expr[1];
            root->left = new Node;
            root->left->data = expr[0];
            root->right = tree_from_infix(inner_expr);
            expr.erase(expr.begin(), expr.begin() + 2);     // at 0 and 1
        }

        else if (is_operator(further_expr[0])) {
            root = new Node;
            root->data = further_expr[0];
            vector<string> left_expr = vector<string>(expr.begin(), expr.begin() + closing_br_i + 1);
            root->left = tree_from_infix(left_expr);
            root->right = tree_from_infix(further_expr);
        }
    }

    else if (expr[0] == "(") {
        int closing_br_i = find_closing_bracket(expr, 0);
        vector<string> inner_expr = vector<string>(expr.begin() + 1, expr.begin() + closing_br_i);
        vector<string> further_expr;

        if (expr.size() > closing_br_i + 1) {
            further_expr = vector<string>(expr.begin() + closing_br_i + 1, expr.end());
        }

        if (further_expr.size() == 0) {
            root = tree_from_infix(inner_expr);
        }

        else if (is_operator(further_expr[0])) {
            root = new Node;
            root->data = further_expr[0];
            further_expr.erase(further_expr.begin());
            root->left = tree_from_infix(inner_expr);
            root->right = tree_from_infix(further_expr);
        }
    }

    return root;
}

Node* construct_tree(string expr_str) {
    // if empty
    if (expr_str.find_first_not_of(' ') == string::npos) return nullptr;
    // convert expr_str to vector(array) expr
    vector<string> expr;
    int st = 0;
    for (int i = 0; i < expr_str.length(); i++) {
        if (expr_str[i] == '(' || expr_str[i] == ')') {
            if (i != st) expr.push_back(expr_str.substr(st, i - st));
            expr.push_back(expr_str.substr(i, 1));
            st = i + 1;
        }
        else if (expr_str[i] == ' ') {
            if (i != st) expr.push_back(expr_str.substr(st, i - st));
            st = i + 1;
        }
    }
    if (st != expr_str.length()) expr.push_back(expr_str.substr(st, expr_str.length() - st));

    Node* root = new Node;
    // infix
    if ((is_number(expr[0]) or is_bracket(expr[0])) && !is_operator(expr[expr.size() - 1])) {
        string prev = "";
        int br_level = 0;
        for (string& x : expr) {
            if (!(is_operator(x) || is_number(x) || is_bracket(x))) return nullptr;
            if (is_operator(prev) && (is_operator(x) || is_bracket(x) && x[0] == ')')) return nullptr;
            if (is_number(prev) && (is_number(x) || is_bracket(x) && x[0] == '(')) return nullptr;
            if (is_bracket(prev)) {
                if (prev[0] == '(' && is_operator(x)) return nullptr;
				if (prev[0] == ')' && is_number(x)) return nullptr;
            }
            if (is_bracket(x)) {
                if (x[0] == '(') br_level++;
                if (x[0] == ')') br_level--;
                if (br_level < 0) return nullptr;
            }
			prev = x;
        }
        if (br_level != 0) return nullptr;
		if (is_operator(expr[expr.size() - 1])) return nullptr;
        root = tree_from_infix(expr);
    }
    else {
        // prefix and postfix validations
        if (expr.size() < 3) return nullptr;
        int num_count = 0;
        int operators_count = 0;
        for (string& x : expr) {
            if (is_number(x)) num_count++;
            else if (is_operator(x)) operators_count++;
            else return nullptr;
        }
        if (operators_count != num_count - 1) return nullptr;

		// prefix
        if (is_operator(expr[0])) {
            if (is_operator(expr[expr.size() - 1])) return nullptr;
            if (is_operator(expr[expr.size() - 2])) return nullptr;
            root = tree_from_prefix(expr);
        }
        // postfix
        else if (is_operator(expr[expr.size() - 1])) {
            if (is_operator(expr[0])) return nullptr;
            if (is_operator(expr[1])) return nullptr;
            root = tree_from_postfix(expr);
        }
        else return nullptr;
    }

    return root;
}

float calculate(Node* root)
{
    if (root == nullptr) return 0;
    if (root->left == nullptr && root->right == nullptr) return stof(root->data);
    float left = calculate(root->left);
    float right = calculate(root->right);
    if (root->data == "*") return left * right;
    if (root->data == "/") return left / right;
    if (root->data == "+") return left + right;
	if (root->data == "-") return left - right;
	if (root->data == "^") return pow(left, right);
    return 0;
}

void print_node(Node* node)
{
    cout << node->data;
}

void print_preorder(Node* root)
{
    if (root != nullptr)
    {
        print_node(root);
        cout << ' ';
        print_preorder(root->left);
        print_preorder(root->right);
    }
}

void print_postorder(Node* root)
{
    if (root != nullptr)
    {
        print_postorder(root->left);
        print_postorder(root->right);
        print_node(root);
        cout << ' ';
    }
}

void print_inorder(Node* root, int depth = 0)
{
    if (root == nullptr) return;
    if (is_operator(root->data)) {
        if (depth != 0) cout << "(";
        print_inorder(root->left, depth + 1);
        cout << ' ';
        print_node(root);
        cout << ' ';
        print_inorder(root->right, depth + 1);
        if (depth != 0) cout << ")";
    }
    else {
        print_node(root);
    }
}

void print_calc(Node* tree)
{
    cout << "\nResult: " << calculate(tree) << endl;
}

void print(Node* tree)
{
    cout << "\nPreorder: ";
    print_preorder(tree);
    cout << "\nPostorder: ";
    print_postorder(tree);
    cout << "\nInorder: ";
    print_inorder(tree);
    print_calc(tree);
}

void destroy(Node* root) {
    if (root == nullptr) return;
    destroy(root->left);
    destroy(root->right);
    delete root;
}

int main()
{
    string expression= "- * 9 7 + 4 2";
    Node* tree = construct_tree(expression);
	cout << "Expression: " << expression << endl;
    if (tree == nullptr) {
        cout << "Invalid expression" << endl;
    }
    else {
        print(tree);
        cout << endl;
    }

    expression = "7 2 + 5 *";
	cout << "\nExpression: " << expression << endl;
    Node* tree2 = construct_tree(expression);
    if (tree2 == nullptr) {
        cout << "Invalid expression" << endl;
    }
    else {
        print(tree2);
        cout << endl;
    }

    // needs brackets for operation order
    expression = "(-2 + (2 * 4)) / 3";
	cout << "\nExpression: " << expression << endl;
    Node* tree3 = construct_tree(expression);
    if (tree3 == nullptr) {
        cout << "Invalid expression" << endl;
    }
    else {
        print(tree3);
        cout << endl;
    }

    destroy(tree);
    destroy(tree2);
    destroy(tree3);

    return 0;
}

