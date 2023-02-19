from trees.node import Node
from trees.binary_search_tree.bin_search_tree import BinSearchTree
from trees.rotation_tree import RotationTree


class AVLTree(BinSearchTree, RotationTree):
	def balance(self, root: Node):
		if root is None:
			return 0
		left = self.height(root.left)
		right = self.height(root.right)
		return left - right 

	def insert(self, value: int):
		node = super().insert(value)	# bst insert
		self.fixup(node, self.root)

	def fixup(self, node: Node, current: Node):
		if current is None or current is node:
			return

		if self.has_node(current.left, node):
			self.fixup(node, current.left)
		else:
			self.fixup(node, current.right)

		if self.balance(current) == 2:
			if self.has_node(current.left.left, node):
				self.right_rotate(current)
			else:
				self.left_rotate(current.left)
				self.right_rotate(current)
		elif self.balance(current) == -2:
			if self.has_node(current.right.right, node):
				self.left_rotate(current)
			else:
				self.right_rotate(current.right)
				self.left_rotate(current)
	