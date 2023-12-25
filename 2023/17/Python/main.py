import time
import math
import heapq


def pop_min(queue, distances):
    min_elem = 0

    for i in range(len(queue)):
        if distances.get(queue[i], math.inf) < distances.get(queue[min_elem], math.inf):
            min_elem = i
    return queue.pop(min_elem)


# stage 1 takes ~ 56s, stage 2 ~567
def find_minimal_heat_loss(city_map, max_consecutive=3, min_consecutive=1):
    distances, prev = {}, {}
    queue = [((0, 0), (0, 0), 0)]
    distances[((0, 0), (0, 0), 0)] = 0
    visited = set()

    directions = [(1, 0), (0, 1), (-1, 0), (0, -1)]

    while len(queue) != 0:
        u, curr_dir, path_length = pop_min(queue, distances)
        dist = distances[(u, curr_dir, path_length)]
        if (u, curr_dir, path_length) in visited:
            continue
        visited.add((u, curr_dir, path_length))
        for direction in directions:
            if direction == (-curr_dir[0], -curr_dir[1]):
                continue
            neighbor = (u[0]+direction[0], u[1]+direction[1])
            if not (0 <= neighbor[0] < len(city_map)):
                continue
            if not (0 <= neighbor[1] < len(city_map[neighbor[0]])):
                continue

            if direction != curr_dir and path_length < min_consecutive and u != (0,0):
                continue

            t = 1
            if direction == curr_dir:
                t += path_length
            if t > max_consecutive:
                continue

            alt = dist + int(city_map[neighbor[0]][neighbor[1]])
            if alt < distances.get((neighbor, direction, t), math.inf):
                distances[(neighbor, direction, t)] = alt
                prev[(neighbor, direction, t)] = u
                queue.append((neighbor, direction, t))

    keys = [k for k in list(distances.keys()) if k[0] == (len(city_map)-1, len(city_map[0])-1)]
    minimal_path = keys[0]
    for key in keys:
        if distances[key] < distances[minimal_path]:
            minimal_path = key

    return distances[minimal_path]


# stage 1 ~ 0.83s, stage 2 ~ 2.9s
def find_minimal_heat_loss_heapq(city_map, max_consecutive=3, min_consecutive=1):
    queue = [(0, (0, 0), (0, 0), 0)]    # heat_loss, node, direction, steps taken
    visited = set()
    directions = [(1, 0), (0, 1), (-1, 0), (0, -1)]
    while len(queue) != 0:
        heat_loss, curr, curr_dir, curr_steps = heapq.heappop(queue)
        key = (curr, curr_dir, curr_steps)
        if key in visited:
            continue
        if curr == (len(city_map)-1, len(city_map[0])-1):
            return heat_loss
        visited.add(key)
        for direction in directions:
            if direction == (-curr_dir[0], -curr_dir[1]):
                continue
            neighbor = (curr[0] + direction[0], curr[1] + direction[1])
            if not (0 <= neighbor[0] < len(city_map)):
                continue
            if not (0 <= neighbor[1] < len(city_map[neighbor[0]])):
                continue
            if direction != curr_dir and curr_steps < min_consecutive and curr != (0, 0):
                continue

            t = 1
            if direction == curr_dir:
                t += curr_steps
            if t > max_consecutive:
                continue
            new_heat_loss = heat_loss + int(city_map[neighbor[0]][neighbor[1]])
            heapq.heappush(queue, (new_heat_loss, neighbor, direction, t))


def main(input_file, stage=1):
    with open(input_file) as file:
        city_map = file.readlines()
        city_map = [row.strip() for row in city_map]
        minimal_heat_loss = 0
        if stage == 1:
            minimal_heat_loss = find_minimal_heat_loss_heapq(city_map)
        elif stage == 2:
            minimal_heat_loss = find_minimal_heat_loss_heapq(city_map, max_consecutive=10, min_consecutive=4)
        print(minimal_heat_loss)


if __name__ == "__main__":
    use_example = True
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time() - start_time:.10f}")

    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time() - start_time:.10f}")
