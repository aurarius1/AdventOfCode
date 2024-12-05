#include <iostream>
#include <chrono>
#include "../Utils/utils.h"

void stage(const std::vector<std::string>& puzzle, int stage = 1){
    int horizontal = 0, depth = 0, aim = 0;
    for(auto& line : puzzle){
        std::stringstream command(line.c_str());
        std::string direction;
        int units;
        command >> direction >> units;

        if(direction == "forward"){
            horizontal += units;
            if(stage == 2){
                depth += (aim * units);
            }
        }
        if(direction == "down"){
            aim += units;
            if(stage == 1){
                depth += units;
            }
        }
        if(direction == "up"){
            aim -= units;
            if(stage == 1){
                depth -= units;
            }
        }
    }
    std::cout << horizontal * depth << "\n";
}

void stage2(const std::vector<std::string>& puzzle){
    int horizontal = 0, depth = 0, aim = 0;
    for(auto& line : puzzle){
        std::stringstream command(line.c_str());
        std::string direction;
        int units;
        command >> direction >> units;

        if(direction == "forward"){
            horizontal += units;

        }
        if(direction == "down"){

        }
        if(direction == "up"){
            aim -= units;
        }
    }
    std::cout << horizontal * depth << "\n";
}

int main(){
    std::vector<std::string> puzzle;
    std::string input = "input";
    aoc_utils::load_puzzle(input, puzzle);

    auto start = std::chrono::high_resolution_clock::now();
    stage(puzzle, 1);
    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> elapsed = end - start;
    std::cout << "Stage 1 took " << elapsed.count() << " seconds\n";


    start = std::chrono::high_resolution_clock::now();
    stage(puzzle, 2);
    end = std::chrono::high_resolution_clock::now();
    elapsed = end - start;
    std::cout << "Stage 2 took " << elapsed.count() << " seconds\n";
}