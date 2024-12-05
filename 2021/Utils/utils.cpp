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

    template <typename T>
    void splitString(const std::string& toSplit, char delim, std::vector<T>& out){
        std::stringstream stream(toSplit);
        out.reserve(std::count(toSplit.begin(), toSplit.end(), delim) + 1);
        std::string token;
        while (getline(stream, token, delim)) {
            // emplace_back adds elements directly to the vector
            std::stringstream tokenStream(token);
            T value;
            if (!(tokenStream >> value)) {
                throw std::invalid_argument("Failed to convert token to desired type");
            }
            out.push_back(value);
        }
    }
    template void splitString<std::string>(const std::string& toSplit, char delim, std::vector<std::string>& out);
    template void splitString<int>(const std::string& toSplit, char delim, std::vector<int>& out);
    template void splitString<size_t>(const std::string& toSplit, char delim, std::vector<size_t>& out);



}