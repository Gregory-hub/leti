clear; clc;
N = 365;
Q = ones(N,1);
for i = 2 : N
    Q(i) = Q(i-1)*(1-(i-1)/N);
end
P = 1 - Q;
plot(1:N,P,'k.')
grid on