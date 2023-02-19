def converter(num: int):
    digits = str(num)
    sums = sorted([
        int(digits[0]) + int(digits[1]),
        int(digits[1]) + int(digits[2]),
        int(digits[2]) + int(digits[3]),
        ], reverse=True)[1:]
    result = int(str(sums[0]) + str(sums[1]))
    return result


for num in range(9999, 999, -1):
    if converter(num) == 145:
        print(num)
        break
