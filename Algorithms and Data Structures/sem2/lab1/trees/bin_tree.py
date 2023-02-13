from trees.node import Node


class BinTree:
	def __init__(self):
		self.root = None

	# public:
	def height(self) -> int:
		# O(n)
		return self.__get_height(self.root)

	def print_tree(self, detail: bool = False) -> None:
		# O(n)
		print(f"Tree of height {self.height()}:")
		self.__print_tree(self.root, detail=detail)
		print()

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

	def __get_height(self, root: Node) -> int:
		# O(n)
		if root is None:
			return -1
		if root.left is None and root.right is None:
			return 0
		
		height_l = self.__get_height(root.left)
		height_r = self.__get_height(root.right)
		return max(height_l, height_r) + 1
