#include<iostream>
#include<cmath>
#include<vector>
#include<stack>
#include<sstream>
#include<stdexcept>
using namespace std;

bool check = 1;

struct vertex {

    pair<int, double> val; // 0 -- число, 1 -- (, 2 -- ), 3 -- *, 4 -- /, 5 -- +, 6 -- -
    vertex* next_vertex;
    vertex* prev_vertex;

    vertex(vertex* prv, vertex* nxt, pair<int, double> vl) {
        prev_vertex = prv;
        next_vertex = nxt;
        val = vl;
    }
};

vertex* replace_with_number(vertex* left, vertex* right, double num) { //указывают включительно
    vertex* vertex_num = new vertex(left->prev_vertex, right->next_vertex, make_pair(0, num));
    if (vertex_num->prev_vertex != nullptr) {
        vertex_num->prev_vertex->next_vertex = vertex_num;
    }
    if (vertex_num->next_vertex != nullptr) {
        vertex_num->next_vertex->prev_vertex = vertex_num;
    }
    return vertex_num;
}

void do_mult_div(vertex* lft, vertex* rght) { //указывают на скобки
    vertex* curr_num = lft->next_vertex;
    while (true) {
        if (curr_num->next_vertex == rght) {
            break;
        }
        vertex* operation = curr_num->next_vertex;
        vertex* next_num = operation->next_vertex;
        if (operation->val.first == 3 || operation->val.first == 4) {
            double n = 0;
            if (operation->val.first == 3) {
                n = (curr_num->val.second) * (next_num->val.second);
            }
            else {
                n = (curr_num->val.second) / (next_num->val.second);
            }
            curr_num = replace_with_number(curr_num, next_num, n);
        }
        else
        {
            curr_num = next_num;
        }
    }
}

void do_plus_minus(vertex* lft, vertex* rght) { //указывают на скобки
    vertex* curr_num = lft->next_vertex;
    while (true) {
        if (curr_num->next_vertex == rght) {
            break;
        }
        vertex* operation = curr_num->next_vertex;
        vertex* next_num = operation->next_vertex;
        double n;
        if (operation->val.first == 5) {
            n = (curr_num->val.second) + (next_num->val.second);
        }
        else {
            n = (curr_num->val.second) - (next_num->val.second);
        }
        curr_num = replace_with_number(curr_num, next_num, n);
    }
}

double calculate(vertex* start) {
    stack<vertex*> left_brackets;
    vertex* curr_vertex = start;
    while (true) {
        if (curr_vertex == nullptr) {
            throw runtime_error("Bad str");
        }
        if (curr_vertex->val.first == 1) {
            left_brackets.push(curr_vertex);
        }
        if (curr_vertex->val.first == 2) {
            if (left_brackets.empty()) {
                throw runtime_error("Bad str");
            }
            vertex* l_last = left_brackets.top();
            left_brackets.pop();
            do_mult_div(l_last, curr_vertex);
            do_plus_minus(l_last, curr_vertex);
            double n = curr_vertex->prev_vertex->val.second;
            replace_with_number(l_last, curr_vertex, n);
            if (curr_vertex->next_vertex == nullptr) {
                if (!left_brackets.empty()) {
                    throw runtime_error("Bad str");
                }
                return n;
            }
        }
        curr_vertex = curr_vertex->next_vertex;
    }
}

signed main() {

    freopen("input.txt", "r", stdin);
    freopen("output.txt", "w", stdout);

    string s0;
    getline(cin, s0);
    string s = "(";

    for (int i = 0; i < s0.length(); i++) {
        if ((i == 0 || s0[i - 1] == '(') && (s0[i] == '+' || s0[i] == '-')) {
            s += '0';
        }
        s += s0[i];
    }

    s += ")";

    vector<char> double_symbols = vector<char>{ '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    vector<pair<int, string>> parts;
    parts.push_back(make_pair(1, "("));
    string cash;

    for (int i = 1; i < s.length(); i++) {
        char ch = s[i];
        if (ch == '.' || ('0' <= ch && ch <= '9')) {
            cash += ch;
        }
        else {
            if (ch == '(' || ch == ')' || ch == '*' || ch == '/' || ch == '+' || ch == '-') {
                if (!cash.empty()) {
                    parts.push_back(make_pair(0, cash));
                    cash = "";
                    if (ch == '(') {
                        check = 0;
                        break;
                    }
                    else if (ch == ')') {
                        parts.push_back(make_pair(2, ")"));
                    }
                    else if (ch == '*') {
                        parts.push_back(make_pair(3, "*"));
                    }
                    else if (ch == '/') {
                        parts.push_back(make_pair(4, "/"));
                    }
                    else if (ch == '+') {
                        parts.push_back(make_pair(5, "+"));
                    }
                    else {
                        parts.push_back(make_pair(6, "-"));
                    }
                }
                else {
                    if (ch == '(' && s[i - 1] != ')') {
                        parts.push_back(make_pair(1, "("));
                    }
                    else if (ch == ')' && s[i - 1] == ')') {
                        parts.push_back(make_pair(2, ")"));
                    }
                    else if (s[i - 1] == ')') {
                        if (ch == '*') {
                            parts.push_back(make_pair(3, "*"));
                        }
                        else if (ch == '/') {
                            parts.push_back(make_pair(4, "/"));
                        }
                        else if (ch == '+') {
                            parts.push_back(make_pair(5, "+"));
                        }
                        else if (ch == '-') {
                            parts.push_back(make_pair(6, "-"));
                        }
                        else {
                            check = 0;
                            break;
                        }
                    }
                    else {
                        check = 0;
                        break;
                    }
                }
            }
            else {
                check = 0;
                break;
            }
        }
    }
    
    if (check) {
        vector<vertex*> vertexes;

        for (auto t : parts) {
            if (t.first == 0) {
                stringstream ss;
                ss << t.second;
                double x;
                ss >> x;
                vertexes.push_back(new vertex(nullptr, nullptr, make_pair(0, x)));
            }
            else {
                vertexes.push_back(new vertex(nullptr, nullptr, make_pair(t.first, 0)));
            }
        }

        int len = parts.size();
        vertexes[0]->next_vertex = vertexes[1];
        vertexes[len - 1]->prev_vertex = vertexes[len - 2];

        for (int i = 1; i < len - 1; i++) {
            vertexes[i]->prev_vertex = vertexes[i - 1];
            vertexes[i]->next_vertex = vertexes[i + 1];
        }

        double num = 0;

        try {
            num = calculate(vertexes[0]);
            cout << num;
        }
        catch (std::runtime_error e) {
            check = 0;
            cout << "Err";
        }
    }
    else {
        cout << "Err";
    }
}