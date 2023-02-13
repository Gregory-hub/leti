from queue import Queue

from ..bin_tree import Node, BinTree


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

	def insert(self, value: int):
		node = Node(value=value)
		parent = self.root
		if parent is None:
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

	def delete(self, node: Node):
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
		if root is None:
			return False

		if node.value == root.value:
			if node == root:
				return True
			return self.__has_node(root.left, node)	or self.__has_node(root.right, node)
		if node.value < root.value:
			return self.__has_node(root.left, node)
		if node.value > root.value:
			return self.__has_node(root.right, node)
