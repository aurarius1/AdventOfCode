import time


cache = {
}


def get_possibilities(springs, group, it=0, len_curr_group=0, group_idx=0):
    if group_idx - len(group) == 0:
        for spring in springs[it:]:
            if spring == "#":
                return 0
        return 1

    if it < len(springs):
        if springs[it] == "#":
            if len_curr_group == group[group_idx]:
                return 0
            key = (it + 1, len_curr_group+1, group_idx)
            if cache.get(key, None) is not None:
                return cache[key]
            res = get_possibilities(springs, group, it + 1, len_curr_group+1, group_idx)
            cache[key] = res
            return res
        elif springs[it] == ".":
            if len_curr_group == group[group_idx]:
                group_idx += 1

                key = (it+1, 0, group_idx)
                if cache.get(key, None) is not None:
                    return cache[key]

                res = get_possibilities(springs, group, it + 1, 0, group_idx)
                cache[key] = res

                return res
            elif len_curr_group == 0:
                key = (it + 1, 0, group_idx)
                if cache.get(key, None) is not None:
                    return cache[key]

                res = get_possibilities(springs, group, it + 1, 0, group_idx)
                cache[key] = res
                return res
            else:
                return 0
        elif springs[it] == "?":
            if len_curr_group == group[group_idx]:
                group_idx += 1
                key = (it + 1, 0, group_idx)
                if cache.get(key, None) is not None:
                    return cache[key]
                res = get_possibilities(springs, group, it + 1, 0, group_idx)
                cache[key] = res
                return res
            elif len_curr_group != 0:
                key = (it + 1, len_curr_group + 1, group_idx)
                if cache.get(key, None) is not None:
                    return cache[key]
                res = get_possibilities(springs, group, it + 1, len_curr_group + 1, group_idx)
                cache[key] = res
                return res
            else:
                key = (it + 1, 0, group_idx)
                if cache.get(key, None) is not None:
                    tmp = cache[key]
                else:
                    tmp = get_possibilities(springs, group, it + 1, 0, group_idx)
                    cache[key] = tmp

                key = (it + 1, len_curr_group + 1, group_idx)
                if cache.get(key, None) is not None:
                    tmp2 = cache[key]
                else:
                    tmp2 = get_possibilities(springs, group, it + 1, len_curr_group + 1, group_idx)
                    cache[key] = tmp2

                return tmp + tmp2

    if len(group)-group_idx == 1:
        if len_curr_group == group[group_idx]:
            return 1
        return 0
    elif len(group)-group_idx > 1:
        return 0

    return 1


def main(input_file, stage=1):
    with open(input_file) as file:
        puzzle = file.readlines()
        springs, groups = [], []
        arrangements = []

        # prepare input
        for row in puzzle:
            spring, group = row.split()
            group = [int(group) for group in group.split(",")]

            if stage == 2:
                spring = "?".join([spring]*5)
                tmp = []
                for i in range(5):
                    tmp.extend(group)
                group = tmp
            cache.clear()

            arrangements.append(get_possibilities(spring, tuple(group)))
        print("arrangements", sum(arrangements))


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")

    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
