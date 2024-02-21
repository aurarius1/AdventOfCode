import time
import math


def main(input_file, stage): 
    with open(input_file, "r") as file: 
        height_map = [line.strip() for line in file.readlines()]
        queue, distance, start, end =  [], {}, (-1, -1), (-1, -1)
        for i in range(len(height_map)):
            for j in range(len(height_map[i])):
                if height_map[i][j] == "E":
                    end = (i, j)
                    height_map[i] = height_map[i].replace("E", "z")
                if height_map[i][j] == "S":
                    start = (i, j)
                    height_map[i] = height_map[i].replace("S", "a")
                queue.append((i, j))
                distance[(i, j)] = math.inf

        if stage == 2: 
            start, end = end, start

        distance[start] = 0

        while queue: 
            position = queue.pop(queue.index(min(queue, key=lambda x: distance[x])))
            curr_height = height_map[position[0]][position[1]]
            if stage == 2 and curr_height == "a": 
                end = position
                break

            for y, x in [(0, 1), (0, -1), (1, 0), (-1, 0)]:
                new_position = (position[0]+y, position[1]+x)
                
                if new_position[0] < 0 or new_position[0] >= len(height_map) or \
                    new_position[1] < 0 or new_position[1] >= len(height_map[0]):
                    continue

                new_height = height_map[new_position[0]][new_position[1]]
                height_diff = ord(new_height) - ord(curr_height)
                if stage == 2: 
                    height_diff *= -1
                if height_diff > 1:
                    continue
                
                alt = distance[position] + 1
                if alt < distance[new_position]:
                    distance[new_position] = alt
        print(distance[end])
        

if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")