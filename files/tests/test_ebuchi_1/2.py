num_count = int(input())
marks = []
for i in range(num_count):
    marks.append(int(input()))
marks.sort()
while marks[0] < 20:
    marks = marks[1:]
print(marks[0])