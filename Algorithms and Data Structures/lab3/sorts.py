from matplotlib import pyplot as plt
from time import time
from random import randint


def insertion_sort(li: list, n: int) -> list:
	# Worst: n^2
	# Average: n^2
	# Best: n
	# Memory: 1
	for i in range(1, n):
		el = li[i]
		j = i - 1
		while j >= 0 and li[j] > el:
			li[j + 1] = li[j]
			j -= 1
		li[j + 1] = el
	return li


def selection_sort(li: list, n: int) -> list:
	# Worst: n^2
	# Average: n^2
	# Best: n^2
	# Memory: 1
	for i in range(n):
		min_el_i = i
		for j in range(i + 1, n):
			if li[j] < li[min_el_i]:
				min_el_i = j
		li[i], li[min_el_i] = li[min_el_i], li[i]
	return li
	

def bubble_sort(li: list, n: int) -> list:
	# Optimized
	# Worst: n^2
	# Average: n^2
	# Best: n
	# Memory: 1
	for i in range(n - 1):
		swapped = False
		for j in range(n - 1 - i):
			if li[j] > li[j + 1]:
				li[j], li[j + 1] = li[j + 1], li[j]
				swapped = True
		if not swapped:
			break
	return li


def merge(left: list, left_len: int, right: list, right_len: int):
	# O(n)
	# Memory: n
	res_li = []
	r, l = 0, 0
	for i in range(left_len + right_len):
		if l == left_len:
			res_li.extend(right[r:])
			break
		elif r == right_len:
			res_li.extend(left[l:])
			break
		elif left[l] < right[r]:
			res_li.append(left[l])
			l += 1
		else:
			res_li.append(right[r])
			r += 1
	return res_li


def merge_sort(li: list, n: int) -> list:
	# Worst: nlogn
	# Average: nlogn
	# Best: nlogn
	# Memory: n
	if n == 1:
		return li
	mid_i = n // 2
	left = merge_sort(li[:mid_i], mid_i)
	right = merge_sort(li[mid_i:], n - mid_i)
	li = merge(left, mid_i, right, n - mid_i)
	return li


def shell_sort(li: list, n: int) -> list:
	# Worst: n^2
	# Average: nlogn
	# Best: nlogn
	# Memory: 1
	gap = n // 2 
	while gap > 0:
		for i in range(gap, n):
			el = li[i]
			j = i - gap
			while j >= 0 and li[j] > el:
				li[j + gap] = li[j]
				j -= gap
			li[j + gap] = el
		gap //= 2

	return li


def partition(li: list, n: int) -> int:
	# O(n)
	# Memory: 1
	pivot = li[-1]
	wall = 0		# left to the wall - els <= pivot, right(or equal) to the wall - > pivot
	for i in range(n - 1):
		if li[i] <= pivot:
			li[i], li[wall] = li[wall], li[i]
			wall += 1
	li[-1], li[wall] = li[wall], li[-1]

	return li, wall


def quick_sort(li: list, n: int) -> list:
	# Worst: n^2
	# Average: nlogn
	# Best: nlogn
	# Memory: logn
	if n <= 0:
		return li

	li, wall = partition(li, n)
	li = quick_sort(li[:wall], wall) + [li[wall]] + quick_sort(li[wall+1:], n - wall - 1)
	return li


def build_plots(f, f_name = "") -> None:			# f is a function
	n = range(2, 1*10**4, 1000)

	y_best = []
	for i in n:
		li = [x - i // 2 for x in range(i)]
		start = time()
		f(li, i)
		end = time()
		y_best.append(round((end - start) * 1000, 6))
	plt.plot(n, y_best, color='y', label=f_name+' best case')

	y_aver = []
	for i in n:
		li = [randint(-1000, 1000) for x in range(i)]
		start = time()
		f(li, i)
		end = time()
		y_aver.append((end - start) * 1000)
	plt.plot(n, y_aver, color='b', label=f_name+' average case')

	y_worst = []
	for i in n:
		li = [x for x in range(i, 0, -1)]
		start = time()
		f(li, i)
		end = time()
		y_worst.append((end - start) * 1000)
	plt.plot(n, y_worst, color='r', label=f_name+' worst case')

	plt.legend()
	plt.show()
	

li = [8, 7, 6, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5, -6, -7]
print("Insertion:  ", insertion_sort(li, len(li)))
print("Selection:  ", selection_sort(li, len(li)))
print("Bubble: ", " " * 3, bubble_sort(li, len(li)))
print("Merge: ", " " * 4, merge_sort(li, len(li)))
print("Shell: ", " " * 4, shell_sort(li, len(li)))
print("Quick: ", " " * 4, quick_sort(li, len(li)))
print("Python sort:", sorted(li))
# build_plots(insertion_sort, "Inisertion sort")
# build_plots(selection_sort, "Selection sort")
# build_plots(bubble_sort, "Bubble sort")
# build_plots(merge_sort, "Merge sort")
# build_plots(shell_sort, "Shell sort")
# build_plots(quick_sort, "Quick sort")
# build_plots(sorted, "Python sort")
 