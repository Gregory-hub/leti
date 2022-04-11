#include "functions.h"


using namespace std;


void replace_words(Text &text) {
	for (int i = 0; i < text.getLen(); i++) {
		Word word = text.getWord(i);
		if (!word.replaced) {
			char first_letter = word.getLetter(0);
			//cout << first_letter << ' ' << i << ' ' << word.replaced << endl;
			if (first_letter == word.getMarker()) {
				word.replaced = true;
			}
			else {
				for (int j = i + 1; j < text.getLen(); j++) {
					if (!text.getWord(j).replaced && text.getWord(j).equals(word)) {
						Word new_word = generate_new_word(text, first_letter, i, j);
						text.setWord(j, new_word);
					}
				}
			}
		}
	}
}

Word generate_new_word(Text text, char first_letter, int i, int j) {
	Word new_word;
	new_word.setMarker(text.getWord(j).getMarker());
	new_word.setLetter(0, first_letter);
	new_word.setLetter(1, '(');
	int num_len = 0;
	if (i <= 9) {
		new_word.setLetter(2, 48 + i);
		num_len = 1;
	}
	else {
		int u = i;
		int i_digits[20];
		while (u % 10 != 0) {
			i_digits[num_len] = u % 10;
			u = (int)u / 10;
			num_len++;
		}
		for (int h = 0; h < num_len; h++) {
			new_word.setLetter(2 + h, '0' + i_digits[num_len - h - 1]);
		}
	}
	new_word.setLetter(num_len + 2, ')');
	new_word.setLetter(num_len + 3, new_word.getMarker());
	new_word.replaced = true;
	return new_word;
}

