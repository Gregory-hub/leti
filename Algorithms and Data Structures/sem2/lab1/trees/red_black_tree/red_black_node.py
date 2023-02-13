from ..node import Node


class RedBlackNode(Node):
	def __init__(self, value, is_black: bool = False):
		self.is_black = is_black
		super().__init__(value)

	def __repr__(self):
		return f"({self.value}, {'b' if self.is_black else 'r'})"
