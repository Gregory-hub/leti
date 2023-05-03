function error = error_theor(n, x, nodes)
    error = 0;
    w = get_w(x, nodes);
    f = get_diff(n, x);
    error = 1 / factorial(n) * max(abs(f)) * max(abs(w));
end

function f = get_diff(n, x)
    f = zeros(1, length(x));
    syms t;
    dF = diff(F(t), n);
    for i = 1 : length(x)
        d = subs(dF, t, x(i));
        f(i) = double(d);
    end
end
function w = get_w(x, nodes)
    w = zeros(length(x), 1);
    for i = 1 : length(x)
        tmp = 1;
        for j = 1 : length(nodes)
            tmp = tmp * (x(i) - nodes(j));
        end
        w(i) = tmp;
    end
end