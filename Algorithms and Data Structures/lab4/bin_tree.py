from queue import Queue


class Node:
	def __init__(self, value: int):
		self.value = value
		self.left = None
		self.right = None

	def __repr__(self):
		return f"<Node({self.value}) at {hex(id(self))}>"


class BinSearchTree:
	def __init__(self, elements: list):
		elements.sort()
		self.root = self.__tree_from_list(elements, len(elements))

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

	def delete(self, node: Node):
		# O(n)
		if node is None:
			return self
		if not self.has_node(node):
			return self

		parent = self.get_parent(node)
		successors = self.__get_nodes_inorder(node.left) + self.__get_nodes_inorder(node.right)

		if parent is None:
			self.root = self.__tree_from_list([el.value for el in successors], len(successors))
		elif parent.left and parent.left == node:
			parent.left = self.__tree_from_list([el.value for el in successors], len(successors))
		elif parent.right and parent.right == node:
			parent.right = self.__tree_from_list([el.value for el in successors], len(successors))

		return self

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


# tests for this are in tests.py in the same directory
if __name__ == "__main__":
	li = list(range(-5, 5))
	tree = BinSearchTree(li)

	value_to_find = -2
	el = tree.root.left

	tree.print_tree()
	print("Height:", tree.height())
	print("Min:", tree.min())
	print("Max:", tree.max())
	print()
	print("Traversal preorder:", [i.value for i in tree.get_nodes_preorder()])
	print("Traversal inorder:", [i.value for i in tree.get_nodes_inorder()])
	print("Traversal postorder:", [i.value for i in tree.get_nodes_postorder()])
	print("Traversal breadth first:", [i.value for i in tree.get_nodes_breadth_first()])
	print()
	print(f"Find {value_to_find}:", tree.find(value_to_find))
	print(f"Next to {el}:", tree.next_el(el))
	print(f"Previous to {el}:", tree.prev_el(el))
	print(f"Parent of {el}:", tree.get_parent(el))
	print(f"Tree has node {el}:", tree.has_node(el))
	new_el = Node(el.value)
	print(f"Tree has node {new_el}:", tree.has_node(new_el))

	print(f"\nDeletion of {el}:\n")
	print("Initial tree:")
	tree.print_tree(detail=True)

	tree.delete(el)

	print("Tree after deletion:")
	tree.print_tree(detail=True)
	print(f"Tree has node {el}:", tree.has_node(el))
