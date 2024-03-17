import time
import argparse
import re


operations = {
    "+": (lambda a, b: a+b),
    "-": (lambda a, b: a-b),
    "*": (lambda a, b: a*b),
    "/": (lambda a, b: a/b)
}
inverse = {
    "/": "*",
    "*": "/",
    "+": "-",
    "-": "+"
}


def job(monkey): 
    monkey = monkey.strip()
    if monkey.isnumeric():
        return [int(monkey)]
    monkey = monkey.split(" ")
    return monkey


def solve_root(monkeys, curr):
    global operations
    if len(monkeys[curr]) == 1: 
        return monkeys[curr][0]
    left = solve_root(monkeys, monkeys[curr][0])
    right = solve_root(monkeys, monkeys[curr][2])
    return operations[monkeys[curr][1]](left, right)


def get_expression(monkeys, curr, needle="humn"):
    global operations
    if curr == needle: 
        return f"({needle})"
    if len(monkeys[curr]) == 1: 
        return "(" + str(monkeys[curr][0]) + ")"

    left = get_expression(monkeys, monkeys[curr][0], needle)
    right = get_expression(monkeys, monkeys[curr][2], needle)
    return "(" + left +  monkeys[curr][1] +  right + ")"


def split_with_brackets(expr):
    num_opening = 0
    for idx, ch in enumerate(expr): 
        if ch == "(":
            num_opening += 1
        if ch == ")":
            num_opening -= 1
        if num_opening == 0:
            idx += 1
            return expr[:idx], expr[idx:idx+1], expr[idx+1:]
    return "", "", ""
    

def solve_equation(expr, solution, needle="humn"): 
    global inverse
    if expr == needle: 
        return solution
    left, operation, right = split_with_brackets(expr)
    evaluated = eval(left if needle in right else right)
    searched = right if needle in right else left
    op = inverse[operation]
    if needle in right and (operation == "-" or operation == "/"): 
        solution, evaluated = evaluated, solution
        op = operation
    solution = eval(f"{solution}{op}{evaluated}")
    return solve_equation(searched[1:-1], solution, needle)



def main(input_file, stage=1):
    with open(input_file) as file:
        monkeys = {monkey.split(":")[0]: job(monkey.split(":")[1]) for monkey in file.readlines()}
    
    l_result, r_result = solve_root(monkeys, monkeys["root"][0]), solve_root(monkeys, monkeys["root"][2])  

    if stage == 1: 
        # technically this could also be solved with get_expression + eval, but it is slower
        result = int(operations[monkeys["root"][1]](l_result, r_result))
        #assert result == 85616733059734
        print(result)
        return
    
    needle = "humn"
    l_expr = get_expression(monkeys, monkeys["root"][0], needle)[1:-1]
    r_expr = get_expression(monkeys, monkeys["root"][2], needle)[1:-1]
    needed = r_result if needle in l_expr else l_result
    result = int(solve_equation(l_expr if needle in l_expr else r_expr, needed, needle))
    #assert result == 3560324848168
    print(result)
    


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
