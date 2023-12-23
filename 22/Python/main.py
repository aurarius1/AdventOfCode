import time


class Position(tuple):
    def __new__(cls, coordinates):
        coordinates = tuple([int(coord) for coord in coordinates])
        return super(Position, cls).__new__(cls, coordinates)

    def __add__(self, other):
        return Position(x + y for x, y in zip(self, other))

    def __radd__(self, other):
        return Position((self[0]+other, self[1]+other, self[2]+other))

    def __rmul__(self, other):
        return Position((self[0]*other, self[1]*other, self[2]*other))

    def __sub__(self, other):
        return Position((self[0]-other[0], self[1]-other[1], self[2]-other[2]))

    def __getitem__(self, item):
        if isinstance(item, int):
            return super().__getitem__(item)
        match item:
            case "x":
                item = 0
            case "y":
                item = 1
            case "z":
                item = 2
            case _:
                raise Exception("INVALID INDEX")
        return super().__getitem__(item)

    def __eq__(self, other):
        return all(s == o for s, o in zip(self, other))

    def __hash__(self):
        return super().__hash__()

    def max(self, o1, o2):
        return Position((max(self.x, o1.x, o2.x), max(self.y, o1.y, o2.y), max(self.z, o1.z, o2.z)))

    def normalize(self):
        x = self.x/self.x if self.x > 0 else 0
        y = self.y / self.y if self.y > 0 else 0
        z = self.z / self.z if self.z > 0 else 0
        return Position((x, y, z))

    @property
    def z(self):
        return super().__getitem__(2)

    @property
    def x(self):
        return super().__getitem__(0)

    @property
    def y(self):
        return super().__getitem__(1)


def print_map(max_pos, layers, direction="x"):
    # prints the map like given in the example
    print("DIRECTION:", direction)
    for i in range(max_pos[direction]):
        print(i, end="")
    print()

    for i in range(max_pos.z - 1, 0, -1):
        # print(layers[i])
        string = ""

        for y in range(max_pos["y" if direction == "x" else "x"]):
            xs = []
            for x in range(max_pos[direction]):
                idx = 0
                if direction == "x":
                    idx = x + y * max_pos.y
                if direction == "y":
                    idx = y + x*max_pos.y

                xs.append(layers[i][idx])

            if all(x is None for x in xs):
                string += "."
                continue

            #if xs.count(None) < len(xs) - 1:
            last_id = -1
            id_cnt = 0
            for t in xs:
                if t is None:
                    continue
                if last_id == -1:
                    last_id = t[0]
                id_cnt += t[0]
            if id_cnt == last_id * (len(xs) - xs.count(None)):
                string += str(hex(last_id%16))[2:]
            else:
                string += "?"
                continue

            #string += [str(t[0]) for t in xs if t is not None][0]  # str(layers[i][j * max_pos.y][0])

        print(string, i)
    print("-" * ((max_pos.x - 1) * (max_pos.y - 1)), 0)


def main(input_file, stage=1):
    with (open(input_file) as file):
        sand_blocks = file.readlines()
        max_pos = Position((0, 0, 0))
        blocks, supporting_blocks, blocks_supported_by = {}, {}, {}
        for i, block in enumerate(sand_blocks):
            start, end = block.strip().split("~")
            start, end = Position(start.split(",")), Position(end.split(","))
            max_pos = max_pos.max(start, end)

            blocks[i] = []
            supporting_blocks[i] = set()
            blocks_supported_by[i] = set()

            direction = end-start
            inc = direction.normalize()
            cnt = 0
            while cnt <= sum(direction):
                split_block = start + cnt*inc
                blocks[i].append(split_block)
                cnt += 1

        max_pos = 1 + max_pos
        layers = {k: [None] * (max_pos.x*max_pos.y) for k in range(0, max_pos.z)}

        for i in range(len(sand_blocks)):
            sand_block = blocks[i]
            for block in sand_block:
                layers[block.z][block.x*max_pos.y + block.y] = (i, block)

        visited = set()
        for layer in range(1, max_pos.z+1):
            blocks_to_move = {}
            for block in layers.get(layer, []):
                if block is None or block[0] in visited:
                    continue
                below = layer-1
                bid, block = block
                visited.add(bid)

                # check how far blocks can fall
                while below > 0:
                    for next_block in blocks[bid]:
                        idx = next_block.x * max_pos.y + next_block.y
                        if layers[below][idx] is not None and layers[below][idx][0] != bid:
                            break
                    else:
                        below -= 1
                        continue

                    # if we break from the inner loop, the block doesnt fit in the current layer, so we increase it
                    # to the last fitting layer
                    below += 1
                    break
                # blocks would fall through the first layer (so they fit in the first layer)
                if below == 0:
                    below += 1

                # if layers can fall, add them to the set to let them fall later on
                blocks_to_move[bid] = 0
                if below < layer:
                    blocks_to_move[bid] = layer-below

            for bid in blocks_to_move.keys():
                for i, block in enumerate(blocks[bid]):
                    pos = block.x * max_pos.y + block.y
                    layer = block.z - blocks_to_move[bid]
                    # if block needs to be falling
                    if blocks_to_move[bid] > 0:
                        blocks[bid][i] = Position((block.x, block.y, layer))
                        layers[block.z][pos], layers[layer][pos] = None, (bid, blocks[bid][i])
                    # if layer below is not supporting (e.g. one spot below is empty)
                    # or layer below is from the same block, we don't need to add it to the lookup dicts
                    if layers[layer - 1][pos] is None or layers[layer-1][pos][0] == bid:
                        continue
                    supporting_blocks[layers[layer - 1][pos][0]].add(bid)
                    blocks_supported_by[bid].add(layers[layer - 1][pos][0])

        if stage == 1:
            removable_blocks = set()
            for supporting_block in supporting_blocks:
                supported_blocks = list(supporting_blocks[supporting_block])
                if any(len(blocks_supported_by[supported_block]) == 1 for supported_block in supported_blocks):
                    continue
                removable_blocks.add(supporting_block)
            print(len(removable_blocks))
            assert len(removable_blocks) == (477 if input_file == "input" else 5)
            return

        cnt = 0
        for supporting_block in supporting_blocks:
            supporting = {supporting_block}
            falling = {supporting_block}
            while supporting:
                curr_supporting = supporting.pop()
                supported_blocks = supporting_blocks[curr_supporting]
                for supported_block in supported_blocks:
                    t = blocks_supported_by[supported_block] - falling
                    if not t:
                        supporting.add(supported_block)
                        falling.add(supported_block)
            cnt += len(falling)-1
        print(cnt)
        assert cnt == (61555 if input_file == "input" else 7)




if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")


    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
