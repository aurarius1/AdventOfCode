import time
import re


# Overloaded tuple for easier operations
class Position(tuple):

    def __rmul__(self, other):
        return Position(other * x for x in self)

    def __add__(self, other):
        return Position(x+y for x, y in zip(self, other))

    def __sub__(self, other):
        return Position(abs(x - y) for x, y in zip(self, other))

    def __eq__(self, other):
        return self[0] == other[0] and self[1] == other[1]

    def __hash__(self):
        return super().__hash__()

    def abs(self):
        return Position((abs(self[0]), abs(self[1])))


def print_garden_plot(plot, out_file=""):
    if out_file == "":
        return
    with open(out_file, "w") as f:
        for row in plot:
            f.write(f"{''.join(row)}\n")


def step_possible(hashmap, step, endpoint, direction, plots, curr):
    if plots[curr[0]][curr[1]] == "#":
        return False

    # corner point
    if abs(endpoint[0]) == step:
        t = Position((direction[0], 0))
        return 0 <= hashmap[step - 1][curr + t][1] < step
    elif abs(endpoint[1]) == step:
        t = Position((0, direction[1]))
        return 0 <= hashmap[step - 1][curr + t][1] < step

    # if we are below the gardener, we need to check above, otherwise flipped
    down_mod = -1 if curr[0] > (len(plots)//2) else 1
    # if we are on the right side of the gardener we need to check to the left, otherwise flipped
    right_mod = -1 if curr[1] > (len(plots[0]) // 2) else 1
    for t in [Position((down_mod, 0)), Position((0, right_mod))]:
        if 0 <= hashmap[step - 1][curr + t][1] <= step:
            return True, False

    return False


def main2(garden_plots, gardener, steps, replacements, out_file=""):
    # sorted by corners ccw, to allow edge direction calculation later on
    directions = [Position((1, 0)), Position((0, -1)), Position((-1, 0)), Position((0, 1))]
    lookup_table = {i: ({} if i > 0 else {gardener: ["S", 0]}) for i in range(steps+1)}

    for i in range(1, steps+1):
        for e in range(len(directions)):
            endpoint = i*directions[e]
            endpoint2 = i*directions[(e + 1) % (len(directions))]

            # edge goes along this direction
            direction = Position(((endpoint[0] * -1 + endpoint2[0])//i, (endpoint2[1] + endpoint[1] * -1)//i))

            while endpoint != endpoint2:
                curr = gardener + endpoint
                if not (0 <= curr[0] < len(garden_plots)) or not (0 <= curr[1] < len(garden_plots[curr[0]])):
                    endpoint = endpoint + direction
                    continue
                step_init = -2 if garden_plots[curr[0]][curr[1]] == "#" else -1
                lookup_table[i][curr] = [garden_plots[curr[0]][curr[1]], step_init]
                if not step_possible(lookup_table, i, endpoint, direction, garden_plots, curr):
                    endpoint = endpoint + direction
                    continue
                lookup_table[i][curr][1] = i
                lookup_table[i][curr][0] = str(i % 10)
                garden_plots[curr[0]][curr[1]] = replacements[i % 2]
                endpoint = endpoint + direction

    # fill gaps, that were blocked by # in the first iteration
    # do this until there are no changes left
    changes = 1
    while changes != 0:
        changes = 0
        for key, nodes in lookup_table.items():
            for node in nodes:
                if lookup_table[key][node][1] != -1:
                    continue
                for direction in directions:
                    curr = node + direction
                    t = lookup_table.get(key-1, {}).get(curr, None)
                    for i in range(key, steps+1):
                        if t is not None:
                            break
                        t = lookup_table.get(i, {}).get(curr, None)

                    if t is None or not str.isdigit(t[0]) or t[1]+1 > steps:
                        continue
                    lookup_table[key][node] = [str((t[1]+1)%10), t[1]+1]
                    garden_plots[node[0]][node[1]] = replacements[(t[1]+1)%2]
                    changes += 1
    max_plots_even = 1              # start at one for S
    max_plots_odd = 0

    for row in range(len(garden_plots)):
        max_plots_even += garden_plots[row].count(replacements[0])
        max_plots_odd += garden_plots[row].count(replacements[1])

    print_garden_plot(garden_plots, out_file=out_file)
    return max_plots_even, max_plots_odd


def main(input_file, stage=1):

    steps = 6 if input_file == "example" else 64

    with open(input_file) as f:
        garden_plots = [[*row.strip()] for row in f.readlines()]
        gardener = Position(((len(garden_plots)-1)//2, (len(garden_plots[0])-1)//2))

        if stage == 1:
            replacements = ["E", "O"]
            even, odd = main2(garden_plots, gardener, steps, replacements, "grow.txt")
            print(even)
            assert even == 3743
            return

        if input_file == "example":
            steps = 5
            elf_max_steps = 50
            print("Stage 2 does not work with the example")
            return
        else:
            steps = 65
            elf_max_steps = 26501365

        replacements = ["E", "O"]
        diamond_steps = main2(garden_plots.copy(), gardener, steps, replacements)

        steps = len(garden_plots[0])
        full_steps = main2(garden_plots, gardener, steps, replacements)
        n = elf_max_steps // steps


        corner_even = full_steps[0] - diamond_steps[0]
        corner_odd = full_steps[1] - diamond_steps[1]



        t = (n + 1) ** 2 * full_steps[1] + n ** 2 * full_steps[0] + n * corner_even - ((n + 1) * corner_odd)
        print(t)
        assert t == 618261433219147


if __name__ == "__main__":
    use_example = True
    file = "example" if use_example else "input"

    start = time.time()
    main(file, 1)
    print(f"Stage 1 time: {time.time()-start:.10f}")

    start = time.time()
    main(file, 2)
    print(f"Stage 2 time: {time.time()-start:.10f}")
