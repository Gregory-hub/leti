class Node:
	def __init__(self, value, is_black: bool = False):
		self.value = value
		self.left = None
		self.right = None
		self.is_black = is_black

	def __repr__(self):
		return f"({self.value}, {'b' if self.is_black else 'r'})"
		# return f"<Node({self.value}, {'b' if self.is_black else 'r'}) at {hex(id(self))}>"
