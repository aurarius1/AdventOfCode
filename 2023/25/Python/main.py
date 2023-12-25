import time
import random

class Component:
    def __init__(self):
        pass



def krager_min_cut(edges, vertices):
    while len(vertices) > 2:
        rand_edge = random.randint(0, len(edges)-1)
        edge = edges[rand_edge]
        vertices.remove(edge[0])
        vertices.remove(edge[1])
        vertices.add(("".join(edge)))

        i = 0
        while i < len(edges):
            first = edges[i][0]
            second = edges[i][1]
            if edge[0] == edges[i][0] or edge[1] == edges[i][0]:
                first = "".join(edge)
            if edge[0] == edges[i][1] or edge[1] == edges[i][1]:
                second = "".join(edge)
            if first == second:
                edges.pop(i)
                continue
            edges[i] = (first, second)
            i += 1
    return len(edges), vertices


def main(input_file, stage=1):
    if stage == 2:
        print("PUSH THE BUTTON")
        return


    with open(input_file) as file:
        puzzle = file.readlines()
        do_vis = False
        if do_vis:
            from graphviz import Source
            import os
            if os.path.exists("test.gv"):
                os.remove("test.gv")
            s = Source.from_file("./graph.dot")
            s.engine = "neato"
            s.render("test.gv", format="png", view=True)

        components = [row.strip() for row in puzzle]
        edges = []
        vertices = set()
        for component in components:
            left, right = component.split(": ")
            vertices.add(left)
            for right in right.split():
                vertices.add(right)
                edges.append((left, right))

        # sit back and relax, this might take a while
        while True:
            num_cuts, v = krager_min_cut(edges.copy(), vertices.copy())
            print(num_cuts)
            if num_cuts == 3:
                v = list(v)
                print(len(v[0])/3 * len(v[1])/3)
                break


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")

    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
