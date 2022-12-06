#include <dos.h>
#include <conio.h>
#include <stdio.h>

int x1 = 25;
int y1 = 5;
int x2 = 55;
int y2 = 15;

int x = 1;
int y = 1;
int ch = 0;
int left = 0;
int down = 0;
int move = 0;
int main()

{

	window(x1, y1, x2, y2);
	textbackground(RED);
	textcolor(YELLOW);

	clrscr();
	do {
		clrscr();
		if ((left == 0) && (down == 0)){
			printf("exit - F1, start - any arrow");
		}
		else {
			if (move == 0){
				x = x;
				y = y;
			}
			else {
				if ((left == 1) && (down == -1) && ((x - 1) >= 0))
					x --;
				if ((left == 0) && (down == -1) && ((x + 1) <= (x2 - x1)))
					x++;
				if ((left == -1) && (down == 0) && ((y - 1) >= 0))
					y--;
				if ((left == -1) && (down == 1) && ((y + 1) <= (y2 - y1)))
					y++;
			}
			gotoxy(x, y);
			putch('*');
			delay(100);
		}
		ch = getch();
		if (ch == 0 || ch == 224){
			switch (getch()){
			case 75:
				move = 1;
				left = 1;
				down = -1;
				break;
			case 77:
				move = 1;
				left = 0;
				down = -1;
				break;
			case 72:
				move = 1;
				left = -1;
				down = 0;
				break;
			case 80:
				move = 1;
				left = -1;
				down = 1;
				break;
			case 59:
				return 0;
				break;
			default:
				move = 0;
				break;
			}
		}
		else
			move = 0;
	} while (ch != 27);
	return 0;

}

