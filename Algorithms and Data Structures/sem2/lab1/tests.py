import unittest

from trees.red_black_tree.red_black_tree import RedBlackTree
from trees.binary_search_tree.bin_search_tree import BinSearchTree
from random import sample


class TreeTester(unittest.TestCase):
    def __init__(self, *args, **kwargs):
        self.tree = None
        super().__init__(*args, **kwargs)

    def check_tree(self, tree: RedBlackTree):
        if tree.root is None:
            return
        self.assertTrue(tree.root.is_black, f"Root is not black. Tree height is {self.tree.height()}")
        self.check_subtree(tree.root)

    def check_subtree(self, root, b_height: int = 1):
        if root is None:
            return b_height

        if not root.is_black:
            self.assertTrue(root.left is None or root.left.is_black, f"Red node has red child. Tree height is {self.tree.height()}")
            self.assertTrue(root.right is None or root.right.is_black, f"Red node has red child. Tree height is {self.tree.height()}")

        l_b_height = self.check_subtree(root.left)
        r_b_height = self.check_subtree(root.right)

        self.assertEqual(l_b_height, r_b_height, f"Black height is not equal in subtrees. Tree height is {self.tree.height()}")
        return l_b_height

    def test_bst_insert(self):
        self.li = sample(range(-100, 100), 100)
        self.tree = BinSearchTree([])
        self.check_tree(self.tree)

        for el in self.li:
            self.tree.insert(el)
            self.check_tree(self.tree)

    def test_rbt_insert(self):
        self.li = sample(range(-100, 100), 100)
        self.tree = RedBlackTree([])
        self.check_tree(self.tree)

        for el in self.li:
            self.tree.insert(el)
            self.check_tree(self.tree)

if __name__ == "__main__":
    unittest.main()
