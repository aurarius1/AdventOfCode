#include <iostream>
#include <chrono>
#include "utils.h"

void stage1(const std::vector<std::string>& puzzle){

}

void stage2(const std::vector<std::string>& puzzle){

}

int main(){
    std::vector<std::string> puzzle;
    std::string input = "example";
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