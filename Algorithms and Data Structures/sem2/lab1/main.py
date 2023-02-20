from random import randrange

from trees.red_black_tree.red_black_tree import RedBlackTree
from trees.binary_search_tree.bin_search_tree import BinSearchTree
from trees.avl_tree.avl_tree import AVLTree


if __name__ == "__main__":
	li = [randrange(-10, 10) for _ in range(6)]
	bst = BinSearchTree(li)
	rbt = RedBlackTree(li)
	avl = AVLTree(li)
	print("li:", li)

	detail = False
	print("BST")
	bst.print_tree(detail=detail)
	print("RBT")
	rbt.print_tree(detail=True)
	print("AVL")
	avl.print_tree(detail=detail)
	
	value = 2
	print(f"Insert {value}\n")

	bst.insert(value)
	rbt.insert(value)
	avl.insert(value)

	print("BST")
	bst.print_tree(detail=detail)
	print("RBT")
	rbt.print_tree(detail=True)
	print("AVL")
	avl.print_tree(detail=detail)
