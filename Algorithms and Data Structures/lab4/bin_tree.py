from node import Node


def is_number(val: str) -> bool:
	if len(val) < 1:
		return False
	if val[0] == '-':
		val = val[1:]

	return val.replace('.', '', 1).isdigit()


class BinTree:
	def __init__(self, expr: str):
		temp_expr = ''
		for sym in expr:
			if sym in ['(', ')', '*', '/', '+', '-']:
				temp_expr = temp_expr + ' ' + sym + ' '
			else:
				temp_expr = temp_expr + sym
		expr = list(temp_expr.split())

		if expr[0] in ['+', '-', '*', '/']:
			self.root = self.tree_from_prefix(expr)
		elif expr[-1] in ['+', '-', '*', '/']:
			self.root = self.tree_from_prefix(expr[::-1])
		elif is_number(expr[0]) or expr[0] in ['(', ')']:
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
		if len(expr) < 3 or len(expr) == 1 and is_number(expr[0]):
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
		if len(expr) < 1:
			return None
		if not is_number(expr[0]) and not expr[0] in ['(', ')']:
			raise ValueError(f"Invalid expression: {expr}")

		# ( 5 - ( 3 - 1 ) * 2 )
		if len(expr) == 1 and is_number(expr[0]):
			root = Node(expr[0])

		elif len(expr) == 2 and is_number(expr[0]):
			raise ValueError(f"Invalid expression: {expr}")

		elif len(expr) == 3 and is_number(expr[0]) and expr[1] in ['+', '-', '*', '/'] and is_number(expr[2]):
			root = Node(expr.pop(1))
			root.left = Node(expr.pop(0))
			root.right = Node(expr.pop(0))

		elif is_number(expr[0]) and expr[1] in ['+', '-', '*', '/'] and expr[2] == '(':
			if ')' not in expr:
				raise ValueError(f"Invalid expression: {expr}")

			closing_bracket_i = self.__find_closing_bracket(expr, 2)
			if closing_bracket_i < 4:	# including -1
				raise ValueError(f"Invalid expression: {expr}")

			inner_expr = expr[3:closing_bracket_i]
			further_expr = []
			if len(expr) > closing_bracket_i + 1:
				further_expr = expr[closing_bracket_i + 1:]

			if len(further_expr) == 0:
				root = Node(expr.pop(1))
				root.left = Node(expr.pop(0))
				root.right = self.tree_from_infix(inner_expr)

			elif further_expr[0] in ['+', '-', '*', '/']:
				root = Node(further_expr.pop(0))
				root.left = self.tree_from_infix(expr[:closing_bracket_i + 1])
				root.right = self.tree_from_infix(expr[closing_bracket_i + 1:])

			else:
				raise ValueError(f"Invalid expression: {expr}")

		elif expr[0] == '(':
			if ')' not in expr:
				raise ValueError(f"Invalid expression: {expr}")

			closing_bracket_i = self.__find_closing_bracket(expr, 0)
			if closing_bracket_i < 2 or closing_bracket_i == 3:	# including -1
				raise ValueError(f"Invalid expression: {expr}")

			inner_expr = expr[1:closing_bracket_i]
			further_expr = []
			if len(expr) > closing_bracket_i + 1:
				further_expr = expr[closing_bracket_i + 1:]

			if len(further_expr) == 0:
				root = self.tree_from_infix(inner_expr)

			elif further_expr[0] in ['+', '-', '*', '/']:
				root = Node(further_expr.pop(0))
				root.left = self.tree_from_infix(inner_expr)
				root.right = self.tree_from_infix(expr[closing_bracket_i + 2:])

			else:
				raise ValueError(f"Invalid expression: {expr}")
		else:
			raise ValueError(f"Invalid expression: {expr}")

		return root

	def print_tree(self, detail: bool = False) -> None:
		# O(n)
		print(f"Tree of height {self.height()}:")
		self.__print_tree(self.root, detail=detail)
		print()
	
	def get_expression(self, notation: str):
		# O(n)
		if notation == "prefix":
			expr = self.__get_prefix_expression(self.root)
		elif notation == "postfix":
			expr = self.__get_prefix_expression(self.root)
			expr = [' ' + val.strip() for val in expr][::-1]
			expr[0].strip()
		elif notation == "infix":
			expr = self.__get_infix_expression(self.root)[1:-1]
		else:
			raise ValueError("Invalid notation")

		return ''.join(expr).strip()

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

	def __get_infix_expression(self, root: Node, expr: str = '') -> list:
		# O(n)
		if root is None:
			return []

		expr = []
		left = self.__get_infix_expression(root.left)
		right = self.__get_infix_expression(root.right)
		l_bracket = ''
		r_bracket = ''
		if left != [] or right != []:
			l_bracket = '('
			r_bracket = ')'
		if root.value in ['*', '/', '+', '-']:
			root.value = ' ' + root.value + ' '
		expr = [l_bracket] + left + [root.value] + right + [r_bracket]

		return expr

	def __get_prefix_expression(self, root: Node, expr: str = '') -> list:
		# O(n)
		if root is None:
			return []

		expr = []
		left = self.__get_prefix_expression(root.left)
		right = self.__get_prefix_expression(root.right)
		for s in root.value:
			if s in ['*', '/', '+', '-']:
				root.value = root.value.strip()

		root.value = ' ' + root.value
		expr = [root.value] + left + right

		return expr

	def __get_height(self, root: Node) -> int:
		# O(n)
		if root is None:
			return -1
		if root.left is None and root.right is None:
			return 0
		
		height_l = self.__get_height(root.left)
		height_r = self.__get_height(root.right)
		return max(height_l, height_r) + 1

	def __find_closing_bracket(self, expr: list, opening_br_index: int) -> int:
		# O(n)
		if expr[opening_br_index] != '(':
			raise ValueError("expr[opening_br_index] is not '('")

		level = 1
		i = 0
		while i != opening_br_index:
			if expr[i] == '(':
				level += 1
			i += 1

		i += 1
		while i < len(expr) and level != 0:
			if expr[i] == '(':
				level += 1
			elif expr[i] == ')':
				level -= 1
			i += 1

		if level != 0:
			return -1

		return i - 1


# for infix notation please use brackets for ordering
expression = "+ - 4 13 * 6 - + 11 2 8"
tree = BinTree(expression)
tree.print_tree()
print("Expression:", expression)
print("Infix:", tree.get_expression(notation="infix"))
print("Prefix:", tree.get_expression(notation="prefix"))
print("Postfix:", tree.get_expression(notation="postfix"))
print()

expression = "8 2 11 + - 6 * 13 4 - +"
print("Expression:", expression)
tree = BinTree(expression)
print("Infix:", tree.get_expression(notation="infix"))
print("Prefix:", tree.get_expression(notation="prefix"))
print("Postfix:", tree.get_expression(notation="postfix"))
print()

expression = "(4 - 13) + (6 * ((11 + 2) - 8))"
print("Expression:", expression)
tree = BinTree(expression)
print("Infix:", tree.get_expression(notation="infix"))
print("Prefix:", tree.get_expression(notation="prefix"))
print("Postfix:", tree.get_expression(notation="postfix"))
print()
