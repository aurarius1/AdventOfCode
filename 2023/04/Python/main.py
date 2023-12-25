import math


def main(stage=1):
    with open("input", "r") as file:
        cards = file.readlines()
        won_cards = {k + 1: 0 for k in range(len(cards))}
        for card in cards:
            card, numbers = card.strip().split(":")
            card = int(card.split()[-1])
            winning_numbers, numbers = numbers.split("|")
            num_own_winning_numbers = len(set(winning_numbers.split()).intersection(numbers.split()))
            won_cards[card] += math.floor(pow(2, num_own_winning_numbers - 1)) if stage == 1 else 1

            if stage == 1:
                continue

            for won_card in range(1, num_own_winning_numbers + 1):
                won_cards[won_card + card] += won_cards.get(card, 0)

        print(sum(won_cards.values()))


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    main(2)

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
