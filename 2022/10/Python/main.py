import time


def main(input_file, stage): 
    with open(input_file, "r") as file: 
        instructions = file.readlines()
        instruction_cycles = {"addx": 2, "noop": 1}
        x, cycle, signal_strengths, screen = 1, 0, 0, [[], [], [], [], [], []]
        
        # alternative approach
        # add one noop for each addx to be able to have addx in one cycle
        if False:
            instructions = ",".join(instructions).replace("addx", "noop\n,addx").split(",")
            instruction_cycles["addx"] = 1

        for instruction in instructions:
            instruction = instruction.strip("\n").split()

            for _ in range(instruction_cycles[instruction[0]]):
                if ((cycle+1)-20)%40 == 0: 
                    signal_strengths += ((cycle+1) * x)

                char = "."
                if (x-1) <= (cycle % 40) <= (x+1):
                    char = "#"
                
                screen[int(cycle / 40)].append(char)
                cycle += 1
            
            if instruction[0] == "addx": 
                x += int(instruction[1])

                
        if stage == 1:
            print(signal_strengths)
            return

        for row in screen:
            for pixel in row: 
                print(pixel, end="")
            print()


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")