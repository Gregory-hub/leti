function [y] = ddf4(x, h)
    y = ((-f(x + 2 * h)) + 16 * f(x + h) - 30 * f(x) + 16 * f(x - h) - f(x - 2 * h)) / (12 * h^2);
end
