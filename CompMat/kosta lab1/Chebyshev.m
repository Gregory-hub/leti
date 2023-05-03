clear; clc
clf;
n = 7;
a = -2;
b = 7;
x = a : 0.01 : b;
Nodes = zeros();
for k = 1 : n
    Nodes(k) = 0.5 * (a + b) + 0.5 * (a - b)*cos((2 * k - 1) / (2 * n) * pi);
end
labels = F(Nodes);
F = F(x);
poly_lagrange = get_poly_lagrange(Nodes, labels, x);
poly_newton = get_poly_newton(Nodes, labels, x);
error_pract = F - poly_newton;
plot(x, F, "red", x, poly_lagrange, "blue");
xlabel $x$;
ylabel $y$;
title (["Lagrange interpolation by " num2str(n) "Chebyshev nodes "]);
grid on;
figure
plot(x, F, "red", x, poly_newton, "yellow");
xlabel $x$;
ylabel $y$;
title (["Newton interpolation by " num2str(n) "Chebyshev nodes "]);
grid on;
figure
plot (x, error_pract, "green");
xlabel $x$;
ylabel $y$;
title (["interpolation error on " num2str(n) "Chebyshev nodes"]);
grid on;
error_pract_n = max(abs(error_pract));
error_theor = error_theor(n, x, Nodes);
error_theor_n = max(abs(error_theor));
disp(error_pract_n);
disp(error_theor_n);