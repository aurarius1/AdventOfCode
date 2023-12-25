import time


class Direction:
    RIGHT = (0, 1)
    LEFT = (0, -1)
    UP = (-1, 0)
    DOWN = (1, 0)


def mirror_ray(mirror, direction, curr_pos):
    # could be done this way, but is not that readable
    #if direction[0] != 0:
    #    if mirror == "/":
    #        return (curr_pos[0], curr_pos[1] - direction[0]), (0, -direction[0])
    #    if mirror == "\\":
    #        return (curr_pos[0], curr_pos[1] + direction[0]), (0, direction[0])

    match direction:
        case Direction.UP:
            if mirror == "/":
                return (curr_pos[0], curr_pos[1]+1), Direction.RIGHT
            if mirror == "\\":
                return (curr_pos[0], curr_pos[1]-1), Direction.LEFT
        case Direction.DOWN:
            if mirror == "/":
                return (curr_pos[0], curr_pos[1]-1), Direction.LEFT
            if mirror == "\\":
                return (curr_pos[0], curr_pos[1]+1), Direction.RIGHT
        case Direction.RIGHT:
            if mirror == "/":
                return (curr_pos[0]-1, curr_pos[1]), Direction.UP
            if mirror == "\\":
                return (curr_pos[0]+1, curr_pos[1]), Direction.DOWN
        case Direction.LEFT:
            if mirror == "/":
                return (curr_pos[0] + 1, curr_pos[1]), Direction.DOWN
            if mirror == "\\":
                return (curr_pos[0] - 1, curr_pos[1]), Direction.UP


def trace_ray(grid, start, direction=Direction.RIGHT, cache=None):
    # BFS could make this way more efficient
    if cache is None:
        cache = []

    warm_pos = []

    if start in cache:
        return warm_pos

    rng = range(0, 0, 1)
    match direction:
        case Direction.RIGHT:
            rng = range(start[1], len(grid[0]), 1)
        case Direction.LEFT:
            rng = range(start[1], -1, -1)
        case Direction.DOWN:
            rng = range(start[0], len(grid), 1)
        case Direction.UP:
            rng = range(start[0], -1, -1)

    for i in rng:
        if direction[1] != 0:
            curr_pos = (start[0], i)
        else:
            curr_pos = (i, start[1])

        if curr_pos in cache:
            return warm_pos

        curr_space = grid[curr_pos[0]][curr_pos[1]]
        warm_pos.append(curr_pos)

        if curr_space == "|" and direction[1] != 0:
            cache.append(curr_pos)
            warm_pos.extend(trace_ray(grid, (curr_pos[0] + 1, curr_pos[1]), Direction.DOWN, cache))
            warm_pos.extend(trace_ray(grid, (curr_pos[0] - 1, curr_pos[1]), Direction.UP, cache))
            return warm_pos
        elif curr_space == "-" and direction[0] != 0:
            cache.append(curr_pos)
            warm_pos.extend(trace_ray(grid, (curr_pos[0], curr_pos[1] - 1), Direction.LEFT, cache))
            warm_pos.extend(trace_ray(grid, (curr_pos[0], curr_pos[1] + 1), Direction.RIGHT, cache))
            return warm_pos
        elif curr_space in ["/", "\\"]:
            next_start, new_dir = mirror_ray(curr_space, direction, curr_pos)
            warm_pos.extend(trace_ray(grid, next_start, new_dir, cache))
            return warm_pos
    return warm_pos


def main(input_file, stage=1):
    with open(input_file) as file:
        puzzle = file.readlines()
        grid = [row.strip() for row in puzzle]
        if stage == 1:
            res = len(set(list(trace_ray(grid, (0, 0)))))
            print(res)
            return

        max_energized = 0
        # up and down
        for i in range(len(grid[0])):
            up_down = len(set(list(trace_ray(grid, (0, i), Direction.DOWN))))
            down_up = len(set(list(trace_ray(grid, (0, len(grid[0])-1-i), Direction.UP))))
            max_energized = max(max_energized, up_down, down_up)

        # left and right
        for i in range(len(grid)):
            left_right = len(set(list(trace_ray(grid, (i, 0), Direction.RIGHT))))
            right_left = len(set(list(trace_ray(grid, (len(grid)-1-i, 0), Direction.LEFT))))
            max_energized = max(max_energized, left_right, right_left)

        print(max_energized)


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")


    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
