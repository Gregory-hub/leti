#include "Word.h"


class Line {
	public:
		Word getWord(int index);
		void setWord(int index, Word word);
		int getLen();
		void setLen(char length);
		void printWord(int index, fstream &file);

	private:
		Word words[30];
		int len = 0;
};

