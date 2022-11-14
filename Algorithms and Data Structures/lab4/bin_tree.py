from node import Node


def is_number(val: str) -> bool:
	if len(val) < 1:
		return False
	if val[0] == '-':
		val = val[1:]

	return val.replace('.', '', 1).isdigit()


class BinTree:
	def __init__(self, expr: str):
		expr = list(expr.split())
	
		if expr[0] in ['+', '-', '*', '/']:
			self.root = self.tree_from_prefix(expr)
		elif expr[-1] in ['+', '-', '*', '/']:
			self.root = self.tree_from_prefix(expr[::-1])
		elif is_number(expr[0]):
			self.root = self.tree_from_infix(expr)
		else:
			raise ValueError("Invalid notation")

	# public:
	def height(self) -> int:
		# O(n)
		return self.__get_height(self.root)

	def tree_from_prefix(self, expr: list) -> Node:
		# O(n)
		# n - number of operands + number of numbers
		if len(expr) < 3:
			return None
		if expr[0] not in ['+', '-', '*', '/']:
			raise ValueError(f"Invalid expression: {expr}")

		root = Node(expr.pop(0))

		if is_number(expr[0]):
			root.left = Node(expr.pop(0))
		elif expr[0] in ['+', '-', '*', '/']:
			root.left = self.tree_from_prefix(expr)

		if is_number(expr[0]):
			root.right = Node(expr.pop(0))
		elif expr[0] in ['+', '-', '*', '/']:
			root.right = self.tree_from_prefix(expr)

		return root

	def tree_from_infix(self, expr: list) -> Node:
		# O(n)
		# n - number of operands + number of numbers
		if len(expr) < 3:
			return None
		if not is_number(expr[0]):
			raise ValueError(f"Invalid expression: {expr}")

		# return root

	def print_tree(self, detail: bool = False) -> None:
		# O(n)
		print(f"Tree of height {self.height()}:")
		self.__print_tree(self.root, detail=detail)
		print()

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

	# 		*+/421-52
	# 					*
	# 			+				-
	# 		/		1		5		2
	# 	4		2

tree = BinTree("3 / 2 + -3")
tree.print_tree()
