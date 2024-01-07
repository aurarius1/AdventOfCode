import time
import re


def print_stacks(stacks):
    import itertools
    for items in itertools.zip_longest(*stacks, fillvalue="   "): 
        for item in items: 
            print(item, end=" ")
        print()


def main(input_file, stage): 
    with open(input_file, "r") as file: 

        init = [line.replace("\n", " ") for line in file.readlines()]
        delimiter_row = init.index(' ')
        initial_stacks, commands = init[:delimiter_row-1], init[delimiter_row+1:]
        stacks = [[] for _ in range(len(init[delimiter_row-1].split()))]
        item_size = len("[ ] ")
        for stack in initial_stacks:
            for item in range(0, len(stack), item_size):
                crate = stack[item:item+item_size].strip()
                if len(crate) != 0: 
                    stacks[item // item_size].append(crate)
                
        for command in commands: 
            amount, src, dst = map(int, re.sub("[^0-9 ]+", "", command).split())
            crates_to_move = stacks[src-1][:amount]
            del stacks[src-1][:amount]
            if stage == 1: 
                crates_to_move.reverse()
            stacks[dst-1][:0] = crates_to_move

        top = "".join([stack[0].replace("[", "").replace("]", "") for stack in stacks if len(stack) > 0])
        print(top)
        print_stacks(stacks)


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) # QNHWJVJZW
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2) # BPCZJLFJW
    print(f"Stage 2 time: {time.time() - start}")