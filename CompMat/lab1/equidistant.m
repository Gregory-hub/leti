disp('Equidistant')

a = -1; b = 7;
secs = 2;
t = a : (b - a) / 1000 : b;

err1 = [];
format long g
for n = 1 : 5   % степень полинома = кол-во точек - 1
    x = a : (b - a) / n : b;
    p = polyfit(x, f(x), n);
    disp(['Коэффициенты полинома L', num2str(n), ': ', num2str(p)]);
    y = polyval(p, t);
    err1 = [err1, max(abs(f(t) - y))];
    % голубой - функция, красный - интерполяционный многочлен
    plot(t, f(t), 'b', t, y, 'r', x, f(x), 'ko')
    title(['Equidistant L', num2str(n)])
    grid on
    pause(secs);
    plot(t, f(t) - y, 'b', x, x * 0, 'ko')
    title(['Err1: n = ', num2str(n)])
    grid on
    pause(secs);
end

format short g
disp(['Погрешность: ', num2str(err1)]);
