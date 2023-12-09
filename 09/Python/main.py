import time


def main(input_file, stage=1):
    with open(input_file, "r") as file:
        lines = [[int(num) for num in line.strip().split()] for line in file.readlines()]
        history = 0
        for line in lines:
            new_line = line
            last_values = [line[-1 if stage == 1 else 0]]
            while any(x != 0 for x in new_line):
                new_line = [new_line[x+1]-new_line[x] for x in range(0, len(new_line)-1)]
                last_values.append(new_line[-1 if stage == 1 else 0])

            res = 0
            for i, last in enumerate(reversed(last_values)):
                res = last + (res if stage == 1 else -res)
            history += res
            # history += sum(last_values) if stage == 1 else [res := last - res for last in reversed(last_values)][-1]
        print(history)


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")

    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
