class Node:
	def __init__(self, value: int):
		self.value = value
		self.left = None
		self.right = None

	def __repr__(self):
		return f"<Node({self.value})>"


class BinSearchTree:
	def __init__(self, elements: list):
		elements.sort()
		self.root = self.__tree_from_list(elements, len(elements))

	# public:
	def height(self):
		# O(n)
		return self.__get_height(self.root)

	def print_tree(self):
		# O(n)
		print(f"Tree of height {self.height()}:")
		self.__print_tree(self.root)
		print()

	def min(self):
		# O(h)
		return self.__find_min(self.root)

	def max(self):
		# O(h)
		return self.__find_max(self.root)

	def get_nodes_inorder(self):
		# O(n)
		return self.__traversal_inorder(self.root)

	# private:
	def __get_height(self, root: Node):
		# O(n)
		if root is None:
			return 0
		if root.left is None and root.right is None:
			return 0
		
		height_l = self.__get_height(root.left)
		height_r = self.__get_height(root.right)
		return max(height_l, height_r) + 1

	def __tree_from_list(self, elements: list, n: int):
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

	def __print_tree(self, root: Node, level: int = 0, child_letter = ''):
		# O(n)
		if root is None:
			return

		child_str = child_letter + ':' if child_letter != '' else ''
		print(". ", ". " * (level - 1), child_str, root.value, sep='')
		self.__print_tree(root.left, level + 1, 'l')
		self.__print_tree(root.right, level + 1, 'r')

	def __find_min(self, root: Node):
		# O(h)
		if root.left is None:
			return root
		return self.__find_min(root.left)

	def __find_max(self, root: Node):
		# O(h)
		if root.right is None:
			return root
		return self.__find_max(root.right)

	def __traversal_inorder(self, root: Node):
		# O(n)
		if root is None:
			return []
		
		left = self.__traversal_inorder(root.left)
		right = self.__traversal_inorder(root.right)

		return left + right + [root]


li = [0, 1, -39, -212, 57, 22, -190, 30, -41]
tree = BinSearchTree(li)
tree.print_tree()
print(tree.min().value)
print(tree.max().value)
print(tree.get_nodes_inorder())
