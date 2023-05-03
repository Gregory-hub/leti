function poly_newton = get_poly_newton_r(nodes,f_labels,x, h)
table = get_table(nodes,f_labels);
for i = 1:length(nodes) - 1
    a(i) = table(1,i);
end

for k = 1:length(x)
    res = f_labels(1);
    for i = 2 : length(nodes)
        tmp = 1;
        for j = 1:i-1
            tmp = tmp*(x(k)-nodes(j));
        end
        tmp1 = 1;
        for j = 1 : i - 1
            tmp1 = tmp1 * h;
        end
        res = res + tmp*a(i - 1)/(tmp1 * factorial(i - 1));
    end
    poly_newton(k) = res;
    end
end

function table_div_diff = get_table(nodes,f_labels)
    table_div_diff = zeros(length(nodes) - 1);

    for i = 1:length(nodes) - 1
        table_div_diff(i,1) = f_labels(i + 1) - f_labels(i);
    end

    for i = 2 : length(nodes) - 1
        for j = 1 : length(nodes) - i
            table_div_diff(j,i) = table_div_diff(j + 1,i - 1) - table_div_diff(j,i - 1);
        end
    end
end