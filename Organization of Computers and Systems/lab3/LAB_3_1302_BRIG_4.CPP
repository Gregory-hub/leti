#include<graphics.h>
#include<math.h>
#include<conio.h>
#include<stdio.h>

int main() {
	clrscr();

	int graph_driver = 0;
	int graph_mode;
	int graph_error_code;

	int margin = 25;

	double legendxpos = margin;
	double legendypos = (int)(margin / 2 - 3);

	char buf[32];

	static const double PI = 3.1415926535;

	detectgraph(&graph_driver, &graph_mode);
	initgraph(&graph_driver, &graph_mode, "C:\\TURBOC3\\BGI");
	graph_error_code = graphresult();
	if(graph_error_code != grOk) {
		return 255;
	}

	line(margin, margin, margin, getmaxy() - margin);
	line(margin, getmaxy()/2, getmaxx() - margin, getmaxy()/2);
	outtextxy(getmaxx() - margin - 25, getmaxy()/2 - 20, "x,pi");
	line(margin, margin, margin-5, margin+8);
	line(margin, margin, margin+5, margin+8);
	line(getmaxx() - margin, getmaxy() / 2, getmaxx() - margin - 8, getmaxy() / 2 - 5);
	line(getmaxx() - margin, getmaxy() / 2, getmaxx() - margin - 8, getmaxy() / 2 + 5);

	double y;
	int y_mid = (int)(getmaxy() / 2);
	int plot_width = getmaxx() - 2 * margin;
	int y_scale = (getmaxy() - 2 * margin) / 20;

	double angle = 3*PI/2;
	double rad_per_px = 19.5 * PI / plot_width;

	int num = 0;
	for (double x = margin; x < plot_width; x += plot_width / 19.5) {
		line(x, y_mid + 3, x, y_mid - 3);
		sprintf(buf, "%d", num);
		outtextxy(x - 10, y_mid + 8, buf);
		num++;
	}

	for (double i = 1; i < 10; i++) {
		sprintf(buf, "%i", (int)i);
		line(margin - 3, y_mid - y_scale * i, margin + 3, y_mid - y_scale * i);
		outtextxy(margin - 8, y_mid - y_scale * i + 4, buf);
		sprintf(buf, "%i", (int)(-i));
		line(margin - 3, y_mid + y_scale * i, margin + 3, y_mid + y_scale * i);
		outtextxy(margin - 16, y_mid + y_scale * i + 4, buf);
	}
	outtextxy(margin - 16, margin - 6, "y");

	double y_max = 0;
	for (x = margin + angle / rad_per_px; x < plot_width - PI / rad_per_px; x += 1){
		y = y_mid - y_scale*(pow(cos(angle/2), 2) + sqrt(angle));
		putpixel(x, y, YELLOW);
		angle += rad_per_px;
		if (y > y_max) {
			y_max = y;
		}
	}

	setbkcolor(BLACK);

	int left = getmaxx() - margin - 250;
	int top = getmaxy() - margin - 100;
	int right = getmaxx() - margin - 50;
	int bottom = getmaxy() - margin - 50;
	setviewport(left, top, right, bottom, 0);
	setcolor(RED);
	rectangle(0, 0, right-left, bottom-top);
	setcolor(WHITE);
	outtextxy (5, 5, "max f(x) = ");
	sprintf(buf, "%f", y_max);
	outtextxy(94, 5, buf);
	getch();

	closegraph();
	return 0;
}
