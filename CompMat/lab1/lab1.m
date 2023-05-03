clear, clc, clf

% input
a = -1;
b = 7;
number_of_nodes = 9;

dx = 9 / number_of_nodes;
x = a : dx : b;
y = arrayfun(@(x) f(x), x);

x_precise = a : .1 : b;
y_inplot = arrayfun(@(x) f(x), x_precise);

tiledlayout(2, 1)
% input plot
nexttile

scatter(x, y, 'r', 'filled');
hold on;

plot(x_precise, y_inplot, 'r');
hold off;

grid on;
xlabel $x$, ylabel $y$;
title('$$ f(x) = \frac{1000} {x^2 - 5x + 72} $$')

% interpolation polynomial
p = polyfit(x, y, length(x));
y_new = polyval(p, x_precise);

% interpolated polynomial plot
nexttile
plot(x_precise, y_new, 'm');
grid on;
xlabel $x$, ylabel $y$;

tit = "$$ f(x) = ";
for i = 1 : length(p) - 1
    if p(i) == 0
        continue
    end

    if i ~= 1
        tit = tit + " + ";
    end
    
    if length(p) - i ~= 1 
        pow = "^" + (length(p) - i);
    else
        pow = "";
    end
    
    tit = tit + num2str(p(i), 3) + "x" + pow;
end

p0 = p(length(p));
if p0 ~= 0
    p0 = " + " + num2str(p0, 3);
else
    p0 = "";
end

tit = tit + p0 + " $$";
title(tit)


function y = f(x)
    % input function
    y = 1000 / (x^2 - 5 * x + 72);
end
