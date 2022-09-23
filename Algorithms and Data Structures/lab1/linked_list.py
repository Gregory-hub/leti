class Node:
    pass


class LinkedList:
    pass


class Node:
    def __init__(self, val: int):
        self.value = val
        self.next = None

    def set_next(self, next: Node):
        self.next = next

    def set_value(self, val: int):
        self.value = val

    def __eq__(self, other: Node):
        return self.value == other.value


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

    def is_empty(self):
        return self.size == 0

    def get(self, index: int):
        if index >= self.size or index < 0:
            return None

        node = self.head
        for i in range(index):
            node = node.next

        return node

    def print(self):
        node = self.head
        for i in range(self.size):
            if (node is None):
                ValueError("Error: size do not match actual nodes number")
                return
            print(i, ": ", node.value, sep='')
            node = node.next

    def insert(self, index: int, value: int):
        if not 0 <= index <= self.size:
            print("Error: invalid index")
            return
        if not isinstance(value, int):
            print("Error: value must be int")
            return

        node = Node(value)
        if index == 0:
            node.next = self.head
            self.head = node
        elif index == self.size:
            last_el = self.get(self.size - 1)
            if last_el is None:
                return
            last_el.next = node
        else:
            prev_el = self.get(index - 1)
            next_el = self.get(index)
            prev_el.next = node
            node.next = next_el

        self.size += 1

    def remove(self, index: int):
        if not 0 <= index < self.size:
            print("Error: invalid index")
            return

        prev_el = self.get(index - 1)
        next_el = self.get(index + 1)
        if prev_el is None and next_el is None:
            self.head = None
        elif prev_el is None:
            # prev_head = self.head
            self.head = self.get(1)
            # del prev_head
        elif next_el is None:
            prev_el = self.get(index - 1)
            prev_el.next = None
        else:
            prev_el.next = next_el

        self.size -= 1

    def append(self, value: int):
        if not isinstance(value, int):
            print("Error: value must be int")
            return

        self.insert(self.size, Node(value))

    def replace(self, index: int):
        if not 0 <= index < self.size:
            print("Error: invalid index")
            return

        self.remove(index)
        self.insert(index)

    def delete_all(self):
        for i in range(self.size):
            self.remove(i)

    def find_last_list_entrance(self, li: LinkedList):
        last_entrance = -1

        curr_el = self.head
        i = 0
        while i <= self.size - li.size:
            if curr_el == li.get(0):
                el1 = curr_el
                el2 = li.get(0)
                success = True
                for j in range(li.size):
                    if el1 == el2:
                        el1 = el1.next
                        el2 = el2.next
                    else:
                        success = False
                        break
                if success:
                    last_entrance = i
            curr_el = curr_el.next
            i += 1

        return last_entrance
