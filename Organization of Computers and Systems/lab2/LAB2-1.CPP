#include <conio.h>
#include <dos.h>

void main() {
	clrscr();

	union REGS r;

	r.h.ah = 1; //cursor
	r.h.cl = 0;
	int86(0x10, &r, &r);

	char colors[16][13] = {
		"BLACK",
		"BLUE",
		"GREEN",
		"CYAN",
		"RED",
		"MAGENTA",
		"BROWN",
		"LIGHTGRAY",
		"DARKGRAY",
		"LIGHTBLUE",
		"LIGHTGREEN",
		"LIGHTCYAN",
		"LIGHTRED",
		"LIGHTMAGENTA",
		"YELLOW",
		"WHITE"
	};

	window(25, 5, 55, 15);

	for (int i = 0; i < 8; i++) {
		textbackground(i);
		for (int j = 0; j < 16; j++) {
			textcolor(j);
			cprintf("Bg: %s, f: %s\n\r", colors[i], colors[j]);
			delay(600);
		}
	}
	getch();

	// scrolling
	r.h.ah = 7;
	r.h.al = 1;
	r.h.ch = 4;
	r.h.cl = 24;
	r.h.dh = 14;
	r.h.dl = 54;
	r.h.bh = 0;

	for (int k = 0; k < 12; k++) {
		int86(0x10, &r, &r);
	}

}