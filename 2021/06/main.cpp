#include <iostream>
#include <chrono>
#include <regex>
#include "../Utils/utils.h"
#include "../Utils/Vector.h"

void solvePuzzle(const std::vector<std::string>& puzzle, int stage){

    std::vector<size_t> lifetimes;
    aoc_utils::splitString(puzzle.at(0), ',', lifetimes);

    std::array<size_t, 9> fishPerDay = {0};
    for(auto& time : lifetimes){
        fishPerDay[time]++;
    }

    for(int i = 0; i < (stage == 1 ? 80 : 256); i++){
        size_t zero = fishPerDay[0];
        for(int j = 1; j < 9; j++){
            fishPerDay[j-1] = fishPerDay[j];
        }
        fishPerDay[6] += zero;
        fishPerDay[8] = zero; // that many fish are born this day;
    }

    size_t totalFishes = 0;
    for(int i = 0; i < 9; i++){
        totalFishes += fishPerDay[i];
    }
    std::cout << totalFishes << "\n";
}

void stage1(const std::vector<std::string>& puzzle){
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