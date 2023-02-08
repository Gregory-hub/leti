from queue import Queue

from red_black_node import Node


class RedBlackTree:
	def __init__(self, elements: list):
		self.root = None
		for el in elements:
			self.insert(el)

	# public:
	def height(self) -> int:
		# O(n)
		return self.__get_height(self.root)

	def print_tree(self, detail: bool = False) -> None:
		# O(n)
		print(f"Tree of height {self.height()}:")
		self.__print_tree(self.root, detail=detail)
		print()

	def min(self) -> Node:
		# O(h)
		return self.__min(self.root)

	def max(self) -> Node:
		# O(h)
		return self.__max(self.root)

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

	def find(self, value: int) -> Node:
		# O(h)
		return self.__find(self.root, value)

	def next_el(self, node: Node) -> Node:
		# O(h)
		if node is None or not self.has_node(node):
			return None
		if node.right is not None:
			return self.__min(node.right)

		return self.__get_next_in_ancestors(self.root, node)

	def prev_el(self, node: Node) -> Node:
		# O(h)
		if node is None or not self.has_node(node):
			return None
		if node.left is not None:
			return self.__max(node.left)

		return self.__get_prev_in_ancestors(self.root, node)

	def get_parent(self, node: Node) -> Node:
		# O(h)
		if node is None:
			return None
		return self.__get_parent(node, self.root)
	
	def has_node(self, node: Node) -> bool:
		# O(h)
		return self.__has_node(self.root, node)

	def left_rotate(self, node: Node) -> None:
		# O(h)
		if node.right is None:
			return

		right = node.right
		node.right = right.left
		right.left = node

		parent = self.get_parent(node)	# O(h)
		if parent is None:
			self.root = right
		elif parent.left is node:
			parent.left = right
		elif parent.right is node:
			parent.right = right

	def right_rotate(self, node: Node) -> None:
		# O(h)
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

	def insert(self, value) -> None:
		node = Node(value=value, is_black=False)
		parent = self.root
		if parent is None:
			node.is_black = True
			self.root = node
			return

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
		self.fixup(node)

	def fixup(self, curr: Node):
		parent = self.get_parent(curr)
		while parent is not None and not parent.is_black:
			if parent is None:
				curr.is_black = True
				self.root = curr
				return

			grandparent = self.get_parent(parent)
			if grandparent is None:
				parent.is_black = True
				return
			if grandparent.left is parent:
				uncle = grandparent.right
			if grandparent.right is parent:
				uncle = grandparent.left

			if uncle is not None and not uncle.is_black:
				grandparent.is_black = False
				parent.is_black = True
				uncle.is_black = True
				curr = grandparent
			else:
				if grandparent.right is parent:
					if parent.left is curr:
						self.right_rotate(parent)
						curr, parent = parent, curr
					self.left_rotate(grandparent)
					grandparent.is_black = False
					parent.is_black = True
				elif grandparent.left is parent:
					if parent.right is curr:
						self.left_rotate(parent)
						curr, parent = parent, curr
					self.right_rotate(grandparent)
					grandparent.is_black = False
					parent.is_black = True

			parent = self.get_parent(curr)
		self.root.is_black = True

	def bst_delete(self, node: Node) -> Node:
		# O(n)
		if node is None:
			return None
		if not self.has_node(node): # O(h)
			return None

		parent = self.get_parent(node) # O(h)
		if node.right is None or node.left is None:	# node has 0 or 1 child
			if node.right is None and node.left is None:	# 0 child
				successor = None
			elif node.right is None:	# 1 child
				successor = node.left
			elif node.left is None:		# 1 child
				successor = node.right

		else:	# node has 2 children
			successor = self.__min(node.right)	# O(h)
			successors_parent = self.get_parent(successor)	# O(h)
			if successors_parent is not node:
				successors_parent.left = successor.right
				successor.right = node.right
			successor.left = node.left

		if parent:
			if node is parent.left:
				parent.left = successor
			elif node is parent.right:
				parent.right = successor
		else:
			self.root = successor

		return successor

	def delete(self, node: Node) -> None:
		if not self.has_node(node): # O(h)
			return

		node = self.bst_delete(node)

		if not node.is_black:
			return

		while node is not self.root and not node.is_black:
			parent = self.get_parent(node)
			if node is parent.left:
				brother = parent.right
				if brother is not None and not brother.is_black:
					brother.is_black = True
					parent.is_black = False
					self.left_rotate(parent)
					parent = self.get_parent(node)
					brother = parent.right
				if (brother.left is None or brother.left.is_black) and (brother.right is None or brother.right.is_black):
					brother.is_black = False
					node = parent
				elif brother.right is None or brother.right.is_black:
					brother.left.is_black = True
					brother.is_black = False
					self.right_rotate(brother)
					parent = self.get_parent(node)
					brother = parent.right
				else:
					brother.is_black = parent.is_black
					parent.is_black = True
					if brother.right:
						brother.right.is_black = True
					self.left_rotate(parent)
					node = self.root
			else:
				brother = parent.left
				if brother is not None and not brother.is_black:
					brother.is_black = True
					parent.is_black = False
					self.left_rotate(parent)
					parent = self.get_parent(node)
					brother = parent.left
				if (brother.left is None or brother.left.is_black) and (brother.right is None or brother.right.is_black):
					brother.is_black = False
					node = parent
				elif brother.left is None or brother.left.is_black:
					brother.right.is_black = True
					brother.is_black = False
					self.left_rotate(brother)
					parent = self.get_parent(node)
					brother = parent.left
				else:
					brother.is_black = parent.is_black
					parent.is_black = True
					if brother.left:
						brother.left.is_black = True
					self.left_rotate(parent)
					node = self.root
			node.is_black = True

	# private:
	def __get_height(self, root: Node) -> int:
		# O(n)
		if root is None:
			return -1
		if root.left is None and root.right is None:
			return 0
		
		height_l = self.__get_height(root.left)
		height_r = self.__get_height(root.right)
		return max(height_l, height_r) + 1

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

	def __get_parent(self, node: Node, root: Node) -> Node:
		# O(h)
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

	def __has_node(self, root: Node, node: Node) -> bool:
		# O(h)
		if root is None or node is None:
			return False

		if node.value == root.value:
			if node == root:
				return True
			return self.__has_node(root.left, node)	or self.__has_node(root.right, node)
		if node.value < root.value:
			return self.__has_node(root.left, node)
		if node.value > root.value:
			return self.__has_node(root.right, node)


# tests for this are in tests.py in the same directory
if __name__ == "__main__":
	li = list(range(0, 10))
	tree = RedBlackTree(li)

	el = tree.find(5)

	print("\nInitial tree:")
	tree.print_tree(detail=True)

	print(f"Deletion of {el}:\n")
	tree.delete(el)
	print("Tree after deletion:")
	tree.print_tree(detail=True)
