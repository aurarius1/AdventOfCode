import time

class FSElement: 
    def __init__(self, name, is_directory):
        self.name = name
        self.is_directory = is_directory
        self.size = 0


class File(FSElement): 
    def __init__(self, name, size): 
        super().__init__(name, False)
        self.size = int(size)


class Directory(FSElement):
    def __init__(self, name, parent):
        super().__init__(name, True)
        self.name = name
        self.parent = parent
        self.children: list[FSElement] = []
        self.size = 0
    
    def new_file(self, file_size, file_name):
        self.children.append(File(file_name, file_size))
        self.size += self.children[-1].size

    def new_dir(self, dir):
        self.children.append(dir)

    def directories(self):
        return [child for child in self.children if child.is_directory]

    def files(self):
        return [child for child in self.children if not child.is_directory]

    def dir_size(self):
        return sum([child.dir_size() for child in self.children if child.is_directory]) + self.size

    def get_dir(self, dir):
        if dir == "..":
            return self.parent
        for child in self.children:
            if child.is_directory and child.name == dir:
                return child



def print_directories(directory, depth=0):
    print(f"  "*depth, "-", directory.name, "(dir)")
    for dir in directory.directories():
        print_directories(dir, depth+1)

    for file in directory.files():
        print(f"  "*depth+"  ", "-", f"{file.name} (file, size={file.size})")


def get_removable_directories(directory, dir_size = 100000, key=lambda x, y: x <= y): 
    removable_directories = []
    queue = [directory]

    while queue: 
        dir = queue.pop()
        if key(dir.dir_size(), dir_size):
            removable_directories.append(dir)
        queue.extend(dir.directories())

    return removable_directories
    

def main(input_file, stage): 
    root = Directory("/", None)
    current_directory = root

    with open(input_file, "r") as file: 
        terminal_stream = [line.strip("\n") for line in file.readlines()]
        #current_directory = "/"
        
        # BUILD FS
        for line, out in enumerate(terminal_stream): 
            if line == 0: 
                continue
            if out.startswith("$ ls"): 
                # nothing to do here
                pass
            elif out.startswith("$ cd"):
                current_directory = current_directory.get_dir(out[len("$ cd "):])
            elif out.startswith("dir "):
                current_directory.new_dir(Directory(out[len("dir "):], current_directory))
            else: 
                current_directory.new_file(*out.split())
       
        if stage == 1:
            removable_dirs = get_removable_directories(root)
            print(sum([removable_dir.dir_size() for removable_dir in removable_dirs]))
            return
        
        fs_size = 70000000
        update_space = 30000000
        unused_space = fs_size - root.dir_size()
        needed_space = update_space-unused_space

        removable_dirs = get_removable_directories(root, needed_space, key=lambda x, y: x >= y)
        print(min([removable_dir.dir_size() for removable_dir in removable_dirs]))


if __name__ == "__main__": 
    use_example = False
    file_name = "example" if use_example else "input"
    
    start = time.time()
    main(file_name, 1) 
    print(f"Stage 1 time: {time.time() - start}")

    start = time.time()
    main(file_name, 2) 
    print(f"Stage 2 time: {time.time() - start}")