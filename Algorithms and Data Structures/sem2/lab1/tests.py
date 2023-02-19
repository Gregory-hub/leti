import unittest

from trees.red_black_tree.red_black_tree import RedBlackTree
from trees.binary_search_tree.bin_search_tree import BinSearchTree
from trees.avl_tree.avl_tree import AVLTree
from random import randrange


class BSTTester(unittest.TestCase):
    def __init__(self, *args, **kwargs):
        self.tree = None
        super().__init__(*args, **kwargs)

    def check_tree(self, root):
        if root is None:
            return
        self.assertTrue(root.left is None or root.left.value <= root.value)
        self.assertTrue(root.right is None or root.right.value >= root.value)

        self.check_tree(root.left)
        self.check_tree(root.right)

    def test_bst_insert(self):
        self.li = [randrange(-100, 100) for _ in range(100)]
        self.tree = BinSearchTree([])
        self.check_tree(self.tree.root)

        for el in self.li:
            node = self.tree.insert(el)
            self.check_tree(self.tree.root)
            self.assertTrue(self.tree.has_node(self.tree.root, node))


class RBTTester(unittest.TestCase):
    def __init__(self, *args, **kwargs):
        self.tree = None
        super().__init__(*args, **kwargs)

    def check_tree(self, tree: RedBlackTree):
        if tree.root is None:
            return
        self.assertTrue(tree.root.is_black, f"Root is not black. Tree height is {self.tree.height(self.tree.root)}")
        self.check_subtree(tree.root)

    def check_subtree(self, node, b_height: int = 1):
        if node is None:
            return b_height

        if not node.is_black:
            self.assertTrue(node.left is None or node.left.is_black, f"Red node has red child. Tree height is {self.tree.height(self.tree.root)}")
            self.assertTrue(node.right is None or node.right.is_black, f"Red node has red child. Tree height is {self.tree.height(self.tree.root)}")

        self.assertTrue(node.left is None or node.left.value <= node.value)
        self.assertTrue(node.right is None or node.right.value >= node.value)

        l_b_height = self.check_subtree(node.left)
        r_b_height = self.check_subtree(node.right)

        self.assertEqual(l_b_height, r_b_height, f"Black height is not equal in subtrees. Tree height is {self.tree.height(self.tree.root)}")
        return l_b_height

    def test_rbt_insert(self):
        self.li = [randrange(-100, 100) for _ in range(100)]
        self.tree = RedBlackTree([])
        self.check_tree(self.tree)

        for el in self.li:
            node = self.tree.insert(el)
            self.check_tree(self.tree)
            self.assertTrue(self.tree.has_node(self.tree.root, node))


class AVLTester(unittest.TestCase):
    def __init__(self, *args, **kwargs):
        self.tree = None
        super().__init__(*args, **kwargs)

    def check_tree(self, root):
        if root is None:
            return

        self.assertTrue(root.left is None or root.left.value <= root.value, f"Node: {root}")
        self.assertTrue(root.right is None or root.right.value >= root.value, f"Node: {root}")

        self.check_tree(root.left)
        self.check_tree(root.right)

        balance = self.tree.balance(root)
        self.assertTrue(abs(balance) < 2, f"Balance is bigger than 1(node: {root}, {balance})")

    def test_avl_insert(self):
        self.li = [randrange(-100, 100) for _ in range(100)]
        self.tree = AVLTree([])
        self.check_tree(self.tree.root)

        for el in self.li:
            node = self.tree.insert(el)
            self.check_tree(self.tree.root)
            self.assertTrue(self.tree.has_node(self.tree.root, node))


if __name__ == "__main__":
    unittest.main()
