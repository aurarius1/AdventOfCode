import time
import re
import operator


workflows = {}


def test_all_workflows(ratings, current_workflow="in"):
    global workflows
    combinations = 0
    if current_workflow == "A":
        combinations = 1
        for r in ratings:
            combinations *= (r[1][1] - r[1][0] + 1)
        return combinations
    if current_workflow == "R":
        return 0
    for workflow in workflows[current_workflow]:
        if len(workflow) == 1:
            combinations += test_all_workflows(ratings, workflow[0])
            continue

        rating = None
        for r in range(len(ratings)):
            if ratings[r][0] == workflow[0]:
                rating = r
                break
        curr_rating = ratings[rating]

        if workflow[1](curr_rating[1][0], workflow[2]):
            ratings[rating] = (curr_rating[0], (curr_rating[1][0], workflow[2] - 1))
            combinations += test_all_workflows(ratings.copy(), workflow[3])
            ratings[rating] = (curr_rating[0], (workflow[2], curr_rating[1][1]))
        elif workflow[1](curr_rating[1][1], workflow[2]):
            ratings[rating] = (curr_rating[0], (workflow[2] + 1, curr_rating[1][1]))
            combinations += test_all_workflows(ratings.copy(), workflow[3])
            ratings[rating] = (curr_rating[0], (curr_rating[1][0], workflow[2]))

    return combinations


def main(input_file, stage=1):
    global workflows
    with open(input_file) as file:
        puzzle_input = file.readlines()
        part_ratings = []
        for workflow in puzzle_input[:puzzle_input.index("\n")]:
            name, rules = workflow.strip().split("{")
            workflows[name] = []
            rules = rules.replace("}", "").split(",")
            for rule in rules:
                rule_op = re.search(r">|<", rule)
                if rule_op is not None:
                    rule_op = rule_op.group()
                    part, rule = rule.split(rule_op)
                    amount, next_rule = rule.split(":")
                    rule_op = operator.gt if rule_op == ">" else operator.lt
                    workflows[name].append([part, rule_op, int(amount), next_rule])
                else:
                    workflows[name].append([rule])

        if stage == 2:
            ratings = [
                ("x", (1, 4000)),
                ("m", (1, 4000)),
                ("a", (1, 4000)),
                ("s", (1, 4000)),
            ]

            print(test_all_workflows(ratings))
            return

        for part_rating in puzzle_input[puzzle_input.index("\n")+1:]:
            part_rating = part_rating.strip().replace("{", "").replace("}", "")
            parts = part_rating.split(",")
            part_rating = {}
            for part in parts:
                part, num_parts = part.split("=")
                part_rating[part] = int(num_parts)
            part_ratings.append(part_rating)

        accepted = 0
        for part in part_ratings:
            current_workflow = "in"
            while current_workflow not in ["R", "A"]:
                # THIS COULD BE DONE RECURSIVELY, BUT I DON'T WANT TO
                for workflow in workflows[current_workflow]:
                    if len(workflow) == 1:
                        current_workflow = workflow[0]
                        continue
                    if workflow[0] not in part.keys():
                        continue

                    if workflow[1](part[workflow[0]], workflow[2]):
                        current_workflow = workflow[3]
                        break

            if current_workflow == "A":
                accepted += sum(part.values())

        print(accepted)


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")

    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
