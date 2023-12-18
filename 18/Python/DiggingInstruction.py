class DiggingInstruction:
    def __init__(self, input_string, stage):
        self.direction, self.length, self.color = input_string.split()
        self.color = self.color.replace("(", "").replace(")", "").replace("#", "")
        self.length = int(self.length)

        direction_map = {"0": "R", "1": "D", "2": "L", "3": "U"}

        if stage == 2:
            self.length = int(self.color[:-1], 16)
            self.direction = direction_map[self.color[-1]]
