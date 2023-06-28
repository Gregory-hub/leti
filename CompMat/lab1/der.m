a = -1; b = 7;
secs = 2;

t = a : (b - a) / 300 : b;
h = t(2) - t(1);
df = f(t);
df = diff(df) / h;
t(1) = [];

mder = [];
for n = 1 : 5
    df = diff(df) / h;
    t(1) = [];
    mder = [mder, max(abs(df))];
    plot(t, df, 'k')
    title(sprintf('f(%d)', n + 1))
    grid on
    pause(secs);
end
format short g
disp(['Наибольшие значения: ', num2str(mder)]);
