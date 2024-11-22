import time
import argparse
from typing import Tuple, Dict, List
from enum import Enum
from math import lcm
import heapq

VERBOSE = False


class Blizzard(): 
    def __init__(self, position: Tuple[int, int], direction: Tuple[int, int]): 
        self._position = position
        self._direction = direction
    
    @property
    def position(self) -> Tuple[int, int]:
        return self._position
    
    @property
    def direction(self) -> str: 
        match self._direction: 
            case (0, 1): 
                return ">"
            case (0, -1): 
                return "<"
            case (-1, 0):
                return "^"
            case (1, 0):
                return "v"
        return ""
    
    def move(self, timesteps: int, height: int, width: int) -> Tuple[int, int]:

        y = self._position[0] - 1 + self._direction[0]*timesteps
        x = self._position[1] - 1 + self._direction[1]*timesteps

        y = y % (height - 2)
        x = x % (width - 2)

        return y + 1, x + 1

    def __str__(self) -> str: 
        return f"Blizzard {self.id} at position {self._position} moving {directions[self._direction][1]}"

    def __repr__(self) -> str: 
        return self.__str__()


def print_map(
    blizzards: List[Blizzard], 
    area: Dict[Tuple[int, int], List[int]], 
    height: int, 
    width: int,
    curr_pos: Tuple[int, int]
):
    global VERBOSE
    if not VERBOSE: 
        return
    for row in range(0, height): 
        for col in range(0, width):
            if (row == 0 and col != 1) or (row == height-1 and col != width-2) or col == 0 or col == width -1:
                print("#", end="") 
                continue
             
            pos = (row, col)
            if pos == curr_pos: 
                print("E", end="")
                continue

            field = area.get(pos, [])
            match len(field): 
                case 0: 
                    print(".", end="")
                case 1: 
                    print(blizzards[field[0]].direction, end="")
                case _: 
                    print(len(field), end="")
        print()


def solve(
    blizzards: List[Blizzard], 
    start_pos: Tuple[int, int], 
    exit_pos: Tuple[int, int],  
    field_height: int, 
    field_width: int, 
    minutes: int = 0,
) -> int: 
    queue = [(minutes, start_pos)]
    visited = set()
    areas = dict()
    
    while True:
        minutes, curr_pos = heapq.heappop(queue)
        if curr_pos == exit_pos:
            timestamp = minutes % lcm(field_height-2, field_width-2)
            print_map(blizzards, areas.get(timestamp), field_height, field_width, curr_pos)
            #print_map(blizzards, areas.get(minutes), field_height, field_width, curr_pos)
            return minutes
        minutes += 1
        if (curr_pos, minutes) in visited:
            continue
        visited.add((curr_pos, minutes))

        # advance blizzard map 
        # the map layout repeats after a multiple of field_height -2 and field_width -2
        timestamp = minutes % lcm(field_height-2, field_width-2)
        area = areas.get(timestamp, None)
        if area is None: 
            areas[timestamp] = {}
            for blizzard_idx, blizzard in enumerate(blizzards): 
                new_pos = blizzard.move(minutes, field_height, field_width)
                if areas[timestamp].get(new_pos) is None: 
                    areas[timestamp][new_pos] = []
                areas[timestamp][new_pos].append(blizzard_idx)
  
        for direction in [(0, 0), (1, 0), (-1, 0), (0, 1), (0, -1)]: 
            next_pos = (curr_pos[0]+direction[0], curr_pos[1]+direction[1])

            if (not (0 < next_pos[0] < field_height-1) or not (0 < next_pos[1] < field_width-1)) and next_pos != start_pos and next_pos != exit_pos: 
                continue
            if len(areas[timestamp].get(next_pos, [])) > 0:
                continue
            heapq.heappush(queue, (minutes, next_pos))
    

def main(input_file, stage=1):

    blizzards: List[Blizzard] = []
    start_pos, exit_pos = (0, 1), (0, 0)
    field_height, field_width = 0, 0

    with open(input_file) as file:
        puzzle = [line.strip() for line in file.readlines()]
        field_height = len(puzzle)
        field_width = len(puzzle[0])
        exit_pos = (field_height-1, field_width - 2)
        for row_idx, row in enumerate(puzzle): 
            for col_idx, col in enumerate(row): 
                match col: 
                    case ">": 
                        direction = (0, 1)
                    case "^": 
                        direction = (-1, 0)
                    case "<": 
                        direction = (0, -1)
                    case "v":
                        direction = (1, 0)
                    case _: 
                        continue
                blizzards.append(Blizzard((row_idx, col_idx), direction))
        

    
        time = solve(blizzards, start_pos, exit_pos, field_height, field_width, 0)
        
        # 308
        if stage == 1: 
            print(time)
            return
        
        back_to_start = solve(blizzards, exit_pos, start_pos, field_height, field_width, time)
        back_to_exit = solve(blizzards, start_pos, exit_pos, field_height, field_width, back_to_start)

        # 908
        print(back_to_exit)


if __name__ == "__main__":
    parser = argparse.ArgumentParser(
                prog='main',
                description='AdventOfCode main skeleton',
                )
    parser.add_argument("-e", "--example", action=argparse.BooleanOptionalAction, default=False, help="Set if you want to run the example")
    parser.add_argument("-s", "--stage", action='store', default=0, help="Pass the stage you want to run", type=int)
    parser.add_argument("-v", "--verbose", action=argparse.BooleanOptionalAction, default=False, help="Pass the stage you want to run")

    args = parser.parse_args()
    VERBOSE = args.verbose


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
