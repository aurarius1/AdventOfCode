import time


def print_cave(cave, stage=1):
    min_x = min([key[1] for key in cave.keys()])
    max_x = max([key[1] for key in cave.keys()])

    min_y = min([key[0] for key in cave.keys()])
    max_y = max([key[0] for key in cave.keys()])


    padding = 0 if stage == 1 else 2


    for y in range(min_y, max_y+1):
        for x in range(min_x-padding, max_x+1+padding):
            print(cave.get((y, x), "."), end="")
        print()
    if stage == 2:
        print("#" * (max_x+1-min_x + 2*padding))


def normalize(start, end):
    if int(start-end) == 0: 
        return 0
    return int((start-end) // abs(start-end))


def get(cave, position, stage, max_y):
    if stage == 2 and position[0] == max_y:
        return "#"
    
    return cave.get(position, ".")


def main(input_file, stage): 
    with open(input_file, "r") as file: 
        sand_entry = (0, 500)
        max_y, max_x, min_x = 0, 0, sand_entry[1]
        cave = {}

        for scan in file.readlines():
            formation = [(int(rock.split(",")[1]), int(rock.split(",")[0]))  for rock in scan.strip().split(" -> ")]
            max_x = max(max_x, max(formation, key=lambda x: x[1])[1])
            min_x = min(min_x, min(formation, key=lambda x: x[1])[1])
            max_y = max(max_y, max(formation, key=lambda x: x[0])[0])    

            for start in range(len(formation)-1):
                end, start = formation[start+1], formation[start]
                direction = (normalize(end[0], start[0]), normalize(end[1], start[1]))
                while start != end:
                    cave[start] = "#"
                    start = (start[0]+direction[0], start[1]+direction[1])
                cave[end] = "#"    
            
        if stage == 2: 
            max_y += 2

        # setup complete
        overflowing = False  
        sand_units = 0

        while not overflowing and get(cave, sand_entry, stage, max_y) != "o":
            sand = sand_entry
            while True: 
                sand = (sand[0]+1, sand[1])
                if sand[0] > max_y:
                    overflowing = True
                    break 
                
                if get(cave, sand, stage, max_y)  == ".":
                    continue
            
                left = sand[1]-1
                if stage == 1 and left < min_x: 
                    overflowing = True
                    break
                if get(cave, (sand[0], left), stage, max_y)  == ".":
                    sand = (sand[0], left)
                    continue

                right = sand[1]+1
                if stage == 1 and right > max_x: 
                    overflowing = True
                    break
                if get(cave, (sand[0], right), stage, max_y) == ".":
                    sand = (sand[0], right)
                    continue
                
                cave[(sand[0]-1, sand[1])] = "o"
                sand_units += 1
                break
        
        #print_cave(cave, stage)
        print(sand_units)



if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")