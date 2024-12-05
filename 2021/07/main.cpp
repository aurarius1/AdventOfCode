#include <iostream>
#include <chrono>
#include <regex>
#include "../Utils/utils.h"
#include "../Utils/Vector.h"


int getFuelConsumption(int position, const std::vector<int>& crabs, int stage) {
    int consumption = 0;
    for(auto& crab : crabs){
        int steps = std::abs(crab - position);
        if(stage == 1){
            consumption += steps;
        } else {
            consumption += ((steps * (steps+1))/2);
        }
    }
    return consumption;
}

void solvePuzzle(const std::vector<std::string>& puzzle, int stage){
    std::vector<int> crabs;
    aoc_utils::splitString(puzzle.at(0), ',', crabs);
    int minPos = crabs.at(0), maxPos = crabs.at(0);
    for(auto& crab : crabs){
        minPos = std::min(crab, minPos);
        maxPos = std::max(crab, maxPos);
    }

    int minFuelConsumption = getFuelConsumption(0, crabs, stage), pos = 0;
    for(int i = 1; i <= maxPos; i++){
        int fuelConsumption = getFuelConsumption(i, crabs, stage);
        if(fuelConsumption < minFuelConsumption){
            minFuelConsumption = fuelConsumption;
            pos = i;
        }
    }
    std::cout << "Aligning at " << pos << " results in lowest total consumption of: " << minFuelConsumption << "\n";
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