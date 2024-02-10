import time


class Monkey:
    def __init__(self, id, items, operation, operand, test, test_true, test_false, PRINT):
        self.id = id
        self.items = items
        self.operation = operation
        self.operand = operand
        self.worry_test = test
        self.worry_test_true = test_true
        self.worry_test_false = test_false
        self.num_items = 0
        self.PRINT = PRINT


    def inspect(self, stage, total_test_mod):
        operand = 0
        curr_worry = self.items.pop(0)

        curr_worry %= total_test_mod


        if self.operand == "old":
            operand = curr_worry
        else:
            operand = int(self.operand)

        if self.operation == "*":
            curr_worry = curr_worry * operand
        elif self.operation == "+":
            curr_worry = curr_worry + operand

        if stage == 1:
            curr_worry = curr_worry // 3

        ret = None
        if curr_worry % self.worry_test == 0:
            ret = (self.worry_test_true, curr_worry)
        else:
            ret =  (self.worry_test_false, curr_worry)

        return ret

    def count(self):
        self.num_items += len(self.items)

    def __str__(self) -> str:
        return f"Monkey {self.id}:\n  Starting items: {', '.join([str(item) for item in self.items])}\n  Operation: new = old {self.operation} {self.operand}\n  Test: divisble by {self.worry_test}\n    If true: throw to monkey {self.worry_test_true}\n    If false: throw to monkey {self.worry_test_false}\n"


def main(input_file, stage):
    with open(input_file, "r") as file:
        lines = file.readlines()
        monkeys = []

        PRINT = False

        total_test_mod = 1

        for line in range(0, len(lines), 7):
            starting_items = [int(item) for item in lines[line+1].replace("Starting items: ", "").split(", ")]
            operation = lines[line+2].replace("Operation: new = old ", "").split()
            test = int(lines[line+3].replace("Test: divisible by ", "").strip())

            total_test_mod *= test

            test_true = int(lines[line+4].replace("If true: throw to monkey ", "").strip())
            test_false = int(lines[line+5].replace("If false: throw to monkey ", "").strip())
            monkeys.append(Monkey(line // 7, starting_items, operation[0], operation[1], test, test_true, test_false, PRINT))




        rounds = 20 if stage == 1 else 10000
        for round in range(rounds):
            for idx, monkey in enumerate(monkeys):
                monkey.count()
                while monkey.items:
                    dst, item = monkey.inspect(stage, total_test_mod)
                    monkeys[dst].items.append(item)

        activities = []
        for idx, monkey in enumerate(monkeys):
            print(f"Monkey {idx} inspected items {monkey.num_items} times.")
            activities.append(monkey.num_items)

        activities.sort(reverse=True)
        print(activities[0] * activities[1])

if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time() - start}")
