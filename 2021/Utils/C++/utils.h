#ifndef UTILS_H
#define UTILS_H

#include <vector>
#include <fstream>
#include <sstream>
#include <algorithm>
#include <vector>
#include <iostream>

namespace aoc_utils {

    void load_puzzle(const std::string& filename, std::vector<std::string>& puzzle);

    template <typename T>
    void splitString(const std::string& toSplit, char delim, std::vector<T>& out);



}

#endif // UTILS_H