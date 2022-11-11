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
	def height(self) -> int:
		# O(n)
		return self.__get_height(self.root)

	def print_tree(self) -> None:
		# O(n)
		print(f"Tree of height {self.height()}:")
		self.__print_tree(self.root)
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
	
	def next_el(self, node: Node):
		# O(h)
		if node is None:
			return None
		if node.right is not None:
			return self.__min(node.right)

		return self.__get_next_in_ancestors(self.root, node)

	def prev_el(self, node: Node):
		# O(h)
		if node is None:
			return None
		if node.left is not None:
			return self.__max(node.left)

		return self.__get_prev_in_ancestors(self.root, node)

	def get_parent(self, node: Node) -> Node:
		if node is None:
			return None
		return self.__get_parent(node, self.root)

	def delete(self, node: Node):
		if node is None:
			return self
		parent = self.get_parent(node)
		successors = self.__get_nodes_inorder(node.left) + self.__get_nodes_inorder(node.right)

		if parent is None:
			self.root = self.__tree_from_list_of_nodes(successors, len(successors))
		elif parent.left and parent.left == node:
			parent.left = self.__tree_from_list_of_nodes(successors, len(successors))
		elif parent.right and parent.right == node:
			parent.right = self.__tree_from_list_of_nodes(successors, len(successors))

		return self

	# private:
	def __get_height(self, root: Node) -> int:
		# O(n)
		if root is None:
			return 0
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

	def __tree_from_list_of_nodes(self, nodes: list, n: int) -> Node:
		# elements must be sorted
		# O(n)
		if n == 1:
			return nodes[0]
		if n == 0:
			return None

		root = nodes[n // 2]
		left = self.__tree_from_list(nodes[:n // 2], n // 2)
		right = self.__tree_from_list(nodes[n // 2 + 1:], n // 2 - 1 + n % 2)
		root.left = left
		root.right = right
		return root

	def __print_tree(self, root: Node, level: int = 0, child_letter = '') -> None:
		# O(n)
		if root is None:
			return

		child_str = child_letter + ':' if child_letter != '' else ''
		print(". ", ". " * (level - 1), child_str, root.value, sep='')
		self.__print_tree(root.left, level + 1, 'l')
		self.__print_tree(root.right, level + 1, 'r')

	def __min(self, root: Node) -> Node:
		# O(h)
		if root.left is None:
			return root
		return self.__min(root.left)

	def __max(self, root: Node) -> Node:
		# O(h)
		if root.right is None:
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

		# 		   1
		# 	     /   \
		#  	   -1      3
		#     / \     / \
		#   -2   0   2   4
		#   /
		# -3   

if __name__ == "__main__":
	li = [-1, -2, -3, 0, 1, 2, 3, 4]
	tree = BinSearchTree(li)
	value_to_find = 1
	el = tree.root.right.left

	tree.print_tree()
	print("Min:", tree.min().value)
	print("Max:", tree.max().value)
	print("Traversal preorder:", [i.value for i in tree.get_nodes_preorder()])
	print("Traversal inorder:", [i.value for i in tree.get_nodes_inorder()])
	print("Traversal postorder:", [i.value for i in tree.get_nodes_postorder()])
	print(f"Find {value_to_find}:", tree.find(value_to_find))
	print(f"Next to {el.value}:", tree.next_el(el))
	print(f"Previous to {el.value}:", tree.prev_el(el))
	print(f"Parent of {el.value}:", tree.get_parent(el))

	print(f"\nDeletion of {el}\n")
	print("Initial tree:\n")
	tree.print_tree()

	tree.delete(el)

	print("Tree after deletion:\n")
	tree.print_tree()

	print("Min:", tree.min().value)
	print("Max:", tree.max().value)
	print("Traversal preorder:", [i.value for i in tree.get_nodes_preorder()])
	print("Traversal inorder:", [i.value for i in tree.get_nodes_inorder()])
	print("Traversal postorder:", [i.value for i in tree.get_nodes_postorder()])
	print(f"Find {el.value}:", tree.find(el.value))
