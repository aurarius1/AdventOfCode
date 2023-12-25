import time



def is_equal(left, right, stage, smudge_fixed=False):
    if stage == 1:
        return left == right, True
    diffs = [idx for idx in range(len(left)) if  left[idx] != right[idx]]

    if smudge_fixed and len(diffs) != 0:
        return False, False
    return len(diffs) <= 1, len(diffs) != 0 or smudge_fixed


def find_perfect_reflection(pattern, horizontal, stage = 1):
    reflection_line = None
    for i in range(0, len(pattern)-1):
        j = 0
        smudge_fixed = False
        while i+1+j < len(pattern) and j < i+1:
            equal, smudge_fixed = is_equal(pattern[i-j], pattern[i+1+j], stage, smudge_fixed)
            if not equal:
                break
            j += 1
        else:
            if smudge_fixed or stage == 1:
                return True, (i+1) * (100 if horizontal else 1)

            # cache first reflection line to be found in case no smudge can be fixed
            if reflection_line is not None:
                reflection_line = (i+1) * (100 if horizontal else 1)

    # no smudge could be fixed
    if reflection_line is not None:
        return False, reflection_line

    return False, 0


def main(inputfile, stage=1):
    with open(inputfile) as file:
        puzzle_input = file.readlines()
        patterns = [[]]
        for puzzle in puzzle_input:
            if puzzle == "\n":
                patterns.append([])
                continue
            patterns[-1].append(puzzle.strip())

        reflections = 0
        for pattern in patterns:

            smudge, refl_value = find_perfect_reflection(pattern, True, stage)
            # if we fixed a smudge here we can just take it for granted and go on
            if smudge:
                reflections += refl_value
                continue

            smudge_col, col_refl_value = find_perfect_reflection(list(zip(*pattern)), False, stage)
            # same as above
            if smudge_col:
                reflections += col_refl_value
                continue

            reflections += refl_value if refl_value == 0 else col_refl_value
        print(reflections)


if __name__ == "__main__":
    use_example = False
    file = "example" if use_example else "input"

    start = time.time()
    main(file, 1)
    print(f"Stage 1 time: {time.time()-start:.10f}")

    start = time.time()
    main(file, 2)
    print(f"Stage 2 time: {time.time()-start:.10f}")
