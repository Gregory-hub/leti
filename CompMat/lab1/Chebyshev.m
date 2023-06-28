disp('Chebyshev');

a = -1; b = 7;
secs = 1;
t = a : (b - a) / 1000 : b;

err2 = [];
format long g
for n = 1 : 5   % степень полинома = кол-во точек - 1
    k = 0 : n;
    z = cos((pi + 2 * pi * k)/(2 * n + 2));
    x = (a + b) / 2 - z * (b - a) / 2;
    p = polyfit(x, f(x), n);
    disp(['Коэффициенты полинома L', num2str(n), ': ', num2str(p)]);
    y = polyval(p, t);
    err2 = [err2, max(abs(f(t) - y))];
    % голубой - функция, красный - интерполяционный многочлен
    plot(t, f(t), 'b', t, y, 'r', x, f(x), 'ko')
    title(['Chebychev L', num2str(n)])
    grid on
    pause(secs);
    plot(t, f(t) - y, 'b', x, x * 0, 'ko')
    title(['Err1: n = ', num2str(n)])
    grid on
    pause(secs);
end

format short g
disp(['Погрешность: ', num2str(err2)]);
