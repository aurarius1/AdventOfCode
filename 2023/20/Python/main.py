import time
import math


class Message:
    def __init__(self, src, high=False):
        self.src = src
        self.high = high


class Module:
    def __init__(self, name, dst, sources, debug):
        self.name = name
        self.dst = dst
        self.sources = sources
        self.message = None
        self.debug = debug

    def send_pulse(self, pulses):
        if self.debug:
            print("default module's send_pulse method called")
        return []

    def recv(self, message, pulses):
        if self.debug:
            print("default module's recv method called")
        return self.send_pulse(pulses)


class FlipFlopModule(Module):
    def __init__(self, name, dst, debug=False):
        super().__init__(name, dst, [], debug)
        self.state = False

    def send_pulse(self, pulses):
        messages = []
        for dst in self.dst:
            if self.state:
                pulses["high"] += 1
            else:
                pulses["low"] += 1
            messages.append((dst, Message(self.name, self.state)))
        self.message = None
        return messages

    def recv(self, message, pulses):
        if self.debug:
            print("Message received by", self.name, "from", message.src, "pulse is high:", message.high)
        if not message.high:
            self.state = not self.state

        if message.high:
            return []

        self.message = message
        return self.send_pulse(pulses)


class ConModule(Module):
    def __init__(self, name, dst, sources, debug=False):
        super().__init__(name, dst, sources, debug)

    def send_pulse(self, pulses):
        messages = []

        response = not all(self.sources.values())
        if response:
            pulses["high"] += len(self.dst)
        else:
            pulses["low"] += len(self.dst)
        for dst in self.dst:
            messages.append((dst, Message(self.name, response)))
        return messages

    def recv(self, message, pulses):
        if self.debug:
            print("Message received by", self.name, "from", message.src, "pulse is high:", message.high)
        self.sources[message.src] = message.high
        return self.send_pulse(pulses)


class BroadcasterModule(Module):
    def __init__(self, name, dst, debug=False):
        super().__init__(name, dst, [], debug)

    def send_pulse(self, pulses):
        messages = []
        for dst in self.dst:
            messages.append((dst, Message(self.name, False)))

        pulses["low"] += len(messages)

        return messages

    # TODO theoretically, this could not a recv function (bc it recv from button, but the default recv from base
    # also calls the send_pulse, so not necessary)


class ButtonModule(Module):
    def __init__(self, debug=False):
        super().__init__("button", ["broadcaster"], [], debug)

    def send_pulse(self, pulses):
        messages = []
        for dst in self.dst:
            messages.append((dst, Message(self.name, high=False)))

        pulses["low"] += 1
        return messages


def build_module(module, con_module_inputs, debug=False):
    match module[0][0]:
        case "%":
            return FlipFlopModule(module[0][1:], module[1], debug)
        case "&":
            sources = con_module_inputs.get(module[0][1:], [])
            sources = {k: False for k in sources}

            return ConModule(module[0][1:], module[1], sources, debug)
        case "#":
            return BroadcasterModule(module[0][1:], module[1], debug)
        case _:
            return ButtonModule(debug)


def main(input_file, stage=1):
    debug = False
    with open(input_file) as file:
        modules = file.readlines()
        vis = False
        if vis:
            from graphviz import Source
            import os
            os.remove("./test.gv")
            s = Source.from_file("./graph.dot")
            s.render("test.gv", format="png", view=True)
            return

        def is_con(m): return m[0] == "&"
        def is_con_input(con, outputs): return con[0][1:] in outputs

        # prepare modules for processing, sort by reversed order to have broadcaster in first position
        modules = [module.replace(",", "").split() for module in sorted(modules, reverse=True)]
        # add symbol to broadcaster, to allow splitting later on
        modules[0][0] = "#" + modules[0][0]
        # group outputs into one list
        modules = [(module[0], [m for m in module[2:]]) for module in modules]

        # build input nodes for con modules
        con_inputs = {m[0][1:]: [i[0][1:] for i in modules if is_con_input(m, i[1])] for m in modules if is_con(m[0])}
        # build module classes
        modules = {module[0][1:]: build_module(module, con_inputs, debug) for module in modules}

        pulses = {"low": 0, "high": 0}
        kl_inputs = {}
        button_presses = 1000 if stage == 1 else 2**12  # max until one cycle should loop around
        i = 0
        while i < button_presses:
            button = build_module(["button"], {})
            queue = button.send_pulse(pulses)
            while queue:
                dst, message = queue.pop(0)
                if stage == 2 and dst == "kl":      # kl is before rx
                    if kl_inputs.get(message.src, None) is None and message.high:
                        kl_inputs[message.src] = i+1
                        if len(kl_inputs) == 4:
                            i = button_presses
                            break
                if (dst := modules.get(dst, None)) is None:
                    continue
                message = dst.recv(message, pulses)
                queue.extend(message)
            i += 1

        if stage == 1:
            print(pulses["low"] * pulses["high"])
        elif stage == 2:
            print(math.lcm(*kl_inputs.values()))


if __name__ == "__main__":
    use_example = False
    file_name = "example" if use_example else "input"

    start_time = time.time()
    main(file_name, 1)
    print(f"Stage 1 time: {time.time()-start_time:.10f}")

    start_time = time.time()
    main(file_name, 2)
    print(f"Stage 2 time: {time.time()-start_time:.10f}")
