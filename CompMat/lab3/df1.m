function [y] = df1(x, h)
    y = (f(x + h) - f(x)) / h;
end