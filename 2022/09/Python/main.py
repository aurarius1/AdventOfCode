import time
from math import sqrt, floor


def print_rope(rope, is_tail=False):
    min_y = min(0, min(rope, key=lambda x: x[0])[0])
    min_x = min(0, min(rope, key=lambda x: x[1])[1])

    max_y = max(0, max(rope, key=lambda x: x[0])[0])
    max_x = max(0, max(rope, key=lambda x: x[1])[1])
    print((min_y, min_x), (max_y, max_x))


    for i in range(min_y, max_y+1):
        for j in range(min_x, max_x+1):
            if (i, j) == (0, 0):
                print("s", end="")
                continue
            if (i, j) in rope:
                print(rope.index((i, j)) if not is_tail else "#", end="")
            else:
                print(".", end="")
        print("")


def main(input_file, stage): 
    with open(input_file, "r") as file: 
        movements = [line.strip("\n").split() for line in file.readlines()]
        rope = [(0, 0) for _ in range(2 if stage == 1 else 10)]
        tail_positions = {rope[-1]}
        directions = {"R": (0, 1), "L": (0, -1), "U": (-1, 0), "D": (1, 0)}
        for direction, steps in movements: 
            direction = directions[direction]
            for _ in range(int(steps)):
                rope[0] = (rope[0][0]+direction[0], rope[0][1]+direction[1])
                for curr_knot in range(1, len(rope)):
                    y_dist = rope[curr_knot-1][0] - rope[curr_knot][0]
                    x_dist = rope[curr_knot-1][1] - rope[curr_knot][1]

                    distance = floor(sqrt(y_dist*y_dist+x_dist*x_dist))
                    if distance <= 1: 
                        break

                    y_dist = (y_dist / abs(y_dist)) if y_dist != 0 else 0
                    x_dist = (x_dist / abs(x_dist)) if x_dist != 0 else 0
                    rope[curr_knot] = (rope[curr_knot][0] + y_dist, rope[curr_knot][1] + x_dist)
                tail_positions.add(rope[-1])
        print(len(tail_positions))


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")

    if use_example: 
        file_name += "2"

    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")