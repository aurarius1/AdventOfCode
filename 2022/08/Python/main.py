import time


def main(input_file, stage): 

    with open(input_file, "r") as file: 
        forrest = [line.strip("\n") for line in file.readlines()]
        directions = [(-1, 0), (1, 0), (0, 1), (0, -1)]

        visible = 0
        max_scenic_score = 0
        for i in range(0, len(forrest)): 
            for j in range(0, len(forrest[i])): 
                if i == 0 or i == len(forrest)-1: 
                    visible += 1
                    continue
                if j == 0 or j == len(forrest[i])-1:
                    visible += 1
                    continue
                scenic_score = 1
                for direction in directions: 
                    position = (i+direction[0], j+direction[1])
                    is_visible = False
                    viewing_distance = 1
                    while forrest[position[0]][position[1]] < forrest[i][j]:
                        
                        if position[0] == 0 or position[0] == len(forrest)-1:
                            visible += 1
                            is_visible = True
                            break
                        if position[1] == 0 or position[1] == len(forrest[0])-1: 
                            visible += 1
                            is_visible = True
                            break
                        viewing_distance += 1
                        position = (position[0] + direction[0], position[1] + direction[1])
                    if stage == 2: 
                        scenic_score *= viewing_distance
                        continue

                    if is_visible: 
                        break
                max_scenic_score = max(scenic_score, max_scenic_score)
        if stage == 2: 
            print(max_scenic_score)
        else:
            print(visible)


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")