a = -1; b = 7;
secs = 2;

t = a : (b - a) / 1000 : b;
mom1 = [];
for n = 1 : 5
    disp(['Количество узлов: ', num2str(n + 1)]);
    x = a : (b - a) / n : b;
    p = poly(x);
    y = polyval(p,t);
    mom1 = [mom1, max(abs(y))];
    disp(['Максимальное значение: ', num2str(max(abs(y)))]);
    plot(t, y, 'b', x, x*0, 'ko')
    title(['Omega: n=', num2str(n)])
    grid on
    pause(secs);
end

format short g
disp(['Теоретическая ошибка: ', num2str(mom1)]);
