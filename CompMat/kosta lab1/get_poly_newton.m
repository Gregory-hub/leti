function poly_newton = get_poly_newton(nodes,f_labels,x)
    table = get_table(nodes,f_labels);
    for i = 1:length(nodes)
        a(i) = table(1,i);
    end

    %P_n(x) = f_0 + f(x_0;x_1)(x-x_0) + f2(x_0;x_1;x_2)(x-x_0)(x-x_1) + ... + f(x_0;...;x_n)(x-x_0)(x-x_1)...(x-x_(n-1))
    for k = 1:length(x)
        res = 0;
        for i = 1:length(nodes)
            tmp = 1;
            for j = 1:i-1
                tmp = tmp*(x(k)-nodes(j));
            end
            res = res + tmp*a(i);
        end
        poly_newton(k) = res;
    end
end

function table_div_diff = get_table(nodes,f_labels)
    table_div_diff = zeros(length(nodes));

    for i = 1 : length(nodes)
        table_div_diff(i,1) = f_labels(i);
    end

    for k = 2 : length(nodes)
        for j = 1 : length(nodes) - k + 1
            table_div_diff(j, k) = (table_div_diff(j + 1, k - 1) - table_div_diff(j, k - 1)) / (nodes(j + k - 1) - nodes(j));
        end
    end
end