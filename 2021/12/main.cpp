#include <iostream>
#include <chrono>
#include <regex>
#include <unordered_set>
#include "../Utils/utils.h"

std::map<std::string, std::vector<std::string>> paths;

int findPaths(const std::string& curr, std::string visitedCaves, bool lowerCaseTwice = false, int stage = 1){
    if(curr == "end"){
        return 1;
    }
    if(curr == "start" && !visitedCaves.empty()){
        return 0;
    }

    bool isLower = std::all_of(curr.begin(), curr.end(), [](char ch) {
        return std::islower(static_cast<unsigned char>(ch));
    });
    if(isLower && visitedCaves.find(curr + ",") != std::string::npos){
        if(stage == 1 || lowerCaseTwice){
            return 0;
        }
        lowerCaseTwice = true;
    }
    visitedCaves += curr + ",";

    const std::vector<std::string>& nextMoves = paths[curr];
    int possiblePaths = 0;
    for(const auto& nextMove : nextMoves){
        possiblePaths += findPaths(nextMove, visitedCaves, lowerCaseTwice, stage);
    }
    return possiblePaths;
}

void printPaths() {
    for (const auto& [key, values] : paths) {
        std::cout << key << ": ";
        for (size_t i = 0; i < values.size(); ++i) {
            std::cout << values[i];
            if (i < values.size() - 1) { // Add a comma if it's not the last value
                std::cout << ", ";
            }
        }
        std::cout << "\n";
    }
}

void stage1() {
    std::cout << findPaths("start", "") << "\n";
}

void stage2(){
    std::cout << findPaths("start", "", false, 2) << "\n";
}

int main(){
    std::vector<std::string> puzzle;
    std::string input = "example";
    input = "input";
    aoc_utils::load_puzzle(input, puzzle);
    std::vector<std::string> curr;
    curr.resize(2);
    for(auto& line : puzzle){
        curr.clear();
        aoc_utils::splitString(line, '-', curr);
        paths[curr[0]].push_back(curr[1]);
        paths[curr[1]].push_back(curr[0]);
    }

    auto start = std::chrono::high_resolution_clock::now();
    stage1();
    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> elapsed = end - start;
    std::cout << "Stage 1 took " << elapsed.count() << " seconds\n";

    start = std::chrono::high_resolution_clock::now();
    stage2();
    end = std::chrono::high_resolution_clock::now();
    elapsed = end - start;
    std::cout << "Stage 2 took " << elapsed.count() << " seconds\n";
}