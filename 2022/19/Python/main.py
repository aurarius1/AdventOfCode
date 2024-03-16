import time
import re
import math

blueprints, c = {}, {}

def robot_costs(blueprint):
    return {
        0: {
            0: int(re.findall("\d+", blueprint[1])[0]), 
        }, 
        1: {
            0: int(re.findall("\d+", blueprint[2])[0]),
        },
        2: {
            0: int(re.findall("\d+", blueprint[3])[0]),
            1: int(re.findall("\d+", blueprint[3])[1]),
        },
        3: {
            0: int(re.findall("\d+", blueprint[4])[0]),
            2: int(re.findall("\d+", blueprint[4])[1])
        }
    }


def blueprint_id(blueprint):
    return int(re.search("\d+", blueprint[0]).group(0))


def cache(func):
    def inner(time, blueprint, robots, materials, max_ore, max_clay, max_obsidian, build_possible):
        val = c.get((time, robots, materials), None)
        if val is None: 
            val = func(time, blueprint, robots, materials, max_ore, max_clay, max_obsidian, build_possible)
            c[(time, robots, materials)] = val
        return val
    return inner


@cache
def break_geodes(time, blueprint, robots, materials, max_ore, max_clay, max_obsidian, build_possible): 
    if time == 1: 
        return robots[3]

    # check building geode robot 
    geode_robot = 0
    if materials[0] >= blueprint[3][0] and materials[2] >= blueprint[3][2]: 
        if 3 not in build_possible:
            new_materials = (materials[0]-blueprint[3][0]+robots[0], materials[1]+robots[1], materials[2]-blueprint[3][2]+robots[2])
            new_robots = (robots[0], robots[1], robots[2], robots[3]+1)
            geode_robot = break_geodes(time-1, blueprint, new_robots, new_materials, max_ore, max_clay, max_obsidian, set())
        build_possible.add(3)

    # check building obsidian robot 
    obsidan_robot = 0
    if materials[0] >= blueprint[2][0] and materials[1] >= blueprint[2][1] and robots[2] < max_obsidian: 
        if 2 not in build_possible:
            new_materials = (materials[0]-blueprint[2][0]+robots[0], materials[1]-blueprint[2][1]+robots[1], materials[2]+robots[2])
            new_robots = (robots[0], robots[1], robots[2]+1, robots[3])
            obsidan_robot =  break_geodes(time-1, blueprint, new_robots, new_materials, max_ore, max_clay, max_obsidian, set())
        build_possible.add(2)

    # check building clay robot 
    clay_robot = 0
    if materials[0] >= blueprint[1][0] and robots[1] < max_clay: 
        if 1 not in build_possible:
            new_materials = (materials[0]-blueprint[1][0]+robots[0], materials[1]+robots[1], materials[2]+robots[2])
            new_robots = (robots[0], robots[1]+1, robots[2], robots[3])
            clay_robot = break_geodes(time-1, blueprint, new_robots, new_materials, max_ore, max_clay, max_obsidian, set())
        build_possible.add(1)

    # check building ore robot 
    ore_robot = 0
    if materials[0] >= blueprint[0][0] and robots[0] < max_ore: 
        if 0 not in build_possible:
            new_materials = (materials[0]-blueprint[0][0]+robots[0], materials[1]+robots[1], materials[2]+robots[2])
            new_robots = (robots[0]+1, robots[1], robots[2], robots[3])
            ore_robot = break_geodes(time-1, blueprint, new_robots, new_materials, max_ore, max_clay, max_obsidian, set())
        build_possible.add(0)

    # check without building anything
    new_materials = (materials[0]+robots[0], materials[1]+robots[1], materials[2]+robots[2])
    no_robot = break_geodes(time-1, blueprint, robots, new_materials, max_ore, max_clay, max_obsidian, build_possible)

    return max(geode_robot, obsidan_robot, clay_robot, ore_robot, no_robot) + robots[3]

def main(file_name, stage):
    global blueprints, c
    with open(file_name, "r") as f: 
       blueprints = {blueprint_id(bp): robot_costs(bp) for bp in [bp.split("Each") for bp in f.readlines()]}

    
    quality_level = 0 if stage == 1 else 1
    time = 24 if stage == 1 else 32

    for blueprint in list(blueprints.keys())[:3 if stage == 2 else None]:
        max_ore = max(blueprints[blueprint][0][0], blueprints[blueprint][1][0], blueprints[blueprint][2][0], blueprints[blueprint][3][0])
        max_clay = blueprints[blueprint][2][1]
        max_obsidian = blueprints[blueprint][3][2]
        rate = break_geodes(time, blueprints[blueprint], (1, 0, 0, 0), (0, 0, 0), max_ore, max_clay, max_obsidian, set())
        if stage == 1: 
            print(f"Blueprint {blueprint}: {rate} - quality level: {rate*blueprint}")
            quality_level += (rate*blueprint)
        else: 
            print(f"Blueprint {blueprint}: {rate}")
            quality_level *= rate
        c = {}

    print(quality_level)


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")
    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")