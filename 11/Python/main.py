import time


def get_expansion_map(curr_map):
    expansions = {k: 0 for k in range(len(curr_map))}
    for i in range(len(curr_map)):
        expansions[i] += expansions.get(i - 1, 0)
        if "#" not in curr_map[i]:
            expansions[i] += 1
    return expansions


def main(input_file, stage=1):
    with open(input_file) as file:
        galaxy_map = file.readlines()
        galaxy_map = [[*line.strip()] for line in galaxy_map]
        galaxy_map_transposed = list(zip(*galaxy_map))

        # expand galaxy map
        # expansion factor reduced by one, as if one row is empty only one row will be added, not two
        expansion_factor = 1
        if stage == 2:
            expansion_factor = 1000000-1


        # original code from stage 1, "bruteforce", just adding the rows directly
        #galaxy_map = [row for line in galaxy_map for row in (line,) * (expansion_factor if line.count("#") == 0 else 1)]
        #galaxy_map = list(zip(*galaxy_map))
        #galaxy_map = [col for line in galaxy_map for col in (line,) * (expansion_factor if line.count("#") == 0 else 1)]
        #galaxy_map = list(zip(*galaxy_map))


        # find all the galaxies
        galaxies = set()
        for i1, g1 in enumerate(galaxy_map):
            if "#" not in g1:
                continue
            # if I would not have split the input map into lists, this could also be done with regex
            for i2 in range(0, len(galaxy_map[i1])):
                if g1[i2] == "#":
                    galaxies.add((i1, i2))

        # sort by y coordinate, to allow minimal loop time later on
        galaxies = list(sorted(galaxies, key=lambda x: x[0]))

        expansions_y = get_expansion_map(galaxy_map)
        expansions_x = get_expansion_map(galaxy_map_transposed)

        distance = 0
        # minimal galaxy pair set: (len(galaxies)*(len(galaxies)-1))/2
        for i in range(len(galaxies)):
            for j in range(i+1, len(galaxies)):
                first_galaxy = galaxies[i]
                second_galaxy = galaxies[j]

                start_ = first_galaxy[0] + (expansions_y[first_galaxy[0]] * expansion_factor)
                end_ = first_galaxy[1] + (expansion_factor * expansions_x[first_galaxy[1]])

                start2_ = second_galaxy[0] + (expansions_y[second_galaxy[0]] * expansion_factor)
                end2_ = second_galaxy[1] + (expansion_factor * expansions_x[second_galaxy[1]])

                distance += abs(start_ - start2_) + abs(end_ - end2_)
        print(distance)


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")
    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
