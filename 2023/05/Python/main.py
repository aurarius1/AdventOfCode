import time

mappings = {}
maps = ['seed-to-soil', 'soil-to-fertilizer', 'fertilizer-to-water', 'water-to-light', 'light-to-temperature',
        'temperature-to-humidity', 'humidity-to-location']


class Range:
    def __init__(self, start_src, start_dst, rng):
        self.start = int(start_src)
        self.rng = int(rng)
        self.end = self.start + self.rng
        self.start_dst = int(start_dst)

    def in_range(self, to_check):
        if self.rng == -1 and self.start <= to_check:
            return True
        return self.start <= to_check < self.end

    def __str__(self):
        return f"start: {self.start}, dst_start: {self.start_dst}, range: {self.rng}"

    def get_destination(self, to_check):
        if self.rng == -1:
            return to_check
        return to_check-self.start + self.start_dst

    def get_rng_remainder(self, offset):
        if self.rng == -1:
            return self.rng

        if offset > self.end:
            return 0

        return self.end - offset


def seed_2_location(seeds):
    global mappings, maps
    for map_name in maps:
        curr_map = mappings[map_name]
        tmp = []
        for seed in seeds:
            idx = [i for i in range(len(curr_map)) if curr_map[i].in_range(seed.start)][0]
            curr_range = curr_map[idx]
            dst_start = curr_range.get_destination(seed.start)
            rng = curr_range.get_rng_remainder(seed.start)
            # current range fits snugly in destination range
            if rng == -1 or seed.rng <= rng:
                seed.start = dst_start
                tmp.append(seed)
                continue

            while True:
                tmp.append(Range(dst_start, -1, rng))
                seed.rng -= rng
                if rng == -1 or seed.rng <= 0:
                    break
                idx += 1
                seed.start += rng
                dst_start = curr_map[idx].get_destination(seed.start)
                rng = min(seed.rng, curr_map[idx].get_rng_remainder(seed.start))
        tmp.sort(key=lambda x: x.start)
        seeds = tmp
    return seeds[0].start


def read_mappings():
    global mappings
    with open("input", "r") as file:
        data = file.readlines()
        needed_seeds = [int(seed) for seed in data[:data.index("\n")][0].split() if seed.isnumeric()]
        data = data[data.index("\n") + 1:]
        while True:
            try:
                end = data.index("\n")
            except ValueError:
                end = -1
            current_map = data[:end]
            current_map_key = current_map[0].split()[0]
            mappings[current_map_key] = []

            for mapping in current_map[1:]:
                soil_start, seed_start, rng = mapping.strip().split()
                tmp_rng = Range(seed_start, soil_start, rng)
                mappings[current_map_key].append(tmp_rng)

            mappings[current_map_key].sort(key=lambda x: x.start)
            tmp_mappings = []
            prev_end = 0

            # add 1:1 ranges for easier usage later on
            for mapping in mappings[current_map_key]:
                if prev_end < mapping.start:
                    tmp_mappings.append(Range(prev_end, prev_end, mapping.start-prev_end))
                prev_end = mapping.start + mapping.rng
                tmp_mappings.append(mapping)

            tmp_mappings.append(Range(prev_end, prev_end, -1))
            mappings[current_map_key] = tmp_mappings
            if end == -1:
                break
            data = data[end + 1:]
        return needed_seeds


def main(stage=1):
    needed_seeds = read_mappings()
    if stage == 1:
        seeds = [Range(seed, -1, 1) for seed in needed_seeds]
    elif stage == 2:
        seeds = [Range(needed_seeds[i], -1, needed_seeds[i+1]) for i in range(0, len(needed_seeds), 2)]
    else:
        print("stage needs to be one or two")
        return
    print(seed_2_location(seeds))


if __name__ == '__main__':
    start = time.time()
    main(1)
    print(f"Stage 1 time: {time.time()-start:.10f}s")
    start = time.time()
    main(2)
    print(f"Stage 2 render time: {time.time()-start:.10f}s")

