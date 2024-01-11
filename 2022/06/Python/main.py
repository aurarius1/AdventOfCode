import time


def main(input_file, stage): 
    with open(input_file, "r") as file: 
        datastream = file.readline().strip("\n")
        marker_start, marker, len_marker = 0, 0, (4 if stage == 1 else 14)
        # first character should always be valid as there cannot be any duplicate yet
        for i in range(1, len(datastream)):
            # if our marker length is reached, we do not care about the next element anymore
            if len(datastream[marker_start:i]) == len_marker:
                marker = marker_start + len(datastream[marker_start:i])
                break

            if datastream[i] in datastream[marker_start:i]: 
                marker_start = datastream[:i].rindex(datastream[i])+1

        print(marker)



if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")