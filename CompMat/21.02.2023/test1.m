% hamud hamud hababi hamud
clear, clc, clf
a = 0;
b = 13;
dx = 0.01;
x = a : dx : b;

y = log(x);
plot(x, y, 'bo-');

xlabel 't';
hold on;

y2 = cos(x);
plot(x, y2);
