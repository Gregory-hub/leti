clear; clc
clf

a = -pi;
dx = 0.01;
b = pi;
m = 69;

x = a : dx: b;
Y = power(x, 2);
plot(x, Y, 'b')
grid on; hold on

xlabel $x$; ylabel $y$;
n = 1 : m;
X = diag(n) * ones(m, 1) * x;
A = power(-1,n) ./ power(n,2);
y = power(pi,2) / 3 + 4 * sum(diag(A)*cos(X),1);
plot(x, y,'r')
title(['$$ x^2 = \frac{\pi^2}{3} + 4 \sum\limits_{n = 1}^{' num2str(m) '} \frac {(-1)^n} {n^2} \cos nx    $$'])