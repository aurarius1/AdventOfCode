import time as ttime
from math import ceil, floor, sqrt


def main(stage=1):
    with open("input", "r") as file:
        times, distances = file.readline(), file.readline()

        if stage == 2:
            times = times.replace(" ", "").replace(":", " ")
            distances = distances.replace(" ", "").replace(":", " ")

        times = times.split()[1:]
        distances = distances.split()[1:]
        margin_of_error = 1

        for time, distance in zip(times, distances):
            time, distance = int(time), int(distance)
            sol_sqrt = sqrt(pow(time, 2) - 4 * (-1) * (-distance))
            x1 = (-time + sol_sqrt) / -2.0
            x2 = (-time - sol_sqrt) / -2.0
            t1, t2 = (floor(x1 + 1), ceil(x2 - 1)) if x1 < x2 else (ceil(x1 - 1), floor(x2 + 1))
            margin_of_error *= (abs(t2-t1)+1)
        print(margin_of_error)


if __name__ == '__main__':
    start = ttime.time()
    main(1)
    print(f"Stage 1 time: {ttime.time() - start:.10f}s")
    start = ttime.time()
    main(2)
    print(f"Stage 2 render time: {ttime.time() - start:.10f}s")
