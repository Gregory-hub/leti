function [f_values] = F(x)
    f_values = 2000 ./ (x.^2 - 4.*x + 74);
end