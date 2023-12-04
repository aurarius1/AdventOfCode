import regex as re
import time

def part1(config):
    cc = 0
    for line in config:
        matches = re.findall("[0-9]", line)
        number = matches[0] + matches[-1]
        cc += int(number)
    print(cc)


def part2(config):
    numbers = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"]
    cc = 0
    pattern = f"[0-9]|{'|'.join(numbers)}"
    for line in config:
        matches = re.findall(pattern, line, overlapped=True)
        if not str.isdigit(matches[0]):
            matches[0] = str(numbers.index(matches[0]) + 1)
        if not str.isdigit(matches[-1]):
            matches[-1] = str(numbers.index(matches[-1]) + 1)
        number = matches[0] + matches[-1]
        cc += int(number)
    print(cc)


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    start = time.time()
    file = open("./config", "r")
    lines = file.readlines()
    file.close()
    part1(lines)
    print(time.time() - start)

    start = time.time()
    file = open("./config", "r")
    lines = file.readlines()
    file.close()
    part2(lines)
    print(time.time() - start)

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
