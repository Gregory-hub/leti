a = -1;
b = 7;
n = 5;

t = a : (b - a) / (n + 1) : b;
tt = a : (b - a) / 1000 : b;
step = 0;
format short g

while step < 20
    [p, sigma] = sys(t, f(t));
    
    ra = f(t) - polyval(p, t);
    r = f(tt) - polyval(p, tt);
    [rmax, i] = max(abs(r)); %#ok<ASGLU>
    tmax = tt(i);
    rmax = r(i);
    
    acc = 1 - abs(sigma / rmax);
    after = sum(t < tmax);
    step = step + 1;
    
    plot(tt, r, 'k', ...
        t, ra, 'ko', tmax, rmax, 'r*', tmax, 0, 'x')
    grid on
    title([num2str(step), ' $$: \sigma = $$ ', num2str(abs(sigma)),...
        ' $$, r_{max} = $$ ' num2str(abs(rmax)),...
        ', acc = ', num2str(acc), ' $$ (i > $$ ', num2str(after), ')'])
    
    [step, sigma, rmax, acc, after]
    ra0 = [0, ra, 0];
    
    if ra0(after + 1) == 0
        t = [tmax, t];
        t(end) = [];
    end
    
    if ra0(after + 2) == 0
        t = [t, tmax];
        t(1) = [];
    end
    
    if ra0(after + 1) * rmax < 0
        t(after + 1) = tmax;
    else
        t(after + 2) = tmax;
    end
    pause(0.1);
end

