from matplotlib.lines import lineStyles
from scipy import integrate
from math import pi, sin, cos
from matplotlib import pyplot as plt


def get_function(A: float, B: float):
	def fun(x):
		if 0 <= x <= 2:
			return B * (4 - x**2)
		elif -2 < x < 0:
			return A * (x + 2)
	return fun


def a0(fun) -> float:
	integral_res, error = integrate.quad(fun, -2, 2)
	return (1 / 2) * integral_res


def ak(fun, k: int) -> float:
	integral_res, error = integrate.quad(lambda x: fun(x)*cos(pi*k*x/2), -2, 2)
	return (1 / 2) * integral_res


def bk(fun, k: int) -> float:
	integral_res, error = integrate.quad(lambda x: fun(x)*sin(pi*k*x/2), -2, 2)
	return (1 / 2) * integral_res


def get_partial_sums_function(fun, n: int):
	def partial_sums(x):
		S = a0(fun) / 2
		for k in range(1, n + 1):
			S += ak(fun, k) * cos(k * x)
			S += bk(fun, k) * sin(k * x)
		return S
	return partial_sums


def standart_deviation(fun, n: int) -> float:
	integral_res, error = integrate.quad(lambda x: fun(x)**2, -2, 2)
	fun_norm_sq = (1 / 2) * integral_res
	part_sum_norm_sq = (a0(fun)**2 / 2)
	for k in range(1, n + 1):
		part_sum_norm_sq += ak(fun, k)**2
		part_sum_norm_sq += bk(fun, k)**2
	return (fun_norm_sq - part_sum_norm_sq)**(1/2)


A = 0.98
B = 0.74

fun = get_function(A, B)

grid_horizontal = ([-11, 11], [0, 0])
grid_vertical = ([0, 0], [-0.25, 3.25])
plt.plot(*grid_horizontal, color='gray', linestyle='--')
plt.plot(*grid_vertical, color='gray', linestyle='--')

x_arr = [0.02 * i for i in range(-499, 501)]

y_fun = [fun(x) for x in x_arr]
plt.plot(x_arr, y_fun, color='b', label="f(x)")

partial_sums = get_partial_sums_function(fun, 1)
y_partial_sums_1 = [partial_sums(x) for x in x_arr]
plt.plot(x_arr, y_partial_sums_1, color='g', label="S1(x)")

partial_sums = get_partial_sums_function(fun, 5)
y_partial_sums_5 = [partial_sums(x) for x in x_arr]
plt.plot(x_arr, y_partial_sums_5, color='y', label="S5(x)")

partial_sums = get_partial_sums_function(fun, 20)
y_partial_sums_20 = [partial_sums(x) for x in x_arr]
plt.plot(x_arr, y_partial_sums_20, color='r', label="S20(x)")

plt.legend()
plt.grid(linestyle=':')
plt.show()

print("D1 =", round(standart_deviation(fun, 1), 6))
print("D5 =", round(standart_deviation(fun, 5), 6))
print("D20 =", round(standart_deviation(fun, 20), 6))
