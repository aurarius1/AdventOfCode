import time

directions = {
    ">": [(0, 1)],
    "<": [(0, -1)],
    "v": [(1, 0)],
    ".": [(1, 0), (-1, 0), (0, 1), (0, -1)],
    "#": []
}
intersections = {}


def print_map(hiking_map):
    for row in hiking_map:
        print(row)
    print()

hiking_map = []
def walk_trail(hiking_map, pos, steps, end, prev=None, visited=None):
    global directions, intersections
    if visited is None:
        visited = set()
    if pos == end:
        return steps
    if prev is None:
        prev_dir = (0, 0)
    else:
        prev_dir = (pos[0]-prev[0], pos[1]-prev[1])
    if pos in visited:
        return -1
    visited.add(pos)
    if pos in intersections:
        max_steps = -1
        for path in intersections[pos]:
            max_steps = max(walk_trail(hiking_map, path[1], steps+path[3]+1, end, visited=visited.copy()), max_steps)
        return max_steps

    possible_directions = []
    for direction in directions[hiking_map[pos[0]][pos[1]]]:
        if (-direction[0], -direction[1]) == prev_dir:
            continue
        new_pos = (pos[0] + direction[0], pos[1] + direction[1])
        if not (0 <= new_pos[0] < len(hiking_map)) or not (0 <= new_pos[1] < len(hiking_map[new_pos[0]])):
            continue
        if hiking_map[new_pos[0]][new_pos[1]] == "#":
            continue
        possible_directions.append((new_pos, direction))
    max_steps = -1
    for path, d in possible_directions:
        max_steps = max(walk_trail(hiking_map, path, steps + 1, end, pos, visited.copy()), max_steps)
    return max_steps


def walk_intersections(hiking_map, pos, steps, end, prev_dir=(0, 0)):
    global intersections

    if pos in intersections or pos == end:
        return steps, pos, prev_dir

    possible = []
    for direction in directions[hiking_map[pos[0]][pos[1]]]:
        if (-direction[0], -direction[1]) == prev_dir:
            continue
        new_pos = (pos[0] + direction[0], pos[1] + direction[1])
        if not (0 <= new_pos[0] < len(hiking_map)) or not (0 <= new_pos[1] < len(hiking_map[new_pos[0]])):
            continue
        if hiking_map[new_pos[0]][new_pos[1]] == "#":
            continue
        possible.append((new_pos, direction))
    if len(possible) == 0:
        return -1, None, None

    if len(possible) > 1:
        return steps, pos, prev_dir

    return walk_intersections(hiking_map, possible[0][0], steps + 1, end, possible[0][1])


def main(input_file, stage=1):
    global directions, intersections
    with open(input_file) as file:
        hiking_map = file.readlines()
        hiking_map = [row.strip() for row in hiking_map]

        start = (0, hiking_map[0].index("."))
        end = (len(hiking_map)-1, hiking_map[-1].index("."))

        if stage == 2:
            directions[">"] = directions["."]
            directions["<"] = directions["."]
            directions["v"] = directions["."]

        for row in range(len(hiking_map)):
            for col in range(len(hiking_map[row])):
                spot = (row, col)
                if hiking_map[spot[0]][spot[1]] == "#":
                    continue
                possible_directions = []
                for direction in directions[hiking_map[spot[0]][spot[1]]]:
                    new_pos = (spot[0] + direction[0], spot[1] + direction[1])
                    if not (0 <= new_pos[0] < len(hiking_map)) or not (0 <= new_pos[1] < len(hiking_map[new_pos[0]])):
                        continue
                    if hiking_map[new_pos[0]][new_pos[1]] != "#":
                        possible_directions.append((new_pos, direction))

                if len(possible_directions) > 2:
                    intersections[spot] = []
                    for new_spot, direction in possible_directions:
                        steps, t, direction = walk_intersections(hiking_map, new_spot, 0, end, direction)
                        if t is not None:
                            intersections[spot].append((new_spot, t, direction, steps))

        print(walk_trail(hiking_map, start, 0, end))
        return

        #print_map(hiking_map)


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"
    # stage 2 runs in approximately 70-80s, not fast, maybe I'll optimize it someday
    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")
    #exit()
    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
