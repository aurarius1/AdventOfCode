import re


def is_symbol(character):
    return character != "." and (32 < ord(character) < 48 or 57 < ord(character) < 65)


def adjacent_in_line(index, lines, start, end):
    for i in range(max(0, index-1), min(len(lines), index+2)):
        for j in range(max(0, start), min(len(lines[i]), end+1)):
            if is_symbol(lines[i][j]):
                return int(lines[index][start+1:end])
    return 0


def find_adjacent_numbers(index, lines, start, stage=1):
    adjacent_numbers = []
    for i in range(max(0, index-1), min(len(lines), index+2)):
        numbers = re.finditer(r'\d+', lines[i])
        adjacent_numbers.extend([int(n.group()) for n in numbers if n.start()-1 <= start <= n.end()])
    if stage == 1:
        return sum(adjacent_numbers)
    if len(adjacent_numbers) > 2 or len(adjacent_numbers) < 2:
        return 0
    return adjacent_numbers[0] * adjacent_numbers[1]


def main(stage):
    file = open("input", "r")
    lines = file.readlines()
    file.close()
    part_numbers = []
    for index, line in enumerate(lines):
        #if stage == 1:
        #    numbers = re.finditer(r'\d+', line)
        #    part_numbers.extend([adjacent_in_line(index, lines, number.start()-1, number.end()) for number in numbers])
        #elif stage == 2:
        stars = re.finditer(r'!|"|#|\$|%|&|\'|\(|\)|\*|\+|,|-|/|:|;|<|=|>|\?|@', line)
        part_numbers.extend([find_adjacent_numbers(index, lines, star.start(), stage) for star in stars])
    print(sum(part_numbers))


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    main(2)

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
