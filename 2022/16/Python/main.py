import time
import functools


def get_stats(valve):
    valve = valve.strip().split("rate=")[1]
    rate = int(valve.split(";")[0])
    valve = valve.replace("valve ", "valves ").split("valves ")[1]
    destinations = valve.split(", ") 
    return {
        "rate": rate, 
        "destinations": destinations
    }


def get_name(valve):
    valve = valve.replace("Valve ", "")
    valve_id = valve[:valve.index(" ")]
    return valve_id


cache = {}
elephant_first = True
def cache_moves(func):
    def inner(valves, time, current_valve, opened_valves, who=""):
        val = cache.get((time, current_valve, frozenset(opened_valves), who), None)
        if val is None: 
            val = func(valves, time, current_valve, opened_valves, who)
            cache[(time, current_valve, frozenset(opened_valves), who)] = val
        return val
    return inner


@cache_moves
def move(valves, time, current_valve, opened_valves, who=""):
    if time == 1: 
        if who == "human":
            return move(valves, 26, "AA", opened_valves, "elephant")
        return 0
    t = 0
    for next_valve in valves[current_valve]["destinations"]:
        tmp = move(valves, time-1, next_valve, opened_valves, who)
        if tmp >= t:
            t = tmp
    if valves[current_valve]["rate"] == 0 or current_valve in opened_valves:
        return t
    
    tmp_valves = opened_valves.copy()
    tmp_valves.add(current_valve)
    tmp  = move(valves, time-1, current_valve, tmp_valves, who) + valves[current_valve]["rate"] * (time-1)
    if tmp >= t:
        t = tmp
    return t


def main(input_file, stage): 
    valves = {}

    with open(input_file, "r") as f: 
        valves = {get_name(valve): get_stats(valve) for valve in f.readlines()}

    time, who = 30, ""
    if stage == 2:
        time, who = 26, "human"

    print(move(valves, time, "AA", set(), who))




if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")