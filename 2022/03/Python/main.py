import time

def main(input_file, stage): 
    with open(input_file, "r") as file: 
        rucksacks = [rucksack.strip() for rucksack in file.readlines()]
        dupes = []
        if stage == 1: 
            for rucksack in rucksacks: 
                front, back = rucksack[:len(rucksack)//2], rucksack[len(rucksack)//2:]
                dupe = None
                for item in front: 
                    if item in back: 
                        dupes.append(item)
                        break
        else: 
            for i in range(0, len(rucksacks), 3):
                b_elf1, b_elf2, b_elf3 = rucksacks[i], rucksacks[i+1], rucksacks[i+2]
                for item in b_elf1: 
                    if item in b_elf2 and item in b_elf3: 
                        dupes.append(item)
                        break
                

        item_priorities = 0
        for dupe in dupes: 
            if dupe is not None: 
                item_priorities += ord(dupe) - (ord("`") if str.islower(dupe) else ord("&"))       
        print(item_priorities)

if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time() - start}")