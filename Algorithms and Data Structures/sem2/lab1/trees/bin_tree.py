from queue import Queue

from trees.node import Node


class BinTree:
	def __init__(self):
		self.root = None

	# public:
	def height(self, root: Node) -> int:
		# O(n)
		if root is None:
			return 0
		if root.left is None and root.right is None:
			return 1
		
		height_l = self.height(root.left)
		height_r = self.height(root.right)
		return max(height_l, height_r) + 1

	def print_tree(self, detail: bool = False) -> None:
		# O(n)
		print(f"Tree of height {self.height(self.root)}:")
		self.__print_tree(self.root, detail=detail)
		print()

	def get_nodes_inorder(self) -> list:
		# O(n)
		return self.__get_nodes_inorder(self.root)

	def get_nodes_preorder(self) -> list:
		# O(n)
		return self.__get_nodes_preorder(self.root)

	def get_nodes_postorder(self) -> list:
		# O(n)
		return self.__get_nodes_postorder(self.root)

	def get_nodes_breadth_first(self) -> list:
		# O(n)
		if self.root is None:
			return []

		nodes = []
		queue = Queue()

		current = self.root
		queue.put(current)
		while not queue.empty():
			current = queue.get()
			nodes.append(current)
			if current.left:
				queue.put(current.left)
			if current.right:
				queue.put(current.right)

		return nodes

	def insert(self, el: Node): ...
	
	# private
	def __print_tree(self, root: Node, level: int = 0, child_letter = '', detail: bool = False) -> None:
		# O(n)
		if root is None:
			return

		child_str = child_letter + ':' if child_letter != '' else ''
		if detail:
			print(". ", ". " * (level - 1), child_str, root, sep='')
		else:
			print(". ", ". " * (level - 1), child_str, root.value, sep='')
		self.__print_tree(root.left, level + 1, 'l', detail)
		self.__print_tree(root.right, level + 1, 'r', detail)

	def __get_nodes_inorder(self, root: Node) -> list:
		# O(n)
		if root is None:
			return []

		left = self.__get_nodes_inorder(root.left)
		right = self.__get_nodes_inorder(root.right)

		return left + [root] + right 

	def __get_nodes_preorder(self, root: Node) -> list:
		# O(n)
		if root is None:
			return []

		left = self.__get_nodes_preorder(root.left)
		right = self.__get_nodes_preorder(root.right)

		return [root] + left + right 

	def __get_nodes_postorder(self, root: Node) -> list:
		# O(n)
		if root is None:
			return []
		
		left = self.__get_nodes_postorder(root.left)
		right = self.__get_nodes_postorder(root.right)

		return left + right + [root]
