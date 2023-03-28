clear, clc
clf

a = 2;
b = 3;

T1 = 80;
dx = 0.01;
dy = 0.01;
m = 5;

x = 0 : dx : a;
y = 0 : dy : b;
n = 1:m;

[X, Y, N] = meshgrid(x, y, n);

mtt1 = 2 * N - 1;
mtt2 = pi * mtt1 / a;

T = (4 * T1 / pi) * ((sin(mtt2 .* X) .* sinh(mtt2 .* Y)) ...
    ./ (mtt1 .* sinh(mtt1 * b)));


mesh(X(:, :, 1), Y(:, :, 1), sum(T, 3));
xlabel $x$; ylabel $y$
title(['$$ T((x, y) = \frac{4t}{\pi} \sum\limits_{n=1}^{' num2str(m) '} \frac{\sin \left[(2n-1) \frac{\pi x}{a} \right]}{2n-1} $$']);