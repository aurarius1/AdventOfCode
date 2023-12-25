import time
import re
from functools import reduce
import math
directions = {}
instructions = []


def got_to_z(position, start=0):
    cnt = start
    stop = not position.endswith("Z") if start == 0 else True
    while stop:
        # L is first item in tuple, R is second
        instruction = (ord(instructions[cnt % len(instructions)]) - ord('L')) % (ord("R") - ord("L") - 1)
        position = directions[position][instruction]
        cnt += 1
        stop = not position.endswith("Z")
    if start != 0 and cnt-start != start:
        print("LOOP TIME NOT EQUAL:", (cnt-start))

    return cnt-start, position


def main(input_file, stage=1):
    global directions, instructions
    with open(input_file, "r") as file:
        maps = file.readlines()
        instructions, nodes = maps[0].strip(), [node.strip().replace(" ", "").split("=")for node in maps[2:]]
        directions = {k: re.sub(r"\(|\)", "", v).split(",") for k, v in nodes}
        positions = [direction for direction in directions.keys() if direction.endswith("A" if stage == 2 else "AAA")]
        steps = [got_to_z(position) for position in positions]

        # check how long it takes for the pattern to repeat to z
        steps = [got_to_z(end, start)[0] for start, end in steps]

        print(reduce(lambda x, y: math.lcm(x, y), steps))


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1) # sol: 20659
    print(f"Stage 1 time: {time.time()-start_time:.10f}")


    start_time = time.time()
    if use_example:
        file_name += "_stage2"
    main(file_name, 2) # sol: 249204891
    print(f"Stage 2 time: {time.time()-start_time:.10f}")

