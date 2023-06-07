clear; clc
clf;
n = 6;
a = -2;
b = 7;
x = a : 0.01 : b;
h = (b - a)/(n - 1);
Nodes = a : h : b;
labels = F(Nodes);
F = F(x);
poly_lagrange = get_poly_lagrange(Nodes, labels, x);
poly_newton = get_poly_newton_r(Nodes, labels, x, h);
error_pract = F - poly_lagrange;
plot(x, F, "red", x, poly_lagrange, "blue");
xlabel $x$;
ylabel $y$;
title (["Lagrange interpolation by " num2str(n) " equidistant nodes "]);
grid on;
figure
plot(x, F, "red", x, poly_newton, "yellow");
xlabel $x$;
ylabel $y$;
title (["Newton interpolation by " num2str(n) "equidistant nodes "]);
grid on;
figure
plot (x, error_pract, "green");
xlabel $x$;
ylabel $y$;
title ([" interpolation error on" num2str(n) " equidistan nodes"]);
grid on;
error_pract_n = max(abs(error_pract));
disp("Pract: ")
disp(error_pract_n);
error_theor_n = error_theor(n, x, Nodes);
disp("Theor: ")
disp(error_theor_n);