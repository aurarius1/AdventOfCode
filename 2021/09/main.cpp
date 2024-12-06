#include <iostream>
#include <chrono>
#include <regex>
#include <unordered_set>
#include <cmath>
#include "../Utils/utils.h"

std::array<std::tuple<int, int>, 4> directions = {
        std::make_tuple(-1, 0),
        std::make_tuple(1, 0),
        std::make_tuple(0, -1),
        std::make_tuple(0, 1)
};

void stage1(const std::vector<std::string>& puzzle){

    int riskLevel = 0;
    for(int row = 0; row < puzzle.size(); row++){
        for(int col = 0; col < puzzle.at(row).length(); col++){
            int lower = 0;
            for(const auto& [dRow, dCol] : directions){
                int newRow = row+dRow, newCol = col+dCol;
                if(newRow < 0 || newRow >= puzzle.size() || newCol < 0 || newCol >= puzzle.at(newRow).length()){
                    lower++; // counts as valid
                    continue;
                }
                if(puzzle.at(row).at(col) < puzzle.at(newRow).at(newCol)){
                    lower++;
                }
            }
            if(lower == 4){
                riskLevel += (puzzle.at(row).at(col) - '0' + 1);

            }
        }
    }
    std::cout << riskLevel << "\n";
}

std::unordered_set<std::string> findBasinSize(int row, int col, std::tuple<int, int> prev_dir, const std::vector<std::string>& puzzle){
    if(puzzle.at(row).at(col) == '9'){
        return {};
    }
    std::unordered_set<std::string> basinSpots = {std::to_string(row) + std::to_string(col)};
    for(const auto& [dRow, dCol] : directions){
        // don't go back
        if(dRow*-1 == std::get<0>(prev_dir) && dCol*-1 == std::get<1>(prev_dir)){
            continue;
        }
        int newRow = row+dRow, newCol = col+dCol;
        if(newRow >= puzzle.size() || newRow < 0 || newCol >= puzzle.at(row).length() || newCol < 0){
            continue;
        }
        if(puzzle.at(newRow).at(newCol) <= puzzle.at(row).at(col)){
            continue;
        }
        auto spots = findBasinSize(newRow, newCol, std::make_tuple(dRow, dCol), puzzle);
        basinSpots.insert(spots.begin(), spots.end());
    }
    return basinSpots;
}


void stage2(const std::vector<std::string>& puzzle){
    std::vector<int> basinSizes;
    for(int row = 0; row < puzzle.size(); row++){
        for(int col = 0; col < puzzle.at(row).length(); col++){
            int lower = 0;
            for(const auto& [dRow, dCol] : directions){
                int newRow = row+dRow, newCol = col+dCol;
                if(newRow < 0 || newRow >= puzzle.size() || newCol < 0 || newCol >= puzzle.at(newRow).length()){
                    lower++; // counts as valid
                    continue;
                }
                if(puzzle.at(row).at(col) < puzzle.at(newRow).at(newCol)){
                    lower++;
                }
            }
            if(lower == 4){
                auto basin = findBasinSize(row, col, std::make_tuple(0, 0), puzzle);
                basinSizes.push_back(static_cast<int>(basin.size()));
            }
        }
    }
    std::sort(basinSizes.begin(), basinSizes.end(), std::greater<>());
    int basins = basinSizes.at(0) * basinSizes.at(1) * basinSizes.at(2);
    std::cout << basins << "\n";
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