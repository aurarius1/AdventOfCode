import time
import functools
import math


def get_coords(x, y, z):
    return (int(x), int(y), int(z))


def get_surface_area(cubes): 
    for cube in cubes.keys():
        for i in range(-1, 2, 2):
            shift_x = (cube[0]+i, cube[1], cube[2])
            shift_y = (cube[0], cube[1]+i, cube[2])
            shift_z = (cube[0], cube[1], cube[2]+i)
            if shift_x in cubes.keys():
                cubes[cube].add(shift_x)
            if shift_y in cubes.keys():
                cubes[cube].add(shift_y)
            if shift_z in cubes.keys():
                cubes[cube].add(shift_z)

    surface_area = sum([6 - len(cubes[cube]) for cube in cubes.keys()])
    return surface_area


def min_max(coordinates, axis): 
    return (min(coordinates, key=lambda t: t[axis])[axis], max(coordinates, key=lambda t: t[axis])[axis])


def build_bounding_box(coordinates):
    x = min_max(coordinates, 0)
    y = min_max(coordinates, 1)
    z = min_max(coordinates, 2)
    bounding_box = []
    for _z in range(z[0]-1, z[1]+2):
        for _x in range(x[0]-1, x[1]+2):
            for _y in range(y[0]-1, y[1]+2):
                if (_x, _y, _z) in coordinates:
                    continue
                bounding_box.append((_x, _y, _z))
    return bounding_box, (x[0]-1, y[0], z[0])


def add_contained_air(cubes):
    bounding_box, start = build_bounding_box(list(cubes.keys()))
    queue = [start]
    while queue: 
        position = queue.pop()
        for direction in [(0, 1, 0), (0, -1, 0), (1, 0, 0), (-1, 0, 0), (0, 0, -1), (0, 0, 1)]:
            new_position = tuple(map(sum, zip(position, direction)))
            if new_position not in bounding_box: 
                continue
            queue.append(new_position)
            bounding_box.pop(bounding_box.index(new_position))

    for cube in bounding_box:
        cubes[cube] = set()


def droplet2file(coordinates):
    out = ""
    x = min_max(coordinates, 0)
    y = min_max(coordinates, 1)
    z = min_max(coordinates, 2)
    for _z in range(z[0]-1, z[1]+2):
        out += f"{_z}\n\n\n"
        for _x in range(x[0]-1, x[1]+2):
            for _y in range(y[0]-1, y[1]+2):
                if (_x, _y, _z) in coordinates: 
                    out += "L"
                else:
                    out += "#"
            out += f"\n"
        out += f"\n\n\n"
    with open("out.txt", "w") as f: 
        f.write(out)


def main(file_name, stage):

    with open(file_name, "r") as f: 
       cubes = {get_coords(*coords.split(",")): set() for coords in f.readlines()}
    
    if stage == 2: 
        add_contained_air(cubes)
    print(get_surface_area(cubes))


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")
    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")