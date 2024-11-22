import time
import argparse
from typing import Tuple, Optional, Dict, List

VERBOSE = False

class Elf: 
    def __init__(self, position: Tuple[int, int], proposed_move: Optional[Tuple[int, int]] = None):
        self._position = position
        self._proposed_move = proposed_move if proposed_move is not None else position
    
    @property
    def position(self) -> Tuple[int, int]:
        return self._position
    
    @position.setter
    def position(self, new_position: Tuple[int, int]):
        self._position = new_position
    
    @property 
    def proposed_move(self) -> Optional[Tuple[int, int]]:
        return self._proposed_move
    
    @proposed_move.setter
    def proposed_move(self, new_move: Optional[Tuple[int, int]]):
        self._proposed_move = new_move
    
    def __str__(self) -> str: 
        return f"Elf at {self._position} proposes move to {self._proposed_move}"

    def __repr__(self) -> str:
        return self.__str__()


def all_free(positions: Dict[Tuple[int, int], List[int]], elf_position: Tuple[int, int], d1: Tuple[int, int], d2: Tuple[int, int], d3: Tuple[int, int]): 
    if positions.get((elf_position[0]+d1[0], elf_position[1]+d1[1])) is not None: 
        return False
    if positions.get((elf_position[0]+d2[0], elf_position[1]+d2[1])) is not None: 
        return False
    if positions.get((elf_position[0]+d3[0], elf_position[1]+d3[1])) is not None: 
        return False
    return True


def print_board(positions, elves):
    global VERBOSE
    cols = [position[1] for position in positions]
    rows = [position[0] for position in positions]
    min_cols, max_cols = min(cols), max(cols)+1
    min_rows, max_rows = min(rows), max(rows)+1

    empty_ground = 0

    for row in range(min_rows, max_rows):
        for col in range(min_cols, max_cols):
            if positions.get((row, col)) is not None: 
                if VERBOSE: 
                    print("#", end="")
            else: 
                empty_ground += 1
                if VERBOSE: 
                    print(".", end="")
        if VERBOSE: 
            print()
    return empty_ground

def main(input_file, stage=1):
    global VERBOSE
    max_rows, max_cols = 0, 0
    min_rows, min_cols = 0, 0
    elves = []
    positions = {}
    with open(input_file) as file:
        puzzle = [row.strip() for row in file.readlines()]
        max_rows = len(puzzle)
        max_cols = len(puzzle[1])

        elf = 0
        for r_idx, row in enumerate(puzzle): 
            for c_idx, col in enumerate(row): 
                if col == "#":
                    elves.append(Elf((r_idx, c_idx))) # second el
                    positions[elves[elf].position] = elf
                    elf += 1
    directions = [
        ((-1, 0), (-1, 1), (-1, -1)), # N
        ((1, 0), (1, 1), (1, -1)), # S
        ((0, -1), (-1, -1), (1, -1)), # W
        ((0, 1), (1, 1), (-1, 1)), #E
    ]
    proposed_moves = {}

    if VERBOSE: 
        print_board(positions, elves)

    rounds = 10
    curr_round = 0
    should_continue = lambda: (curr_round < rounds) if stage == 1 else True
    while should_continue():
        # first half of round
        for elf_id, elf in enumerate(elves): 

            # check if elf does anything in this round
            for d1, d2, d3 in directions: 
                if not all_free(positions, elf.position, d1, d2, d3):
                    break
            else: 
                elf.proposed_move = elf.position
                continue

            if VERBOSE: 
                print(f"elf {elf_id} Starting direction: {(curr_round+cnt) % len(directions)}")
            
            # get the proposed move
            for cnt in range(len(directions)):
                d1, d2, d3 = directions[(curr_round+cnt) % len(directions)]
                if not all_free(positions, elf.position, d1, d2, d3):
                    continue
                elf.proposed_move = (elf.position[0] + d1[0], elf.position[1] + d1[1])
                if proposed_moves.get(elf.proposed_move) is None: 
                    proposed_moves[elf.proposed_move] = 0
                proposed_moves[elf.proposed_move] += 1
                break
        
        # second half of round
        moved = 0
        for elf_id, elf in enumerate(elves): 
            if elf.proposed_move == elf.position: 
                continue
            moved += 1
            if proposed_moves[elf.proposed_move] == 1: 
                del positions[elf.position]
                positions[elf.proposed_move] = elf_id
                elf.position = elf.proposed_move
            
            elf.proposed_move = elf.position
        
        proposed_moves = {}
        if VERBOSE: 
            print(curr_round)
            print_board(positions, elves)
            
        if stage == 2 and moved == 0: 
            breaks
        
        curr_round += 1
    
    if stage == 2: 
        print(curr_round + 1)
    else: 
        print(print_board(positions, elves))


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
