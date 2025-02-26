#include <iostream>
#include <chrono>
#include <regex>
#include "../Utils/utils.h"
#include "../Utils/Vector.h"

using aoc_utils::Vector;



struct HydrothermalVent{
    Vector start, end;

    explicit HydrothermalVent(const std::vector<std::string>& coordinates) : start(coordinates.at(0)), end(coordinates.at(1)){

    }

    [[nodiscard]] bool isDiagonal() const{
        return !(start.x == end.x || start.y == end.y);
    }

    friend std::ostream& operator<<(std::ostream& os, const HydrothermalVent& vent);
};

std::ostream &operator<<(std::ostream &os, const HydrothermalVent &vent) {
    os << vent.start << " -> " << vent.end;
    return os;
}


void solvePuzzle(const std::vector<std::string>& puzzle, int stage){
    std::vector<HydrothermalVent> vents;
    int maxY = 0, maxX = 0;
    for(auto& line : puzzle){
        std::string test = std::regex_replace(line, std::regex(" -> "), "-");
        std::vector<std::string> coordinates;
        aoc_utils::splitString(test, '-', coordinates);
        vents.emplace_back(coordinates);
        maxY = std::max({maxY, vents.back().start.y, vents.back().end.y});
        maxX = std::max({maxX, vents.back().start.x, vents.back().end.x});
    }

    if(stage == 1){
        std::erase_if(vents, [](const HydrothermalVent &vent) {
            return vent.isDiagonal();
        });
    }

    std::vector<int> map((maxY+1)*(maxX+1), 0);
    for(auto& vent : vents){
        Vector direction(vent.end.x - vent.start.x, vent.end.y - vent.start.y);
        direction.normalize();
        while(vent.start != vent.end){
            map.at(maxY*(vent.start.y) + (vent.start.x))++;
            vent.start += direction;
        }
        map.at(maxY*(vent.start.y) + (vent.start.x))++;
    }

    int intersections = std::count_if(map.begin(), map.end(), [](int field) {
        return field > 1;
    });
    std::cout << "Intersections: " << intersections << "\n";
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