x0 = -2;
kk = [ ]; hh = [ ]; rr = [ ];
h = 1;
for k = 0 : 50
    r = ddf(x0) - ddf2(x0, h);
    rr = [rr, r];
    kk = [kk, k];
    hh = [hh, h];
    h = h / 2;
end
semilogy(kk, abs(rr) + 1e-14)
grid on
title('$ddf2$')
format short g
res22 = [kk', hh', rr']
