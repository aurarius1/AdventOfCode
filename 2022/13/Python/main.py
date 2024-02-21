import time
from functools import cmp_to_key


# this works, but eval is just soooo much easier (found eval only 
# after implementing this)
def parse_input(packet_string):
    parsed_packet = []
    idx = 0
    while idx < len(packet_string):
        packet = packet_string[idx]
        if packet == ",": 
            idx += 1
            continue
        if packet == "[":
            sublist, end = parse_input(packet_string[idx+1:])
            parsed_packet.append(sublist)
            idx += end+1
            continue
        idx += 1
        if packet == "]":
            return parsed_packet, idx
        
        while idx < len(packet_string) and packet_string[idx].isnumeric() :
            packet+=packet_string[idx]
            idx+=1

        parsed_packet.append(int(packet))
    return parsed_packet


def right_order(p1, p2):
    for left, right in zip(p1, p2):
        # if both are the same skip all the evaluations
        if left == right: 
            continue
        if isinstance(left, int) and isinstance(right, int):
            return -1 if (left < right) else 1
        elif isinstance(left, int): 
            left = [left]
        elif isinstance(right, int): 
            right = [right]
        
        order = right_order(left, right)
        if order != 0:
            return order
    if len(p1) == len(p2):
        return 0
    return -1 if (len(p1) < len(p2)) else 1


def main(input_file, stage): 
    with open(input_file, "r") as file: 
        packets = []
        for line in file.readlines():
            if len(line) > 1:
                #packets.append(parse_input(data[i][1:-1]))
                packets.append(eval(line.strip()))
        
        if stage == 2: 
            packets.extend([[[2]], [[6]]])
            packets = sorted(packets, key=cmp_to_key(right_order))
            print((packets.index([[2]])+1)*(packets.index([[6]])+1))
            return
        
        packets = [(packets[p], packets[p+1]) for p in range(0, len(packets), 2)]
        result = sum([idx+1 for idx, p in enumerate(packets) if right_order(*p) == -1])
        print(result)


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")