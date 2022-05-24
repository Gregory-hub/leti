#include <iostream>
#include "functions.h"

int main()
{
    cout << "Author: Novikov Gregory\n"
        "Start date: 23.05.2022\n"
        "End date: 24.05.2022\n"
        "Version: 3.1\n" << endl;

    Elem* first = in("in.txt");
    Elem* first_inserted = in("elements.txt");

    string out_filename = "out.txt";
    fstream file;
    file.open(out_filename, ios::out);

    file << "INPUT" << endl;
    cout << "INPUT" << endl;
    out(file, first);

    file << endl << "ELEMENTS TO INSERT" << endl;
    cout << endl << "ELEMENTS TO INSERT" << endl;
    out(file, first_inserted);

    first = insert_before_last(first, first_inserted);

    file << endl << "OUTPUT" << endl;
    cout << endl << "OUTPUT" << endl;
    out(file, first);

    desintegrate(first);

    file << endl << "AFTER DELETION" << endl;
    cout << endl << "AFTER DELETION" << endl;
    out(file, first);

    file.close();
    return 0;
}

