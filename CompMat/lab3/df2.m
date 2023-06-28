function [y] = df2(x, h)
    y = (f(x + h) - f(x - h)) / (2 * h);
end
