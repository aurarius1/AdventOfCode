import time as ttime
import math


def get_solutions_polynomial(time, distance):
    sol_sqrt = math.sqrt(pow(time, 2) - 4*(-1)*(-distance))
    x1 = (-time + sol_sqrt)/-2.0
    x2 = (-time - sol_sqrt)/-2.0
    return (math.floor(x1+1), math.ceil(x2-1)) if x1 < x2 else (math.ceil(x1-1), math.floor(x2+1))


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
            t1, t2 = get_solutions_polynomial(time, distance)
            margin_of_error *= (abs(t2-t1)+1)
        
        print(margin_of_error)

# t*x - x2 > d
# -x2 + t*x + 0 > d
# -x2 + t*x -d > 0
# -1x2 + 1t*x -d = 0

# x = -(1t) +- sqrt((1t)^2 - 4*(-1)*(-d))/-2
#56717999, 334113513502430
if __name__ == '__main__':
    start = ttime.time()
    main(1)
    print(f"Stage 1 time: {ttime.time() - start:.10f}s")
    start = ttime.time()
    main(2)
    print(f"Stage 2 render time: {ttime.time() - start:.10f}s")
