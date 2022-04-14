#include "functions.h"


using namespace std;


void replace_words(Text& text) {
	for (int i = 0; i < text.getLen(); i++) {
		Line line = text.getLine(i);
		for (int j = 0; j < line.getLen(); j++) {
			Word word = line.getWord(j);
			if (!word.replaced) {

				Line new_line = replace_words_in_line(text, i, word, j + 1);
				text.setLine(i, new_line);

				for (int k = i + 1; k < text.getLen(); k++) {
					Line new_line = replace_words_in_line(text, k, word);
					text.setLine(k, new_line);
				}
			}
		}
	}
};


Line replace_words_in_line(Text text, int line_index, Word word, int start) {
	Line line = text.getLine(line_index);
	for (int j = start; j < line.getLen(); j++) {
		if (!line.getWord(j).replaced && line.getWord(j).equals(word)) {
			Word new_word = generate_new_word(word, word.getNumber());
			line.setWord(j, new_word);
		}
	}
	return line;
};


Word generate_new_word(Word word, int i) {
	char first_letter = word.getLetter(0);
	if (first_letter == word.getMarker()) {
		word.replaced = true;
		return word;
	}
	Word new_word;
	new_word.setMarker(word.getMarker());
	new_word.setLetter(0, first_letter);
	new_word.setLetter(1, '(');
	int num_len = 1;
	if (i <= 9) {
		new_word.setLetter(2, 48 + i);
	}
	else {
		int u = i;
		int i_digits[20];
		while (u >= 10) {
			i_digits[num_len - 1] = u % 10;
			u = (int)u / 10;
			num_len++;
		}
		i_digits[num_len - 1] = u;
		for (int h = 0; h < num_len; h++) {
			new_word.setLetter(2 + h, '0' + i_digits[num_len - h - 1]);
		}
	}
	new_word.setLetter(num_len + 2, ')');
	new_word.setLetter(num_len + 3, new_word.getMarker());
	new_word.replaced = true;
	
	return new_word;
};

