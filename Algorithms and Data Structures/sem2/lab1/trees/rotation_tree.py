from trees.bin_tree import BinTree
from trees.node import Node


class RotationTree(BinTree):
	def left_rotate(self, node: Node) -> None:
		# O(n)
		if node.right is None:
			return

		right = node.right
		node.right = right.left
		right.left = node

		parent = self.get_parent(node)	# O(n)
		if parent is None:
			self.root = right
		elif parent.left is node:
			parent.left = right
		elif parent.right is node:
			parent.right = right

	def right_rotate(self, node: Node) -> None:
		# O(n)
		if node.left is None:
			return

		left = node.left
		node.left = left.right
		left.right = node

		parent = self.get_parent(node)	# O(h)
		if parent is None:
			self.root = left
		elif parent.left is node:
			parent.left = left
		elif parent.right is node:
			parent.right = left

	def get_parent(self, node: Node) -> Node:
		# O(n)
		if node is None:
			return None
		return self.__get_parent(node, self.root)

	def __get_parent(self, node: Node, root: Node) -> Node:
		# O(n)
		if root == None:
			return None
		if root.right == node or root.left == node:
			return root

		if root.value < node.value:
			return self.__get_parent(node, root.right)
		elif root.value > node.value:
			return self.__get_parent(node, root.left)
		else:
			left = self.__get_parent(node, root.left)
			right = self.__get_parent(node, root.right)
			if left:
				return left
			if right:
				return right
