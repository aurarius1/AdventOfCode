#include "utils.h"



namespace aoc_utils{

    void load_puzzle(const std::string& filename, std::vector<std::string>& puzzle){
        std::ifstream file(filename);
        if(!file.is_open()){
            std::cerr << "Error opening file\n";
            exit(-1);
        }
        std::string line;
        while(std::getline(file, line)){
            puzzle.push_back(line);
        }

        file.close();
    }


}