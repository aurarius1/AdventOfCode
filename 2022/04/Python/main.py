import time

def main(input_file, stage): 
    with open(input_file, "r") as file: 
        # if we are asking which groups are overlapping the second item for elf1 needs to be smaller 
        # or equal to the second item for elf 2. if we are asking for contained only the first item of
        # elf1 needs to be smaller or equal to the second item for elf 2 (basically: if the range is partially
        # included the first item needs to in range of the second elf's range)
        contained_or_overlap = 1 if stage == 1 else 0 

        cnt = 0
        for assignment in file.readlines(): 
            elf1, elf2 = assignment.strip().split(",")
            elf1, elf2 = (elf1.split("-"), elf2.split("-"))
            elf1, elf2 = (int(elf1[0]), int(elf1[1])), (int(elf2[0]), int(elf2[1]))
            if (elf1[0] >= elf2[0] and elf1[contained_or_overlap] <= elf2[1]) or \
                (elf2[0] >= elf1[0] and elf2[contained_or_overlap] <= elf1[1]): 
                cnt += 1
        print(cnt)


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1)  # result: 605
    print(f"Stage 1 time: {time.time() - start}")
    start = time.time()
    main(file_name, 2)  # result: 914
    print(f"Stage 2 time: {time.time() - start}")