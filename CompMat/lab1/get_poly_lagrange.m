function poly_lagrange = get_poly_lagrange(nodes, labels, x)
number = 1;
for k = 1 : length(x)
    res = 0;
    for i = 1:length(nodes)
        l_i = 1;
        for j = 1:length(nodes)
            if ~(i == j)
                l_i = l_i * (x(k) - nodes(j))/(nodes(i) - nodes(j));
            end
        end
        res = res + labels(i) * l_i;
    end
    poly_lagrange(number) = res;
    number = number + 1;
end
end