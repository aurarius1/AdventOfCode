import time

hand_types = {"five_of_a_kind": 6, "four_of_a_kind": 5, "full_house": 4, "three_of_a_kind": 3, "two_pair": 2, "one_pair": 1, "high_card": 0, "nothing": -1}
card_values = ["T", "J", "Q", "K", "A"]


def card_value(value, stage=1):
    global card_values
    if str.isnumeric(value):
        return int(value)
    elif value == "J":
        return card_values.index(value)+10 if stage == 1 else 1 # J in stage1 is handled different
    else:
        return card_values.index(value)+10


class Hand:
    def __init__(self, hand, points, stage=1):
        global hand_types
        self.points = int(points)
        self.hand = [card_value(card, stage) for card in hand]
        hand_set = list(set(hand))
        card_counts = [hand.count(card) for card in hand_set]
        num_joker = hand.count("J")
        hand_type = "high_card"

        match len(hand_set):
            case 1:
                hand_type = "five_of_a_kind"
            case 2:
                if card_counts.count(4) == 1:
                    hand_type = "four_of_a_kind"
                elif card_counts.count(3) == 1:
                    hand_type = "full_house"
            case 3:
                if card_counts.count(3) == 1:
                    hand_type = "three_of_a_kind"
                elif card_counts.count(2) == 2:
                    hand_type = "two_pair"
            case 4:
                hand_type = "one_pair"

        if stage == 2 and num_joker > 0:
            if hand_type == "four_of_a_kind" or hand_type == "full_house":
                hand_type = "five_of_a_kind"
            elif hand_type == "three_of_a_kind":
                hand_type = "four_of_a_kind"
            elif hand_type == "two_pair":
                hand_type = "full_house" if num_joker == 1 else "four_of_a_kind"
            elif hand_type == "one_pair":
                hand_type = "three_of_a_kind"
            elif hand_type == "high_card":
                hand_type = "one_pair"

        self.type = hand_types[hand_type]


def main(input_file, stage=1):
    with open(input_file) as file:
        hands = file.readlines()
        hands = [Hand(*hand.split(), stage) for hand in hands]
        hands.sort(key=lambda x: (x.type, x.hand))
        points = sum([(i+1)*hand.points for i, hand in enumerate(hands)])
        print(points)


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start = time.time()
    main(file_name, 1) # sol: 249204891
    print(f"Stage 1 time: {time.time()-start:.10f}")

    start = time.time()
    main(file_name, 2) # sol: 249204891
    print(f"Stage 2 time: {time.time()-start:.10f}")
