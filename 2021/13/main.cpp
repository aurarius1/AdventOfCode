#include <iostream>
#include <chrono>
#include <fstream>
#include <unordered_set>
#include "../Utils/utils.h"
#include "../Utils/Vector.h"

using aoc_utils::Vector;

struct Folds {
    int pos = 0;
    char dir;

    Folds(int position, char direction) : pos(position), dir(direction){}
};

void writeMapToFile(const std::vector<char>& map, int maxX, int maxY, const std::string& filename) {
    std::ofstream outFile(filename);
    if (!outFile.is_open()) {
        exit(-1);
    }
    for (int row = 0; row < maxY; row++) {
        for (int col = 0; col < maxX; col++) {
            char currentChar = map[row * maxX + col];
            outFile << currentChar;
        }
        outFile << "\n";
    }
    outFile.close();
    std::cout << "Map written to " << filename << " successfully.\n";
}

void solvePuzzle(const std::vector<std::string>& puzzle, int stage){
    int maxY = 0, maxX = 0;
    std::vector<Folds> folds;
    std::vector<Vector> points;

    for(auto& line : puzzle){
        if(line.empty()){
            continue;
        }
        if(line.starts_with("fold")){
            size_t equalsIdx = line.find('=');
            int fold = std::stoi(line.substr(equalsIdx+1));
            folds.emplace_back(fold, line[equalsIdx-1]);
            continue;
        }
        points.emplace_back(line);
        maxY = std::max(maxY, points.back().y);
        maxX = std::max(maxX, points.back().x);
    }
    maxY++;
    maxX++;
    std::vector<char> map((maxY) * (maxX), '.');

    for(auto& fold : folds){
        for(auto& point : points){
            int yAfterFold = point.y;
            int xAfterFold = point.x;
            if(fold.dir == 'y' && point.y > fold.pos){
                yAfterFold = fold.pos - (point.y - fold.pos);
            }
            if(fold.dir == 'x' && point.x > fold.pos){
                xAfterFold = fold.pos - (point.x - fold.pos);
            }

            point.y = yAfterFold;
            point.x = xAfterFold;
        }
        if(stage == 1){
            break;
        }
    }

    for(auto& point : points){
        map[point.y * (maxX) + point.x] = '#';
    }

    if(stage == 1){
        int visiblePoints = 0;
        for(int row = 0; row < maxY; row++){
            for(int col = 0; col < maxX; col++){
                if(map[row*maxX + col] == '#'){
                    visiblePoints++;
                }
            }
        }
        std::cout << visiblePoints << "\n";
    }
    if(stage == 2){
        writeMapToFile(map, maxX, maxY, "output_map.txt");
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
    std::vector<std::string> curr;
    curr.resize(2);


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