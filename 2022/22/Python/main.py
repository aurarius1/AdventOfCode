import time
import argparse
import re
from enum import Enum

class DIRECTION(Enum):
    UP = 3
    RIGHT = 0
    DOWN = 1
    LEFT = 2


directions = {
    DIRECTION.UP: ((-1, 0), "^"),
    DIRECTION.RIGHT: ((0, 1), ">"),
    DIRECTION.DOWN: ((1, 0), "v"),
    DIRECTION.LEFT: ((0, -1), "<")
}


def wrap(start, board_map, direction, stage):
    facing = directions[direction][0]
    if stage == 1: 
        tmp = ((start[0] + facing[0]) % len(board_map), (start[1] + facing[1]) % len(board_map[0]))
        while board_map[tmp[0]][tmp[1]] == " ":
            tmp = ((tmp[0]+facing[0]) % len(board_map), (tmp[1]+facing[1]) % len(board_map[0]))
        return tmp, direction
    #print(start, bracket(facing))
    # FACE 1
    if direction == direction.LEFT and 0 <= start[0] < 50 and start[1] == 50:
        #print("1left to 5left")
        return (149-start[0], 0), DIRECTION.RIGHT
    if direction == direction.UP and start[0] == 0 and 50 <= start[1] < 100:
        #print("1top to 6left")
        return (start[1]+100, 0), DIRECTION.RIGHT
    
    # FACE 2
    if  direction == direction.UP and start[0] == 0 and 100 <= start[1] < 150:
        #print("2top to 6bottom")
        return (199, start[1]-100), direction
    if direction == direction.RIGHT and start[1] == 149 and 0 <= start[0] < 50:
        #print("2right to 4right")
        return (149-start[0], 99), DIRECTION.LEFT
    if direction == direction.DOWN and start[0] == 49 and 100 <= start[1] < 150:
        #print("2bottom to 3right")
        return (start[1]-50, 99), DIRECTION.LEFT
    
    # FACE 3
    if direction == direction.LEFT and start[1] == 50 and 50 <= start[0] < 100:
        #print("3left to 5top")
        return (100, start[0]-50), DIRECTION.DOWN
    if direction == direction.RIGHT and start[1] == 99 and 50 <= start[0] < 100:
        #print("3right to 2bottom")
        return (49, start[0]+50), DIRECTION.UP

    # FACE 4
    if direction == direction.DOWN and start[0] == 149 and 50 <= start[1] < 100:
        #print("4bottom to 6right")
        return (start[1]+100, 49), DIRECTION.LEFT
    if direction == direction.RIGHT and start[1] == 99 and 100 <= start[0] < 150: 
        #print("4right to 2right")
        return (149-start[0], 149), DIRECTION.LEFT

    # FACE 5
    if direction == direction.UP and start[0] == 100 and 0 <= start[1] < 50:
        #print("5top to 3left")
        return (start[1]+50, 50), DIRECTION.RIGHT
    if direction == direction.LEFT and start[1] == 0 and 100 <= start[0] < 150:
        #print("5left to 1left")
        return (149-start[0], 50), DIRECTION.RIGHT
    
    # FACE 6
    if direction == direction.LEFT and start[1] == 0 and 150 <= start[0] < 200: 
        #print("6left to 1top")      
        return (0, start[0]-100), DIRECTION.DOWN
    if direction == direction.RIGHT and start[1] == 49 and 150 <= start[0] < 200:
        #print("6right to 4bottom")
        return (149, start[0]-100), DIRECTION.UP
    if direction == direction.DOWN and start[0] == 199 and 0 <= start[1] < 50:
        #print("6bottom to 2top")
        return (0, start[1]+100), DIRECTION.DOWN

    return (start[0]+facing[0], start[1]+facing[1]), direction


def move(board_map, curr_instr, start, direction, stage): 
    for step in range(1, curr_instr+1):
        new_pos, new_dir = wrap(start, board_map, direction, stage)
        if board_map[new_pos[0]][new_pos[1]] == "#":
            break
        start = new_pos
        direction = new_dir
    return start, direction


def main(input_file, stage=1):
    # input parsing
    with open(input_file) as file:
        notes = [note.strip("\n") for note in file.readlines()]
        max_length = len(max(notes[:-2], key=lambda x: len(x)))
        board_map = [row.ljust(max_length) for row in notes[:-2]]
        path = [int(instr) if instr.isnumeric() else instr for instr in re.split("(L|R)", notes[-1])]

    start, direction = (0, board_map[0].index(".")), DIRECTION.RIGHT

    instr = -1
    while (instr := instr + 1) < len(path):
        curr_instr = path[instr]
        match path[instr]:
            case "R": 
                direction = DIRECTION((direction.value + 1) % len(DIRECTION))
            case "L": 
                direction = DIRECTION((direction.value - 1) % len(DIRECTION))
            case _: 
                start, direction = move(board_map, curr_instr, start, direction, stage)

    password = 1000 * (start[0]+1) + 4 * (start[1]+1) + direction.value
    print(password)


if __name__ == "__main__":
    parser = argparse.ArgumentParser(
                prog='main',
                description='AdventOfCode main skeleton',
                )
    parser.add_argument("-e", "--example", action=argparse.BooleanOptionalAction, default=False, help="Set if you want to run the example")
    parser.add_argument("-s", "--stage", action='store', default=0, help="Pass the stage you want to run", type=int)
    args = parser.parse_args()

    use_example = args.example
    file_name = "example" if use_example else "input"

    if args.stage == 1 or args.stage == 0:
        start_time = time.time()
        main(file_name, 1)
        print(f"Stage 1 time: {time.time()-start_time:.10f}")

    if args.stage == 2 or args.stage == 0:
        start_time = time.time()
        main(file_name, 2)
        print(f"Stage 2 time: {time.time()-start_time:.10f}")
