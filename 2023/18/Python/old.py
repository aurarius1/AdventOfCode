import time
from DiggingInstruction import DiggingInstruction


def write_trench(trench):
    with open("trench.txt", "w") as f:
        for row in trench:
            f.write("".join(row) + "\n")


def get_map_range(digging_map, directions):
    max_r = 0
    min_r = digging_map[0].length
    d = 0
    for i in digging_map:
        if i.direction not in directions:
            continue
        d += i.length * (-1 if i.direction == directions[1] else 1)
        max_r = max(max_r, d)
        min_r = min(min_r, d)
    return max_r - min_r + 1, min_r * (-1 if min_r < 0 else 1)


def create_trench(digging_map, width, height, current_col=0, current_row=0):
    trench = [["." for j in range(width)] for i in range(height)]
    path = {}
    pos = (current_row, current_col)
    for j, instruction in enumerate(digging_map):
        directions = {"R": (0, 1), "D": (1, 0), "U": (-1, 0), "L": (0, -1)}
        direction = directions[instruction.direction]
        for i in range(1, instruction.length + 1):
            trench[pos[0] + i * direction[0]][pos[1] + i * direction[1]] = "#"
            path[(pos[0] + i * direction[0], pos[1] + i * direction[1])] = instruction.direction
        pos = (pos[0] + instruction.length*direction[0], pos[1] + instruction.length*direction[1])
        if j+1 >= len(digging_map):
            continue
        match instruction.direction + digging_map[j + 1].direction:
            case "RU":
                path[pos] = "RU"
            case "DR":
                path[pos] = "DR"
            case "LD":
                path[pos] = "LD"
            case "UL":
                path[pos] = "UL"
    return trench, path


def main(input_file, stage=1):
    with open(input_file) as file:
        digging_map = file.readlines()
        digging_map = [DiggingInstruction(row.strip(), stage) for row in digging_map]

        width, min_w = get_map_range(digging_map, ["R", "L"])
        height, min_h = get_map_range(digging_map, ["D", "U"])
        trench, graph = create_trench(digging_map, width, height, min_w, min_h)
        cubes = len(graph.keys())

        for g in graph.keys():
            for direction in graph[g]:
                if direction == "R" or direction == "U":
                    direction = (1, 0) if direction == "R" else (0, 1)
                    def stop(s): return s[0] < len(trench) and s[1] < len(trench[s[0]])
                else:
                    direction = (-1, 0) if direction == "L" else (0, -1)
                    def stop(s): return s[0] >= 0 and s[1] >= 0

                start = (g[0] + direction[0], g[1] + direction[1])
                while stop(start):
                    if start in graph.keys():
                        break
                    if trench[start[0]][start[1]] != "#":
                        trench[start[0]][start[1]] = "#"
                        cubes += 1
                    start = (start[0] + direction[0], start[1] + direction[1])
        write_trench(trench)
        print(cubes)


if __name__ == "__main__":
    use_example = False
    file = "example" if use_example else "input"

    start_time = time.time()
    main(file, 1)
    print(f"Stage 1 time: {time.time() - start_time:.10f}")
    print("THIS IS TO SLOW FOR STAGE 2")
