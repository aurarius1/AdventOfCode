import time
import argparse


def state(coordinates, sorted_coordinates):
    for coord in sorted_coordinates: 
        print(coordinates[coord], end=" ")
    print()



def main(input_file, stage=1):

    decryption_key = 1 if stage == 1 else 811589153
    with open(input_file) as file:
        coordinates = {idx: int(coordinate.strip()) * decryption_key for idx, coordinate in enumerate(file.readlines())}
    
    coords = list(coordinates.keys())
    for _ in range(1 if stage == 1 else 10):
        for coord in coordinates.keys(): 
            curr = coords.index(coord)
            new_pos = (curr + coordinates[coord]) % (len(coords)-1)
            coords.pop(curr)
            coords.insert(new_pos, coord)

    for i in range(len(coords)):
        coords[i] = coordinates[coords[i]]

    #print(coords)
    grove_coordinates = 0
    zero = coords.index(0)
    for i in [1000, 2000, 3000]:
        grove_coordinates += coords[(zero + i) % len(coords)]
    
    print(grove_coordinates)
    


if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        prog='main',
        description='AdventOfCode main skeleton',
    )
    parser.add_argument("-e", "--example", action=argparse.BooleanOptionalAction, default=False, help="Set if you want to run the example")
    parser.add_argument("-s", "--stage", action='store', default=0, help="Pass the stage you want to run", type=int)
    args = parser.parse_args()
    file_name = "example" if args.example else "input"

    if args.stage == 1 or args.stage == 0:
        start_time = time.time()
        main(file_name, 1)
        print(f"Stage 1 time: {time.time()-start_time:.10f}")

    if args.stage == 2 or args.stage == 0:
        start_time = time.time()
        main(file_name, 2)
        print(f"Stage 2 time: {time.time()-start_time:.10f}")
