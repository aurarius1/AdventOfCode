import time
def get_score(opponent, own):
    scores = {"R": 1,  "P": 2, "S": 3}
    wins = {"R": "P", "S": "R", "P": "S"}
    if wins[opponent] == own:
        return scores[own] + 6
    return scores[own] + (3 if opponent == own else 0)


def replace_instruction(opponent, own, stage):
    opponent = opponent.replace("A", "R").replace("B", "P").replace("C", "S")
    if stage == 1:
        return opponent, own.replace("X", "R").replace("Y", "P").replace("Z", "S")

    if own == "Z":
        wins = {"R": "P", "S": "R", "P": "S"}
        return opponent, wins[opponent]

    if own == "X":
        looses = {"R": "S", "S": "P", "P": "R"}
        return opponent, looses[opponent]

    return opponent, opponent



def main(input_file, stage):
    with open(input_file, "r") as file:
        strategy_guide = [replace_instruction(*row.strip().split(), stage) for row in file]
        points = [get_score(*row) for row in strategy_guide]
        print(sum(points))



if __name__ == "__main__":
    use_example = False
    file = "example" if use_example else "input"

    start = time.time()
    main(file, 1)
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file, 2)
    print(f"Stage 2 time: {time.time() - start}")
