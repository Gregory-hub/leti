def tre(num: int):
    res = ''
    while num >= 3:
        res += str(num % 3)
        num //= 3
    res += str(num)
    return res[::-1]

min_res = max_res = None
for num in range(1000, 10000):
    if num % 5 != 0 and num % 7 != 0 and num % 11 != 0 and len(tre(num)) == 8:
        min_res = num
        break

for num in range(10000, 1000, -1):
    if num % 5 != 0 and num % 7 != 0 and num % 11 != 0 and len(tre(num)) == 8:
        max_res = num
        break

print(min_res, max_res)
