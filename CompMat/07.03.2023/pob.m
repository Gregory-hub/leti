function n = pob(N, alph)
q = 1;
for n = 2 : N
    q = q * (N-n+1)/N;
    if gt(1-q, alph)
        return 
    end
end
