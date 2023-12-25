import time
import numpy


def det(vec1, vec2):
    return vec1[0]*vec2[1] - vec1[1]*vec2[0]


def dot(vec1, vec2):
    return vec1[0] * vec2[0] + vec1[1] * vec2[1]


class Hailstone:
    def __init__(self, position, velocity):
        self.position = [int(pos) for pos in position.split(", ")]
        self.velocity = [int(vel) for vel in velocity.split(", ")]

    def __str__(self):
        return str(self.position) + " @ " + str(self.velocity)

    def intersect(self, other, v_rock=(0, 0)):
        n_self = (self.velocity[1]-v_rock[1], -(self.velocity[0]-v_rock[0]))
        n_other = (other.velocity[1]-v_rock[1], -(other.velocity[0]-v_rock[0]))
        c1 = dot(self.position, n_self)
        c2 = dot(other.position, n_other)
        determinant = det(n_self, n_other)
        if determinant == 0:
            return -1, -1
        t1 = det((c1, c2), (n_self[1], n_other[1]))
        t2 = det((n_self[0], n_other[0]), (c1, c2))
        return t1 / determinant, t2 / determinant

    def get_z_intersection(self, intersection, v_rock):
        v_own = (self.velocity[0] - v_rock[0], self.velocity[1] - v_rock[1], self.velocity[2] - v_rock[2])
        diff = (intersection[0] - self.position[0], intersection[1] - self.position[1])
        if v_own[0] != 0:
            t = diff[0] / v_own[0]
        elif v_own[1] != 0:
            t = diff[1] / v_own[1]
        else:
            return -1

        if t < 0:
            return -1

        z = self.position[2] + t * v_own[2]

        return *intersection, z


def stage2(hailstones):
    range_start = -400
    range_end = 400
    first_hailstone = hailstones[0]
    for x in range(range_start, range_end):
        for y in range(range_start, range_end):
            velocity = (x, y, 0)
            curr_intersect = (0, 0)
            for i in range(1, len(hailstones)):
                hailstone = hailstones[i]
                intersection = first_hailstone.intersect(hailstone, velocity)
                if intersection[0] == -1 or intersection[1] == -1:
                    break
                if curr_intersect != intersection and curr_intersect != (0, 0):
                    break
                curr_intersect = intersection
            else:
                for z in range(range_start, range_end):
                    z_intersect = (0, 0)
                    velocity = (velocity[0], velocity[1], z)
                    for i in range(1, len(hailstones)):
                        hailstone = hailstones[i]
                        intersection = hailstone.get_z_intersection(curr_intersect, velocity)
                        if intersection[0] == -1 or intersection[1] == -1:
                            break
                        if z_intersect != intersection and z_intersect != (0, 0):
                            break
                        z_intersect = intersection
                    else:
                        print(sum(z_intersect), velocity)
                        return


def stage1(hailstones, is_example):
    if is_example:
        intersection_range = (7, 27)
    else:
        intersection_range = (200000000000000, 400000000000000)
    cnt = 0
    for i, hailstone in enumerate(hailstones):
        for j in range(i + 1, len(hailstones)):
            intersection = hailstone.intersect(hailstones[j])
            if not (intersection_range[0] <= intersection[0] <= intersection_range[1]):
                continue
            if not (intersection_range[0] <= intersection[1] <= intersection_range[1]):
                continue
            t = (intersection[0] - hailstone.position[0], intersection[1] - hailstone.position[1])
            t = (t[0] / hailstone.velocity[0], t[1] / hailstone.velocity[1])
            if t[0] < 1 or t[1] < 1:
                continue

            s = (intersection[0] - hailstones[j].position[0], intersection[1] - hailstones[j].position[1])
            s = (s[0] / hailstones[j].velocity[0], s[1] / hailstones[j].velocity[1])
            if s[0] < 1 or s[1] < 1:
                continue

            cnt += 1
    print(cnt)


def main(input_file, is_example, stage=1):
    with open(input_file) as file:
        hailstones = file.readlines()
        hailstones = [Hailstone(*hailstone.strip().split(" @ ")) for hailstone in hailstones]

        if stage == 1:
            stage1(hailstones, is_example)
        else:
            stage2(hailstones)


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, use_example, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")


    start_time = time.time()
    main(file_name, use_example, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
