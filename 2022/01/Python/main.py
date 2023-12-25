import time


def main(input_file, stage=1):
    with open(input_file) as file:
        file_content = file.readlines()
        elves = [[]]
        for i in file_content:
            if i == "\n":
                elves.append([])
                continue
            elves[-1].append(int(i.strip()))
        calories_per_elf = sorted([sum(calories) for calories in elves])
        print(sum(calories_per_elf[-3 if stage == 2 else -1:]))


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")
    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")

