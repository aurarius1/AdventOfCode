import time


class Position:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def __str__(self):
        return f"({self.y}, {self.x})"

    def __sub__(self, other):
        return Position(self.x - other.x, self.y - other.y)

    def __neg__(self):
        return Position(-self.x, -self.y)

    def __eq__(self, other):
        return self.x == other.x and self.y == other.y

    def __hash__(self):
        return hash(str(self))


pipes = {
    "|": [Position(0, -1), Position(0, 1)],
    "-": [Position(-1, 0), Position(1, 0)],
    "L": [Position(0, -1), Position(1, 0)],
    "J": [Position(0, -1), Position(-1, 0)],
    "7": [Position(0, 1), Position(-1, 0)],
    "F": [Position(0, 1), Position(1, 0)],
    ".": [],
}
dots = {1: set(), -1: set()}
loop_tile_positions = {}


def start_pipe(tile, tiles):
    global pipes
    loops = []
    # check all possible pipes
    for pipe in pipes:
        for pos in pipes[pipe]:
            if -pos in pipes.get(tiles[pos.y + tile.y][pos.x + tile.x], []):
                loops.append(pipe)

    # get element with most occurrences in array, as this is the best candidate for S
    return max(set(loops), key=loops.count)


def add_non_loop_tile(tiles, position, left_or_right=False):
    global dots, loop_tile_positions

    # check left and right of current position
    # left or right depends on direction of pipe
    for r in range(-1, 2, 2):
        x_, y_ = (position.x + r, position.y) if not left_or_right else (position.x, position.y + r)
        if y_ < 0 or y_ >= len(tiles) or x_ < 0 or x_ >= len(tiles[y_]):
            continue
        if x_ not in loop_tile_positions.get(y_, []):
            dots[r].add(Position(x_, y_))


def get_next_tile(curr_tile, pipe_directions, tiles):
    global dots, loop_tile_positions

    # iterate through all possible connections of current pipe
    for pipe_tile in pipe_directions:
        # based on the direction find every possible adjacent tile type
        possible_tiles = [k for k in pipes.keys() if -pipe_tile in pipes[k]]
        y_, x_ = curr_tile.y + pipe_tile.y, curr_tile.x + pipe_tile.x

        # if we hit start again, pipe loop is finished
        if tiles[y_][x_] == "S":
            return False, None

        # if current tile is inside possible tiles we can return, we found our next loop candidate
        if tiles[y_][x_] in possible_tiles:
            if loop_tile_positions.get(y_, None) is not None:
                loop_tile_positions[y_][x_] = tiles[y_][x_]
            else:
                loop_tile_positions[y_] = {x_: tiles[y_][x_]}
            return True, Position(x_, y_)


def main(input_file, stage=1):
    global pipes, loop_tile_positions
    with (open(input_file, "r") as file):
        tiles = [[char for char in line.strip()] for line in file.readlines()]
        loop_tiles = []

        # find start tile
        for y in range(len(tiles)):
            for x in range(len(tiles[y])):
                if tiles[y][x] == "S":
                    pos = Position(x, y)
                    loop_tile_positions[y] = {x: start_pipe(pos, tiles)}
                    loop_tiles.append(pos)
                    break

        loop = True
        while loop:
            curr_tile = loop_tiles[-1]
            pipe_directions = pipes[loop_tile_positions[curr_tile.y][curr_tile.x]]
            # prevent going loop back up
            if len(loop_tiles) > 1:
                tmp_dir = loop_tiles[-2] - curr_tile
                pipe_directions = [pipe_dir for pipe_dir in pipe_directions if pipe_dir != tmp_dir]

            loop, next_tile = get_next_tile(curr_tile, pipe_directions, tiles)
            if next_tile is not None:
                loop_tiles.append(next_tile)

        if stage == 1:
            print("loop distance:", len(loop_tiles)//2)
            return

        t = 0
        for i, loop_tile in enumerate(loop_tiles):

            # check if cw/ccw
            pos1 = loop_tiles[(i + 1) % len(loop_tiles)]
            x = pos1.x - loop_tile.x
            t += x * (pos1.y + loop_tile.y)

            # left/right should be always inside the loop (depending on orientation)
            # flipping accumulation sets, based on turns done in pipe, to have indexing
            # decoupled from orientation (only need ccw/cw once for indexing the correct set)
            # if corner is hit, two adjacent tiles need to be checked
            pipe = loop_tile_positions[loop_tile.y][loop_tile.x]
            if pipe != "|" and pipe != "-":
                add_non_loop_tile(tiles, loop_tile, not (x != 0))
                if pipe == "L" or pipe == "7":
                    dots[1], dots[-1] = dots[-1], dots[1]
            add_non_loop_tile(tiles, loop_tile, x != 0)

        # index set based on looping of pipe loop
        diff = loop_tiles[1 if t > 0 else 0] - loop_tiles[0 if t > 0 else -1]
        contained_set = dots[diff.x if diff.x != 0 else diff.y]

        # find remaining values inside pipe loop (that are not directly next to the pipe)
        filler = set()
        for v in contained_set:
            for i in range(-1, 2, 2):
                cnt = 1
                x = v.x+(i * cnt)
                while 0 <= x < len(tiles[v.y]) and x not in loop_tile_positions.get(v.y, {}):
                    filler.add(Position(x, v.y))
                    cnt += 1
                    x = v.x + (i * cnt)
        result = list(contained_set.union(filler))
        print(len(result))


if __name__ == "__main__":
    use_example = True
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")

    start_time = time.time()
    append_stage2 = True
    if use_example and append_stage2:
        file_name += "_stage2_1"

    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
