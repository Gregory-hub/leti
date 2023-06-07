function [pol, sigma_alignment_level] = sys(quasialternance_p, f_values)

m = length(quasialternance_p);  % m=n+2
matrix = zeros(m); 

for i = 1:m
    for k = 0:m - 2
        matrix(i, k + 1) = quasialternance_p (i) ^ (m - 2 - k);
    end
    matrix(i, m) = (-1) ^ i;
end

[i, k] = size(f_values);
if i < k, f_values = f_values'; end
pol = matrix \ f_values; 
sigma_alignment_level = pol(m); 
pol(m) = []; 
pol = pol';