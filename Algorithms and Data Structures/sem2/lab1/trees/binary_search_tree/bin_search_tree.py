from trees.node import Node
from trees.bin_tree import BinTree


class BinSearchTree(BinTree):
	def __init__(self, elements: list):
		elements.sort()
		self.root = self.__tree_from_list(elements, len(elements))

	# public:
	def min(self) -> Node:
		# O(h)
		return self.__min(self.root)

	def max(self) -> Node:
		# O(h)
		return self.__max(self.root)

	def find(self, value: int) -> Node:
		# O(h)
		return self.__find(self.root, value)

	def next_el(self, node: Node) -> Node:
		# O(h)
		if node is None or not self.has_node(self.root, node):
			return None
		if node.right is not None:
			return self.__min(node.right)

		return self.__get_next_in_ancestors(self.root, node)

	def prev_el(self, node: Node) -> Node:
		# O(h)
		if node is None or not self.has_node(self.root, node):
			return None
		if node.left is not None:
			return self.__max(node.left)

		return self.__get_prev_in_ancestors(self.root, node)

	def has_node(self, root: Node, node: Node) -> bool:
		# O(h)
		if root is None:
			return False

		if node.value == root.value:
			if node == root:
				return True
			return self.has_node(root.left, node) or self.has_node(root.right, node)
		if node.value < root.value:
			return self.has_node(root.left, node)
		if node.value > root.value:
			return self.has_node(root.right, node)

	def insert(self, value: int):
		node = Node(value=value)
		parent = self.root
		if parent is None:
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
		return node

	# private:
	def __tree_from_list(self, elements: list, n: int) -> Node:
		# elements must be sorted
		# O(n)
		if n == 1:
			return Node(elements[0])
		if n == 0:
			return None

		root = Node(elements[n // 2])
		left = self.__tree_from_list(elements[:n // 2], n // 2)
		right = self.__tree_from_list(elements[n // 2 + 1:], n // 2 - 1 + n % 2)
		root.left = left
		root.right = right
		return root

	def __min(self, root: Node) -> Node:
		# O(h)
		if root is None or root.left is None:
			return root
		return self.__min(root.left)

	def __max(self, root: Node) -> Node:
		# O(h)
		if root is None or root.right is None:
			return root
		return self.__max(root.right)

	def __find(self, root: Node, value: int) -> Node:
		# O(h)
		if root is None:
			return None

		if value == root.value:
			return root
		if value < root.value:
			return self.__find(root.left, value)
		if value > root.value:
			return self.__find(root.right, value)
	
	def __get_next_in_ancestors(self, root: Node, node: Node) -> Node:
		# O(h)
		next_el = None
		while root and root.value != node.value:
			if root.value > node.value:
				next_el = root
				root = root.left
			elif root.value < node.value:
				root = root.right
		return next_el

	def __get_prev_in_ancestors(self, root: Node, node: Node) -> Node:
		# O(h)
		next_el = None
		while root and root.value != node.value:
			if root.value < node.value:
				next_el = root
				root = root.right
			elif root.value > node.value:
				root = root.left
		return next_el
