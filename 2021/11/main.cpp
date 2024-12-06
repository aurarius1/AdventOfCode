#include <iostream>
#include <chrono>
#include <regex>
#include <unordered_set>
#include "../Utils/utils.h"
#include "../Utils/Vector.h"


using aoc_utils::Vector;

void solvePuzzle(const std::vector<std::string>& puzzle, int stage){
    std::vector<std::vector<int>> octopuses;
    octopuses.resize(puzzle.size());
    for(int row = 0; row < puzzle.size(); row++){
        octopuses.at(row).reserve(puzzle.at(row).length());
        for(const auto& octopus : puzzle.at(row)){
            octopuses.at(row).push_back(octopus-'0');
        }
    }

    std::unordered_set<std::string> flashed = {};
    int flashCount = 0;
    int step = 0;
    while(stage == 1 ? step < 100 : flashCount != 10*10){
        step++;
        flashCount = stage == 2 ? 0 : flashCount;
        flashed.clear();
        bool octopusFlashed = false;
        for(auto &row : octopuses){
            for(int & octopus : row){
                octopus++;
                octopusFlashed |= octopus > 9;
            }
        }
        while(octopusFlashed){
            octopusFlashed = false;
            for(int row = 0; row < octopuses.size(); row++){
                for(int col = 0; col < octopuses.at(row).size(); col++){
                    Vector curr(col, row);
                    if(octopuses.at(curr.y).at(curr.x) < 10) {
                        continue;
                    }
                    octopuses.at(curr.y).at(curr.x) = 0;
                    if(flashed.contains(curr.id())){
                        continue;
                    }
                    octopusFlashed = true;
                    flashCount++;
                    flashed.insert(curr.id());
                    for(int dRow = -1; dRow < 2; dRow++){
                        for(int dCol = -1; dCol < 2; dCol++){
                            if(dCol == 0 && dRow == 0) {
                                continue;
                            }
                            Vector next(curr.x + dCol, curr.y + dRow);
                            if(next.y < 0 || next.y >= octopuses.size() || next.x < 0 || next.x >= octopuses.at(next.y).size()){
                                continue;
                            }
                            if(!flashed.contains(next.id())){
                                octopuses.at(next.y).at(next.x)++;
                            }
                        }
                    }
                }
            }
        }
    }
    if(stage == 1){
        std::cout << flashCount << "\n";
    } else {
        std::cout << step << "\n";
    }

}

void stage1(const std::vector<std::string>& puzzle) {
    solvePuzzle(puzzle, 1);
}

void stage2(const std::vector<std::string>& puzzle){
    solvePuzzle(puzzle, 2);
}

int main(){
    std::vector<std::string> puzzle;
    std::string input = "example";
    input = "input";
    aoc_utils::load_puzzle(input, puzzle);

    auto start = std::chrono::high_resolution_clock::now();
    stage1(puzzle);
    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> elapsed = end - start;
    std::cout << "Stage 1 took " << elapsed.count() << " seconds\n";


    start = std::chrono::high_resolution_clock::now();
    stage2(puzzle);
    end = std::chrono::high_resolution_clock::now();
    elapsed = end - start;
    std::cout << "Stage 2 took " << elapsed.count() << " seconds\n";
}