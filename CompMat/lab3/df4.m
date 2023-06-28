function [y] = df4(x, h)
    y = ((-f(x + 2 * h)) + 8 * f(x + h) - 8 * f(x - h) + f(x - 2 * h)) / (12 * h);
end