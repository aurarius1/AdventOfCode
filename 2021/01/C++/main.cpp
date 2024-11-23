#include <iostream>
#include <chrono>
#include "utils.h"

void stage1(const std::vector<std::string>& puzzle){
    int increased = 0;
    for(int i = 1; i < puzzle.size(); i++){
        int prev = std::stoi(puzzle.at(i-1));
        int curr = std::stoi(puzzle.at(i));

        if(curr > prev){
            increased++;
        }
    }
    std::cout << increased << "\n";
}

void stage2(const std::vector<std::string>& puzzle){
    int increased = 0;
    for(int i = 0; i+3 < puzzle.size(); i += 1){
        int j = i + 1;
        int a = std::stoi(puzzle.at(i)) + std::stoi(puzzle.at(i+1)) + std::stoi(puzzle.at(i+2));
        int b = std::stoi(puzzle.at(j)) + std::stoi(puzzle.at(j+1)) + std::stoi(puzzle.at(j+2));

        if(b > a){
            increased++;
        }

    }
    std::cout << increased << "\n";
}

int main(){
    std::vector<std::string> puzzle;
    std::string input = "input";
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