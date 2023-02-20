from trees.binary_search_tree.bin_search_tree import BinSearchTree
from trees.red_black_tree.red_black_node import RedBlackNode
from trees.rotation_tree import RotationTree


class RedBlackTree(BinSearchTree, RotationTree):
	def __init__(self, elements: list):
		self.root = None
		for el in elements:
			self.insert(el)

	def insert(self, value) -> None:
		# O(n)
		node = RedBlackNode(value=value, is_black=False)
		parent = self.root
		if parent is None:
			node.is_black = True
			self.root = node
			return node

		while True:
			if parent.value > value:
				if parent.left is not None:
					parent = parent.left
				else:
					parent.left = node
					break
			else:
				if parent.right is not None:
					parent = parent.right
				else:
					parent.right = node
					break
		self.fixup(node)	# O(n)

		return node

	def fixup(self, curr: RedBlackNode):
		# O(n)
		parent = self.get_parent(curr)	# O(n)	~Theta(h)
		while parent is not None and not parent.is_black:
			if parent is None:
				curr.is_black = True
				self.root = curr
				return

			grandparent = self.get_parent(parent)
			if grandparent is None:
				parent.is_black = True
				return
			if grandparent.left is parent:
				uncle = grandparent.right
			if grandparent.right is parent:
				uncle = grandparent.left

			if uncle is not None and not uncle.is_black:
				grandparent.is_black = False
				parent.is_black = True
				uncle.is_black = True
				curr = grandparent
			else:
				if grandparent.right is parent:
					if parent.left is curr:
						self.right_rotate(parent)
						curr, parent = parent, curr
					self.left_rotate(grandparent)
					grandparent.is_black = False
					parent.is_black = True
				elif grandparent.left is parent:
					if parent.right is curr:
						self.left_rotate(parent)
						curr, parent = parent, curr
					self.right_rotate(grandparent)
					grandparent.is_black = False
					parent.is_black = True

			parent = self.get_parent(curr)
		self.root.is_black = True
