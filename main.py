import time
import argparse


def main(input_file, stage=1):
    with open(input_file) as file:
        puzzle = file.readlines()


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
