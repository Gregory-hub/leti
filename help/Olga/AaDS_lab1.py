class Node:
    pass


class LinkedList:
    pass


class Node:
    def __init__(self, val: int):
        self.value = val
        self.next = None

    def __eq__(self, other: Node):
        return self.value == other.value

    def __repr__(self):
        next_value = self.next.value if self.next else None
        return f"<Node(value={self.value}, next=<Node(value={next_value})>)>"


class LinkedList:
    def __init__(self, els: list = None):
        self.size = 0
        if els is None or len(els) == 0:
            self.head = None
        else:
            self.head = Node(els[0])
            node = self.head
            self.size = 1
            for el in els[1:]:
                node.next = Node(el)
                node = node.next
                self.size += 1

    def __repr__(self):
        return f"<LinkedList(head={self.head}, size={self.size})>"

    def is_empty(self):
        # O(1)
        return self.size == 0

    def get_size(self):
        # O(1)
        return self.size

    def get(self, index: int):
        # O(n)
        if index >= self.size or index < 0:
            return None

        node = self.head
        for i in range(index):
            node = node.next

        return node

    def print(self):
        # O(n)
        node = self.head
        for i in range(self.size):
            if (node is None):
                ValueError("Error: size do not match actual nodes number")
                return
            print(i, ": ", node.value, sep='')
            node = node.next

    def insert(self, index: int, value: int):
        # O(n)
        # index == 0 => O(1)
        # index =! 0 => O(n)
        if not 0 <= index <= self.size:
            print("Error: invalid index")
            return
        if not isinstance(value, int):
            print("Error: value must be int")
            return

        node = Node(value)
        if index == 0:
            # O(1)
            node.next = self.head
            self.head = node
        elif index == self.size:
            # O(n)
            last_el = self.get(self.size - 1)
            if last_el is None:
                return
            last_el.next = node
        else:
            # O(n)
            prev_el = self.get(index - 1)
            next_el = self.get(index)
            prev_el.next = node
            node.next = next_el

        self.size += 1

    def add_left(self, value: int):
        # O(1)
        self.insert(0, value)

    def add_right(self, value: int):
        # O(n)
        if not isinstance(value, int):
            print("Error: value must be int")
            return

        self.insert(self.size, value)

    def remove(self, index: int):
        # O(n)
        if not 0 <= index < self.size:
            print("Error: invalid index")
            return

        prev_el = self.get(index - 1)
        next_el = self.get(index + 1)
        if prev_el is None and next_el is None:
            # O(1)
            self.head = None
        elif prev_el is None:
            # O(1)
            self.head = self.get(1)
        elif next_el is None:
            # O(1)
            prev_el.next = None
        else:
            # O(1)
            prev_el.next = next_el

        self.size -= 1

    def remove_last(self):
        # O(n)
        self.remove(self.size - 1)

    def remove_first(self):
        # O(1)
        self.remove(0)

    def replace(self, index: int, value: int):
        # O(n)
        if not 0 <= index < self.size:
            print("Error: invalid index")
            return

        self.remove(index)
        self.insert(index, value)

    def delete_all(self):
        # O(n)
        while (self.head is not None):
            self.remove_first()

    def insert_list(self, index: int, li: LinkedList):
		# O(n)
        if li.size == 0 or index < 0 or index > self.size:
            print("Error: invalid index")
            return

        if index == 0:
			# O(n)
            head = self.head
            self.head = li.head
            li.get(li.size - 1).next = head
        elif index == self.size:
			# O(n)
            self.get(self.size - 1).next = li.head
        else:
			# O(n)
            prev_el = self.get(index - 1)
            next_el = prev_el.next
            prev_el.next = li.head
            li.get(li.size - 1).next = next_el

        self.size += li.size


def main():
    li = LinkedList([2, 3, 4, 0, -1, 0, 1, 3, 4, 5, 6, -1, 0, 1, 2, 3, 4, 5, 6, 7])
    li.remove_first()
    li.remove_last()
    li.add_left(0)
    li.add_right(10)
    li.insert(3, 13)
    li.print()
    li.insert_list(4, LinkedList([5, -2, 4, 0]))
    li.print()
    print(li.is_empty())
    li.delete_all()
    li.print()
    print(li.is_empty())


if __name__ == "__main__":
    main()
