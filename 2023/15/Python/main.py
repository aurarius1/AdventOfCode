import time
hashmap = {}


def hash_label(label, prev_node=0):
    current_value = prev_node
    for char in label:
        current_value += ord(char)
        current_value *= 17
        current_value %= 256
    return current_value


def hash_sequence(sequence, stage=1):
    sequence_hash = 0
    for code in sequence:

        operation = "-" if "-" in code else "="
        label, focal_length = code.split(operation)
        lbl_hash = hash_label(label)
        if stage == 2:
            global hashmap
            hashmap[label] = lbl_hash
        sequence_hash += hash_label(operation+focal_length, lbl_hash)
    return sequence_hash


def main(input_file, stage=1):
    with open(input_file) as file:
        sequence = file.readlines()
        sequence = sequence[0].strip().split(",")

        sequence_hash = hash_sequence(sequence, stage)
        if stage == 1:
            print(sequence_hash)
            return

        # commented out is the first version using a list of tuples instead of a dict
        # didn't know at first that dicts keep the order of insertion

        #boxes = [[] for _ in range(256)]
        boxes = [{} for _ in range(256)]
        global hashmap
        for code in sequence:
            operation = "-" if "-" in code else "="
            label, focal_length = code.split(operation)
            #box_num = 0
            #while box_num < len(boxes[hashmap[label]]):
            #    if boxes[hashmap[label]][box_num][0] == label:
            #        break
            #    box_num += 1

            if operation == "-":
                #if box_num < len(boxes[hashmap[label]]):
                if boxes[hashmap[label]].get(label, None) is not None:
                    del boxes[hashmap[label]][label]
            if operation == "=":
                boxes[hashmap[label]][label] = int(focal_length)
                #if box_num < len(boxes[hashmap[label]]):
                #    boxes[hashmap[label]][box_num][1] = int(focal_length)
                #else:
                #    boxes[hashmap[label]].append([label, int(focal_length)])

        focusing_power = 0
        for i in range(len(boxes)):
            focusing_power_lenses = [(i+1) * (j+1) * boxes[i][slot] for j, slot in enumerate(boxes[i].keys())]
            focusing_power += sum(focusing_power_lenses)
        print(focusing_power)


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")

    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
