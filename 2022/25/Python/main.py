import time
import argparse

VERBOSE = False

snafu_symbols = {
    "2": 2,
    "1": 1, 
    "0": 0, 
    "-": -1, 
    "=": -2
}



def snafu_to_decimal(number: str) -> int: 
    global VERBOSE 

    snafu = reversed([*number])
    
    decimal = 0
    for idx, digit in enumerate(snafu): 
        decimal += (5**idx * snafu_symbols[digit]) 
    if VERBOSE: 
        print(f"{number}\t\t{decimal}")

    return decimal

def decimal_to_snafu(number: int) -> str:
    global VERBOSE

    digit = 0
    snafu = []
    
    while number > 0:
        remainder = (number ) % 5
        if 0 <= remainder <= 2: 
            snafu.append(str(remainder))
        else: 
            snafu.append("-" if (remainder == 3) else '=')

        number = number // 5
        if remainder > 2: 
            number += 1
        
    return "".join(reversed(snafu))


def main(input_file, stage=1):
    with open(input_file) as file:
        snafu_numbers = [line.strip() for line in file.readlines()]
    
    snafu_in_decimal = sum([snafu_to_decimal(snafu_number) for snafu_number in snafu_numbers])
    print(decimal_to_snafu(snafu_in_decimal))


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
