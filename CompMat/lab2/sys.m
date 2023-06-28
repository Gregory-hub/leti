function [p, sigma] = sys(t, y)
% выравнивает квазиальтернанс
% p - коэффициенты многочлена, sigma - значение уровня выравнивания

m = length(t);
a = zeros(m);

for i = 1 : m
    for k = 0 : m - 2
        a(i, k + 1) = t(i)^(m - 2 - k);
    end
    a(i, m) = (-1)^i;
end

[i, k] = size(y);
if i < k, y = y'; end
p = a \ y;
sigma = p(m);
p(m) = [];
p = p';
