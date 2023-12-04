import re


def game_possible(draw, max_colors, get_power):
    colors = draw.replace(" ", "").split(",")
    for color in colors:
        matches = re.findall("[0-9]+", color)
        drawn_balls = int(matches[0])
        ball_color = color.replace(matches[0], "")
        if get_power:
            if drawn_balls > max_colors[ball_color]:
                max_colors[ball_color] = drawn_balls
        elif drawn_balls > max_colors[ball_color]:
            return 0
    return 1


def main(games, max_colors, get_power=False):
    id_sum = 0
    for game in games:
        gid, game = game.strip("\n").split(":")
        possible = [game_possible(draw, max_colors, get_power) for draw in game.split(";")]
        if get_power:
            id_sum += max_colors["red"] * max_colors["green"] * max_colors["blue"]
            max_colors = {"red": 0, "green": 0, "blue": 0}
        elif sum(possible) == len(game.split(";")):
            id_sum += int(gid.replace("Game ", ""))
    print(id_sum)


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    file = open("games", "r")
    read_games = file.readlines()
    file.close()
    main(read_games, {"red": 12, "green": 13, "blue": 14}, False)
    print("Stage 2\n")
    main(read_games, {"red": 0, "green": 0, "blue": 0}, True)


