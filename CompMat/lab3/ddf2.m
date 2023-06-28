function [y] = ddf2(x, h)
    y = (f(x + h) - 2 * f(x) + f(x - h)) / (h^2);
end
