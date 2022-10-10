from random import randint
from math import ceil, floor


def create_snails(side_size: int):
    snail_num = randint(2, SIDE_SIZE**2)
    snails = []
    for i in range(snail_num):
        snails.append([randint(0, SIDE_SIZE), randint(0, SIDE_SIZE)])
    return snails


def distance(snail1: list, snail2: list):
    dist_x = snail1[0] - snail2[0]
    dist_y = snail1[1] - snail2[1]
    return (dist_x**2 + dist_y**2)**(1/2)


def merge_snails(l_snails: list, r_snails: list, by_y: bool = False):
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
    if len(snails) <= 1:
        return snails
    mid_i = ceil(len(snails) / 2)
    snails_l = merge_sort(snails[:mid_i], by_y)
    snails_r = merge_sort(snails[mid_i:], by_y)
    return merge_snails(snails_l, snails_r, by_y)


def min_in_mid_area(l_snails: list, r_snails: list):
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
    if len(snails) == 1:
        return -1, snails, False
    
    l_min_dist, l_snails, l_confused = find_min_dist(snails[:floor(len(snails) / 2)])
    r_min_dist, r_snails, r_confused = find_min_dist(snails[floor(len(snails) / 2):])

    if l_min_dist == -1 and r_min_dist == -1:       # one snail in each
        min_dist = distance(l_snails[0], r_snails[0])
    elif l_min_dist == -1:                          # one snail in l
        min_dist = r_min_dist
    elif r_min_dist == -1:                          # one snail in r
        min_dist = l_min_dist
    else:                                           # > 1 snails in each
        if l_min_dist == r_min_dist:
            confused = True
        min_dist = min(l_min_dist, r_min_dist)

    l_snails = merge_sort(l_snails[ceil(len(l_snails) - min_dist):], by_y=True)
    r_snails = merge_sort(r_snails[:ceil(min_dist)], by_y=True)

    min_mid = min_in_mid_area(l_snails, r_snails)   # from (snails / 2 - min_dist) to (snails / 2 + min_dist)
    if min_dist == min_mid:
        confused = True
    elif min_mid < min_dist:
        min_dist = min_mid
        confused = False

    return min_dist, snails, confused


SIDE_SIZE = 3
# snails = create_snails(SIDE_SIZE)
snails = merge_sort([[0, 2], [0, 4], [0, 0], [2, 4]])

min_dist, snails, confused = find_min_dist(snails)
print("Snails:")
print(*snails, sep='\n')
print("Time:", str(min_dist) + "s")
print("Snails are confused:", confused)
