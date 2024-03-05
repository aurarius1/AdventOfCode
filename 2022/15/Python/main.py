import time


def pos(p):
    return tuple(map(int, p.split("at ")[1].strip().replace("x=", "").replace("y=", "").split(", ")))[::-1]


def parse(sensor, beacon):
    return pos(sensor), pos(beacon)


def man_dist(sensor, beacon):
    return abs(sensor[0]-beacon[0]) + abs(sensor[1]-beacon[1])


def main(input_file, stage, example): 
    sensor_map, blocked, boundaries = {}, set(), (2000000 if not example else 10) * stage

    with open(input_file, "r") as f: 
        sensor_map = {s: (b, man_dist(s, b)) for s, b in [parse(*scan.split(":")) for scan in f.readlines()]}

    rows = range(boundaries, boundaries+1) if stage == 1 else range(0, boundaries+1)
    for questioned_row in rows:
        blocked = set()
        for sensor in sensor_map.keys():
            dist_to_row = abs(sensor[0] - questioned_row)
            if dist_to_row > sensor_map[sensor][1]: 
                continue
            blocked_spots = (sensor_map[sensor][1] - dist_to_row ) * 2  + 1 
            blocked.add((sensor[1] - (blocked_spots-1)//2, sensor[1] + (blocked_spots-1)//2))
        blocked = list(sorted(blocked, key=lambda x: (x[0], x[1])))

        while len(blocked) > 1:
            b1 = blocked.pop(0)
            b2 = blocked.pop(0)
            if (b1[0] <= b2[0] and b1[1] >= b2[0]) or b1[1]+1 == b2[0]:
                blocked.insert(0, (b1[0], b2[1] if b2[1] > b1[1] else b1[1]))
                continue

            print((b1[1]+1) * 4000000 + questioned_row)
            return

    blocked = list(blocked)[0]
    print(blocked[1]-blocked[0])


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1, use_example) 
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2, use_example) 
    print(f"Stage 2 time: {time.time() - start}")