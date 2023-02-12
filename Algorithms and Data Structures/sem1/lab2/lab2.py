from random import randint
from math import ceil, log2
from matplotlib import pyplot as plt
from datetime import date, datetime


def create_snails(side_size: int, number_of_snails: int):
	# O(n)
    snails = []
    for i in range(number_of_snails):
        snails.append([randint(0, side_size), randint(0, side_size)])
    return merge_sort(snails)


def distance(snail1: list, snail2: list):
	# O(1)
    dist_x = snail1[0] - snail2[0]
    dist_y = snail1[1] - snail2[1]
    return (dist_x**2 + dist_y**2)**(1/2)


def merge_snails(l_snails: list, r_snails: list, by_y: bool = False):
	# O(n)
	# n = len(l) + len(r)
    snails = []
    l, r = 0, 0
    for i in range(len(l_snails) + len(r_snails)):
        if l == len(l_snails):
            for j in range(r, len(r_snails)):
                snails.append(r_snails[j])
            break
        if r == len(r_snails):
            for j in range(l, len(l_snails)):
                snails.append(l_snails[j])
            break

        if by_y:
            main_i = 1
            secondary_i = 0
        else:
            main_i = 0
            secondary_i = 1

        if l_snails[l][main_i] < r_snails[r][main_i] or (l_snails[l][main_i] == r_snails[r][main_i] and l_snails[l][secondary_i] < r_snails[r][secondary_i]):
            snails.append(l_snails[l])
            l += 1
        else:
            snails.append(r_snails[r])
            r += 1

    return snails


def merge_sort(snails: list, by_y: bool = False):
	# 2T(n/2) + O(n) = O(nlogn)
    if len(snails) <= 1:
        return snails
    mid_i = len(snails) // 2
    snails_l = merge_sort(snails[:mid_i], by_y)		# T(n/2)
    snails_r = merge_sort(snails[mid_i:], by_y)		# T(n/2)
    return merge_snails(snails_l, snails_r, by_y)	# O(n)


def min_in_mid_area(l_snails: list, r_snails: list):
	# O(n**2)
    min_dist = SIDE_SIZE
    for l_sn in l_snails:
        r_i = 0
        init_min_dist = min_dist
        while r_i < len(r_snails) and l_sn[1] - r_snails[r_i][1] > init_min_dist:
            r_i += 1
        while r_i < len(r_snails) and l_sn[1] - r_snails[r_i][1] <= init_min_dist:
            if distance(l_sn, r_snails[r_i]) < min_dist:
                min_dist = distance(l_sn, r_snails[r_i])
            r_i += 1

    return min_dist


def find_min_dist(snails: list, confused: bool = False):
	# 2T(n/2) + 2O(nlogn) = O(nlogn)
    # snails must be sorted by x first, than by y
    if len(snails) <= 1:
        return -1, snails, False
    
    l_min_dist, l_snails, l_confused = find_min_dist(snails[:len(snails) // 2])		# T(n/2)
    r_min_dist, r_snails, r_confused = find_min_dist(snails[len(snails) // 2:])		# T(n/2)

    if l_min_dist == -1 and r_min_dist == -1:       # one snail in each
        min_dist = distance(l_snails[0], r_snails[0])								# O(1)
    elif l_min_dist == -1:                          # one snail in l
        min_dist = r_min_dist
    elif r_min_dist == -1:                          # one snail in r
        min_dist = l_min_dist
    else:                                           # > 1 snails in each
        if l_min_dist == r_min_dist:
            confused = True
        min_dist = min(l_min_dist, r_min_dist)

    l_snails = merge_sort(l_snails[ceil(len(l_snails) - min_dist):], by_y=True)		# O(nlogn)
    r_snails = merge_sort(r_snails[:ceil(min_dist)], by_y=True)						# O(nlogn)

    # from (snails / 2 - min_dist) to (snails / 2 + min_dist)
    min_mid = min_in_mid_area(l_snails, r_snails)                                   # O(n) because number of dots from r for every dot in l <= some const
    if min_dist == min_mid:
        confused = True
    elif min_mid < min_dist:
        min_dist = min_mid
        confused = False

    return min_dist, snails, confused


def test_algorithm(side_size: int, number_of_snails: int):
	snails = create_snails(side_size, number_of_snails)
	start_time = datetime.now()
	find_min_dist(snails)
	end_time = datetime.now()
	return (end_time - start_time).total_seconds()


SIDE_SIZE = 10			# in centimetres
NUMBER_OF_SNAILS = 5

# snails = create_snails(SIDE_SIZE, NUMBER_OF_SNAILS)
# start_time = datetime.now()
# min_dist, snails, confused = find_min_dist(snails)
# end_time = datetime.now()

# print("Snails:\n", *snails, sep='\n')
# print("Time for first snail to find a partner:", str(round(min_dist, 2)) + "s")		# min_dist / speed = min_dist / 1 = min_dist
# print("Snails are confused:", confused)

x = [i for i in range(1000, 100000, 1000)]
y_snails = []
for number_of_snails in x:
	y_snails.append(round(test_algorithm(SIDE_SIZE, number_of_snails) * 1000, 2))
y_n = [i // 1000 for i in x]
y_log = [log2(i) for i in x]
y_nlog = [i * log2(i) // 10000 for i in x]

plt.plot(x, y_snails, color='b', label="snails")
plt.plot(x, y_n, color='g', label="y(n) = n / 1000")
plt.plot(x, y_log, color='r', label="y(n) = log(n)")
plt.plot(x, y_nlog, color='y', label="y(n) = nlog(n) / 10000")

plt.xlabel("number of snails")
plt.ylabel("time, ms")
plt.title("Time complexity")
plt.legend()
plt.show()
