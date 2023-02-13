from trees.red_black_tree.red_black_tree import RedBlackTree
from trees.binary_search_tree.bin_search_tree import BinSearchTree


if __name__ == "__main__":
	bst = BinSearchTree([5, 2, 4, 8])
	rbt = RedBlackTree([5, 2, 4, 8])
	bst.print_tree()
	rbt.print_tree(detail=True)
