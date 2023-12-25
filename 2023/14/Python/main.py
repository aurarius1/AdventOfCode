import time


# this was my first approach, also works, but is slower (~2s)
def tilt_old(platform, direction="north"):
    outer = range(0)
    inner_end = 0
    inner = range(inner_end)
    steps = 1
    if direction == "west" or direction == "east":
        platform = list(map(list, zip(*platform)))
    if direction == "north" or direction == "west":
        outer = range(len(platform[0]))
        inner = range(len(platform))
        inner_end = len(platform)
    elif direction == "south" or direction == "east":
        outer = range(len(platform[0]))
        inner = range(len(platform)-1, -1, -1)
        inner_end = -1
        steps = -1

    for j in outer:
        for t in inner:
            if platform[t][j] != ".":
                continue

            for i in range(t, inner_end, steps):
                if platform[i][j] == "O":
                    platform[t][j] = "O"
                    platform[i][j] = "."
                    break
                if platform[i][j] == "#":
                    break
    if direction == "west" or direction == "east":
        platform = list(map(list, zip(*platform)))

    return platform


def tilt(platform, direction="north"):
    platform = transpose(platform)
    # O....O#....O
    for row in platform:
        first_empty = -1

        if direction == "north" or direction == "west":
            i = 0
            stop = len(row)
            step = 1
        else:
            i = len(row)-1
            stop = -1
            step = -1

        while i != stop:
            if first_empty == -1 and row[i] == ".":
                first_empty = i
            if row[i] == "#":
                first_empty = -1
            if row[i] == "O" and first_empty != -1:
                row[i] = "."
                row[first_empty] = "O"
                i = first_empty
                first_empty = -1
            i += step

    return platform


def tilt_wrapper(platform):
    platform = tilt(platform, "north")
    for i in range(3):
        platform = transpose(platform)
    return platform


def calculate_load(platform):
    return sum([platform[i].count("O") * (len(platform)-i) for i in range(len(platform))])


def platform_to_string(platform):
    return "|".join(["".join(row) for row in platform])


def string_to_platform(string):
    return [[*row] for row in string.split("|")]


def transpose(platform):
    return list(map(list, zip(*platform)))


def main(input_file, stage=1):
    with open(input_file) as puzzle:
        puzzle_input = puzzle.readlines()

        platform = [[*line.strip()] for line in puzzle_input]
        if stage == 1:
            print(calculate_load(transpose(tilt(platform))))
            return

        num_cycles = 1000000000
        cache = {}

        t = platform_to_string(platform)
        for i in range(num_cycles):
            if cache.get(t, None) is not None:
                period = i
                break

            for direction in ["north", "west", "south", "east"]:
                platform = tilt(platform, direction)  # was initially: tilt_old(platform, direction)

            cache[t] = platform_to_string(platform)
            t = cache[t]

        keys = list(cache.keys())
        first_cache = keys.index(t)
        period -= first_cache

        cache_key = keys[(num_cycles-first_cache) % period+(first_cache-1)]
        print(calculate_load(string_to_platform(cache[cache_key])))


if __name__ == "__main__":
    use_example = False
    file = "example" if use_example else "input"

    start = time.time()
    main(file, 1)
    print(f"Stage 1 time: {time.time()-start:.10f}")

    start = time.time()
    main(file, 2)
    print(f"Stage 2 time: {time.time()-start:.10f}")
