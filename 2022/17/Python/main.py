import time

rocks = [
    ["####"],
    [".#.", "###", ".#."],
    ["###", "..#", "..#"],
    ["#", "#", "#", "#"],
    ["##", "##"]

]

def blocked(r1, r2):
    for a, b in zip(r1, r2):
        if a == b and a == ".":
            continue

        xor = ord(a) ^ ord(b)
        if xor == 0:
            return True
    return False


def fall(rock, cave, height, start):
    if height == 0: 
        return False
    new_height = height - 1
    idx = rock % (len(rocks))
    rock = rocks[rock % (len(rocks))]
    # lets check if piece fits new height 
    for h in range(len(rock)):
        if (height+h) >= len(cave):
            break
        if blocked(cave[new_height+h][start:start+len(rock[h])], rock[h]): 
            return False
    return True


def blow(start, shift, current_rock, cave, height): 
    old_start = start
    start += (1 if shift == ">" else -1)
    
    if start < 0 or (start + len(current_rock[0])) > 7 :
        return old_start
    
    for h in range(len(current_rock)):
        if (height+h) >= len(cave):
            break
        if blocked(cave[height+h][start:start+len(current_rock[h])], current_rock[h]): 
            return old_start
    return start


def cave_to_file(cave):
    idx = len(cave)-1
    for i in reversed(cave):
        c += f"|{i}| {idx: 4d}\n"
        idx -= 1
    with open("cave.txt", "w") as f: 
        f.write(c)


def main(file_name, stage):
    wind = []
    with open(file_name, "r") as f: 
        wind = [*f.readline().strip()]
    cave = ["."*7 for i in range(4)]

    rock = -1
    curr_wind = 0

    total_rocks = 2022 if stage == 1 else 1000000000000

    first_symbol = wind[curr_wind]

    repetitions = {}
    heights = {}

    highest = 0
    while (rock := rock + 1) < total_rocks:
        falls = True
        current_rock = rocks[rock % (len(rocks))]
        start = 2 
        height = highest + 3
        while falls:
            start = blow(start, wind[(curr_wind) % (len(wind))], current_rock, cave, height)
            falls = fall(rock, cave, height, start)
            if falls: 
                height -= 1
            curr_wind += 1
        
        for line in current_rock:
            new_line = ""
            for i in range(len(line)):
                new_line += (cave[height][start+i] if line[i] == "." else line[i])
            cave[height] = cave[height][:start] + new_line + cave[height][start+len(line):]
            height += 1
        highest = max(highest, height)
        heights[rock] = highest

        for i in range(3):
            cave.append("."*7)


        repetition = repetitions.get((rock % (len(rocks)), (curr_wind) % (len(wind))), None)
        if repetition is None:
            repetitions[((rock % len(rocks)), (curr_wind % len(wind)))] = (rock, 0)
            continue
            
        if (rock - repetition[0]) != repetition[1]:
            repetitions[((rock % len(rocks)), (curr_wind % len(wind)))] = (rock, rock-repetition[0])
            continue

        first = repetition[0]
        cycle_length = rock - first
        cycle_height = heights[rock] - heights[first]

        off_before = (first - cycle_length)
        off_after = (total_rocks - off_before) % cycle_length
        cycles = (total_rocks - off_after - off_before) // cycle_length
        highest = cycles * cycle_height + heights[(off_after + off_before)-1]
        break

    print(highest)
    #cave_to_file(cave)


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")