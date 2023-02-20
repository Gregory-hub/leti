from random import randrange
from math import log2
from time import time_ns

from matplotlib import pyplot as plt

from trees.red_black_tree.red_black_tree import RedBlackTree
from trees.binary_search_tree.bin_search_tree import BinSearchTree
from trees.avl_tree.avl_tree import AVLTree


def get_height_distribution(tree, values: list) -> list:
	height_dist = []
	for value in values:
		tree.insert(value)
		height_dist.append(tree.height(tree.root))

	return height_dist


def get_fixup_time_distribution(tree, values: list, sample_frequency: int) -> list:
	time_dist = []
	i = 0
	st = time_ns()
	for value in values:
		tree.insert(value)
		if i == sample_frequency:
			time_dist.append(time_ns() - st)
			st = time_ns()
			i = 0
		i += 1

	return [i * sample_frequency for i in range(len(time_dist))], time_dist


def show_height_plots(n: int):
	x = list(range(n))
	values = [randrange(-1000, 1000) for _ in x]
	# values = x	# worst case - sorted list(all els go to the right)
	bst = BinSearchTree([])
	rbt = RedBlackTree([])
	avl = AVLTree([])

	bst_height_dist = get_height_distribution(bst, values)
	rbt_height_dist = get_height_distribution(rbt, values)
	avl_height_dist = get_height_distribution(avl, values)

	plt.plot(x, bst_height_dist, color='b', label="BST")
	plt.plot(x, rbt_height_dist, color='r', label="RBT")
	plt.plot(x, avl_height_dist, color='g', label="AVL")
	plt.plot(x, [0] + [log2(i) for i in x[1:]], color='black', label="log(n)")

	plt.title("Height")
	plt.legend()
	plt.show()


def show_fixup_time_plots(n):
	x = list(range(n))
	values = [randrange(-1000, 1000) for _ in x]
	# values = [0] * len(x)	# Worst case
	rbt = RedBlackTree([])
	avl = AVLTree([])

	x, rbt_time_dist = get_fixup_time_distribution(rbt, values, 400)
	x, avl_time_dist = get_fixup_time_distribution(avl, values, 400)

	plt.plot(x, rbt_time_dist, color='r', label="RBT")
	plt.plot(x, avl_time_dist, color='g', label="AVL")
	# plt.plot(x, [0] + [log2(i) for i in x[1:]], color='black', label="log(n)")

	plt.title("Fixup time")
	plt.legend()
	plt.show()


if __name__ == "__main__":
	show_height_plots(5000)
	# show_fixup_time_plots(5000)
