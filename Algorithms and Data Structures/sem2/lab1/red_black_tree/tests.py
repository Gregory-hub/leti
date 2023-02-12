import unittest

from red_black_tree import RedBlackTree
from random import sample


class RBTreeTester(unittest.TestCase):
    def __init__(self, *args, **kwargs):
        self.deleted_count = 0
        super().__init__(*args, **kwargs)

    def check_tree(self, tree: RedBlackTree):
        if tree.root is None:
            return
        self.assertTrue(tree.root.is_black, f"Root is not black. Tree len is {len(self.li) - self.deleted_count}")
        self.check_subtree(tree.root)

    def check_subtree(self, root, b_height: int = 1):
        if root is None:
            return b_height

        if not root.is_black:
            self.assertTrue(root.left is None or root.left.is_black, f"Red node has red child. Tree len is {len(self.li) - self.deleted_count}")
            self.assertTrue(root.right is None or root.right.is_black, f"Red node has red child. Tree len is {len(self.li) - self.deleted_count}")

        l_b_height = self.check_subtree(root.left)
        r_b_height = self.check_subtree(root.right)

        self.assertEqual(l_b_height, r_b_height, f"Black height is not equal in subtrees. Tree len is {len(self.li) - self.deleted_count}")
        return l_b_height

    def test_delete(self):
        self.li = sample(range(-100, 100), 100)
        tree = RedBlackTree(self.li)
        self.check_tree(tree)

        for el in self.li:
            tree.delete(tree.find(el))
            self.deleted_count += 1
            self.check_tree(tree)

if __name__ == "__main__":
    unittest.main()
