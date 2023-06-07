clear; clc

a = -2;
b = 7;
n = 10;

quasialternance_p = a:(b-a) / (n+1):b; 
gr_arr = a:(b-a) / 1000:b; 
res = []; 
step = 0; 
format short g

while true
    [pol, sigma_alignment_level] = sys(quasialternance_p, F(quasialternance_p));

    error = F(quasialternance_p) - polyval(pol, quasialternance_p);
    error_arr = F(gr_arr) - polyval(pol, gr_arr); 
    [err_max, max_point_num] = max(abs(error_arr)); 
    point_max = gr_arr(max_point_num); 
    err_max = error_arr(max_point_num); 
    accuracy = 1 - abs(sigma_alignment_level / err_max); 
    after = sum(quasialternance_p < point_max); 
    step = step + 1;
    if step == 1 | step == 2 | step == 3 | step == 4 | step == 5 | step == 7
        figure
    end
    plot(gr_arr, error_arr, 'k', ... 
        quasialternance_p, error, 'ko', quasialternance_p, quasialternance_p*0, 'x', ... 
            point_max, err_max, '*', point_max, 0, 'x') 
    grid on
    title(['$' num2str(step) ...
        '\hspace{3mm} \sigma = ' num2str(abs(sigma_alignment_level)) ...
            '\hspace{3mm}  \max |f(x) - p(x)| =' num2str(abs(err_max)) ...
                '\hspace{3mm}  \varepsilon =' num2str(accuracy) '$'])
    %title(sprintf('%d: n sigma =%g, err_max=%g, accuracy=%g (i> %d)', ...
        %step, abs(sigma_alignment_level), abs(err_max), accuracy, after))

    res = [res; [step, sigma_alignment_level, err_max, accuracy, after]];

    %after
    err = [0, error, 0];
    %disp('ra(after), err_max, ra(after+1)')
    %[err(after+1), err_max, err(after+2)]
    if err(after + 1) ~= 0 & err(after + 1)/err_max > 0
        quasialternance_p(after) = point_max;
    elseif err(after + 2) ~= 0 & err(after + 2)/err_max > 0
        quasialternance_p(after + 1) = point_max;
    elseif err(after + 1) == 0 & error(1)/err_max < 0
        quasialternance_p = [point_max,quasialternance_p];
        quasialternance_p(length(quasialternance_p)) = [];
    elseif err(after + 2) == 0 & error(length(error))/err_max < 0
        quasialternance_p = [quasialternance_p, point_max];
        quasialternance_p(1) = [];
    end
    if abs(sigma_alignment_level - abs(err_max)) <= 10^(-12)
        break;
    end
end
format long g
pol
format short g
res
