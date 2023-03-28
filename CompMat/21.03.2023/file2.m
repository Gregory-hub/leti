clear, clc
clf

N = 20;
E = zeros(N, 1);
E(1) = 1 / exp(1);

for n = 2 : N
    E(n) = 1 - n * E(n - 1);
end

for n = 1 : N
    fprintf('%d\t%11.7f\n', n, E(n));
end

E(N) = 0;
for n = N : -1 : 2
    E(n-1) = (1 - E(n)) / n;
end

for n = 1 : N
    fprintf('%d\t%11.7f\n', n, E(n));
end