from math import log2
from collections import deque

from bin_tree import BinSearchTree, Node


class Test_BST:
	def test_height(self):
		print("testing height...", end='')
		lists = [
			[[j for j in range(i)] for i in range(100)]
			]

		for li in lists:
			tree = BinSearchTree(li)
			assert tree.height() == int(log2(len(li))), f"Error: list {[i for i in li]}, height {int(log2(len(li)))}"
		print("OK")

	def test_min(self):
		print("testing min...", end='')
		li = list(range(-100, 100))
		for i in range(len(li)):
			li[i] = -101
			tree = BinSearchTree(li)
			assert tree.min().value == -101, f"Tree min is not min(-101, {tree.min()}"
			li = list(range(-100, 100))
		print("OK")

	def test_max(self):
		print("testing max...", end='')
		li = list(range(-100, 100))
		for i in range(len(li)):
			li[i] = 101
			tree = BinSearchTree(li)
			assert tree.max().value == 101, f"Tree max is not max(101, {tree.max()}"
			li = list(range(-100, 100))
		print("OK")

	def test_find(self):
		print("testing find...", end='')
		li = list(range(-100, 100))
		for i in range(len(li)):
			tree = BinSearchTree(li)
			assert tree.find(li[i]).value == li[i], f"Find did not find {li[i]}"
			li = list(range(-100, 100))
		print("OK")

	def test_next_el(self):
		print("testing next_el...", end='')
		li = list(range(-100, 100))
		for i in range(len(li) - 1):
			tree = BinSearchTree(li)
			el = tree.find(li[i])
			assert tree.next_el(el).value == li[i + 1], f"Next el is not next (current: {el}, expected value: {li[i + 1]}, got: {tree.next_el(el)}"
			li = list(range(-100, 100))
		el = tree.find(li[-1])
		assert tree.next_el(el) == None,  f"Next el is not next (current: {el}, expected value: {None}, got: {tree.next_el(el)}"
		print("OK")

	def test_prev_el(self):
		print("testing prev_el...", end='')
		li = list(range(-100, 100))
		for i in range(1, len(li)):
			tree = BinSearchTree(li)
			el = tree.find(li[i])
			assert tree.prev_el(el).value == li[i - 1], f"Next el is not next (current: {el}, expected value: {li[i - 1]}, got: {tree.prev_el(el)}"
			li = list(range(-100, 100))
		el = tree.find(li[0])
		assert tree.prev_el(el) == None,  f"Next el is not next (current: {el}, expected value: {None}, got: {tree.prev_el(el)}"
		print("OK")

	def test_get_parent(self):
		print("testing get_parent...", end='')
		li = list(range(-100, 100))
		tree = BinSearchTree(li)

		stack = deque()
		stack.append(tree.root)
		while len(stack) != 0:
			current = stack.pop()
			if current.left:
				stack.append(current.left)
				assert tree.get_parent(current.left) == current, f"Parent is not parent({tree.get_parent(current.left)}, {current})"
			if current.right:
				stack.append(current.right)
				assert tree.get_parent(current.right) == current, f"Parent is not parent({tree.get_parent(current.right)}, {current})"

		print("OK")

	def test_has_node(self):
		print("testing has_node...", end='')
		li = list(range(-100, 100))
		tree = BinSearchTree(li)

		stack = deque()
		stack.append(tree.root)
		while len(stack) != 0:
			current = stack.pop()
			assert tree.has_node(current) == True, f"Says it doesn't have node while it have({tree.has_node(current)}, {current})"

			if current.left:
				stack.append(current.left)
			if current.right:
				stack.append(current.right)

		print("OK")

	def test_delete(self):
		print("testing delete...", end='')
		li = list(range(-100, 100))
		tree = BinSearchTree(li)

		stack = deque()
		stack.append(tree.root)
		while len(stack) != 0:
			temp_tree = BinSearchTree(li)
			current = stack.pop()
			temp_tree.delete(current)
			assert temp_tree.has_node(current) == False, f"Did not delete({tree.has_node(current)}, {current})"

			for i in range(len(li)):
				if li[i] != current.value:
					assert temp_tree.find(li[i]) is not None, f"delete() deletes what it must not({li[i]})"

			if current.left:
				stack.append(current.left)
			if current.right:
				stack.append(current.right)

		print("OK")


if __name__ == "__main__":
	tree_tester = Test_BST()
	tree_tester.test_height()
	tree_tester.test_min()
	tree_tester.test_max()
	tree_tester.test_find()
	tree_tester.test_next_el()
	tree_tester.test_prev_el()
	tree_tester.test_get_parent()
	tree_tester.test_has_node()
	tree_tester.test_delete()
