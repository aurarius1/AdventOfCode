#include <iostream>
#include <chrono>
#include <fstream>
#include <unordered_set>
#include <unordered_map>
#include "../Utils/utils.h"
#include "../Utils/Vector.h"

typedef std::vector<std::string> puzzle_t;

void solvePuzzle(const puzzle_t& puzzle, int stage){
    std::string polymer = *puzzle.begin();
    std::map<std::string, char> insertionRules;
    std::map<std::string, size_t> polymerPairs;
    std::map<char, size_t> elementCount;

    for (auto it = puzzle.begin() + 2; it != puzzle.end(); ++it) {
        char toInsert = (*it).substr((*it).find(" -> ") + 4, 1)[0];
        std::string insertTemplate = (*it).substr(0, 2);
        insertionRules[insertTemplate] = toInsert;
    }

    for(int i = 0; i < polymer.length()-1;i++){
        polymerPairs[polymer.substr(i, 2)]++;
        elementCount[polymer[i]]++;
    }
    elementCount[polymer.back()]++;

    for(int i = 0; i < ((stage == 1) ? (10) : (40)); i++){
        std::map<std::string, size_t> newPolymerPairs(polymerPairs);
        for(auto& [key, value] : polymerPairs){
            if(value == 0 || insertionRules.find(key) == insertionRules.end()){
                continue;
            }
            char newElement = insertionRules[key];
            elementCount[newElement] += value;
            newPolymerPairs[key] -= value;
            newPolymerPairs[std::string{key[0], newElement}] += value;
            newPolymerPairs[std::string{newElement, key[1]}] += value;
        }
        polymerPairs = std::move(newPolymerPairs);
    }

    size_t maxElement = elementCount.begin()->second;
    size_t minElement = elementCount.begin()->second;
    for (auto it = std::next(elementCount.begin()); it != elementCount.end(); it++) {
        maxElement = std::max(maxElement, it->second);
        minElement = std::min(minElement, it->second);
    }

    std::cout << maxElement - minElement << "\n";
}

void stage1(const puzzle_t& puzzle) {
    solvePuzzle(puzzle, 1);
}

void stage2(const puzzle_t& puzzle){
    solvePuzzle(puzzle, 2);
}

int main(){
    puzzle_t puzzle;
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