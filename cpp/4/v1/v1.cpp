// Автор: Новиков Г.В.
// Группа: 1302
// Дата начала: 11.11.2021
// Дата окончания: 12.11.2021
// Версия: 4.1.01

#include <iostream>
#include <fstream>
#include <vector>

using namespace std;


vector<int> read_array(string arrayname) {
    fstream file;
    file.open("arrays/" + arrayname + ".txt", ios::in);

    int array_size;
    file >> array_size;

    if (array_size < 0) {
        array_size = 0;
    }

    vector<int> array;

    int el;
    while (file >> el) {
        array.push_back(el);
    }

    file.close();

    bool sizes_are_equal = array.size() == array_size;
    if (!sizes_are_equal) {
        cerr << "Array " + arrayname + ": invalid data(first line and number of elements do not match)" << endl;
        exit(1);
    }

    return array;
}


vector<int> concatenate_vecs(vector<int> F, vector<int> G, vector<int> H) {
    vector<int> Q = F;
    
    for(int i = 0; i < G.size(); i++) {
        Q.push_back(G[i]);
    }

    for(int i = 0; i < H.size(); i++) {
        Q.push_back(H[i]);
    }

    return Q;
}


void out(vector<int> F, vector<int> G, vector<int> H, vector<int> Q) {
    fstream file;
    file.open("out.txt", ios::out);

    file << "F:" << endl;
    for(int i = 0; i < F.size(); i++) {
        file << F[i] << endl;
    }
    
    file << endl << "G:" << endl;
    for(int i = 0; i < G.size(); i++) {
        file << G[i] << endl;
    }

    file << endl << "H:" << endl;
    for(int i = 0; i < H.size(); i++) {
        file << H[i] << endl;
    }

    file << endl << "Q:" << endl;
    for(int i = 0; i < Q.size(); i++) {
        file << Q[i] << endl;
    }

    file.close();
}


int main(int argc, char const *argv[])
{
    vector<int> F = read_array("F");
    vector<int> G = read_array("G");
    vector<int> H = read_array("H");

    vector<int> Q = concatenate_vecs(F, G, H);
    out(F, G, H, Q);

    return 0;
}
