#include<stdio.h>
#include<graphics.h>
#include<conio.h>
#include<dos.h>
void tm_delay(){
	union REGS r;
	r.h.ah = 0x86;
	//r.h.al = 1;
	r.x.cx = 0x1E;
	r.x.dx = 0x8480;
	int86(0x15, &r, &r);
}
int main(){

	int graph_driver = 0;
	int graph_mode;
	detectgraph(&graph_driver, &graph_mode);
	initgraph(&graph_driver, &graph_mode, "C:\\TURBOC3\\BGI");
	setbkcolor(RED);
	setcolor(YELLOW);
	int k = 0, i = 0;
	int x = getmaxx()/2;
	int y = getmaxy()/2;
	int xt = x -textwidth("to exit press any key")/2;
	outtextxy(xt, y, "to exit press any key");
	delay (3000);
	while (kbhit() == 0){
		//clrscr();
		clearviewport();
		//putpixel(x, y, YELLOW);
		if (k == 0){
			line(x - 30, y - 30, x + 30, y - 30);
		}
		if (k == 1){
			line(x + 30, y - 30, x + 30, y + 30);
		}
		if (k == 2){
			line(x - 30, y + 30, x + 30, y + 30);
		}
		if (k == 3){
			line(x - 30, y + 30, x - 30, y - 30);
		}
		tm_delay();
		k++;
		if (k == 4)
			k = 0;
	}
	return 0;
}