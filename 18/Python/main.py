import time
from DiggingInstruction import DiggingInstruction


def get_graph_edge(digging_map, current_col=0, current_row=0):
    graph = []
    len_graph_edge = 0
    for j, instruction in enumerate(digging_map):
        match instruction.direction:
            case "R":
                current_col += instruction.length
            case "L":
                current_col -= instruction.length
            case "D":
                current_row += instruction.length
            case "U":
                current_row -= instruction.length
        len_graph_edge += instruction.length
        graph.append((current_row, current_col))
    return graph, len_graph_edge


def main(input_file, stage=1):
    with open(input_file) as digging_map:
        digging_map = digging_map.readlines()
        digging_map = [DiggingInstruction(row.strip(), stage) for row in digging_map]
        graph, len_graph = get_graph_edge(digging_map, 0, 0)
        _sum = 0
        for i in range(len(graph)):
            v1 = graph[i]
            v2 = graph[(i+1) % len(graph)]
            _sum += v1[1]*v2[0]-v1[0]*v2[1]
        area = abs(_sum)*0.5

        i = area+1 - (len_graph/2)  # pick's theorem
        print(i+len_graph)


if __name__ == "__main__":
    use_example = False
    file = "example" if use_example else "input"

    start = time.time()
    main(file, 1)
    print(f"Stage 1 time: {time.time()-start:.10f}")

    start = time.time()
    main(file, 2)
    print(f"Stage 2 time: {time.time()-start:.10f}")
