#include <iostream>
#include <chrono>
#include <fstream>
#include <unordered_set>
#include <unordered_map>
#include <queue>
#include "../Utils/utils.h"
#include "../Utils/Vector.h"

using aoc_utils::Vector;
typedef std::vector<std::string> puzzle_t;

struct Position{
    Vector pos_;
    int dangerLevel_;

    Position(int y, int x, int dangerLevel) : pos_(x, y), dangerLevel_(dangerLevel) { }

    bool operator==(const Position& other) const {
        return pos_ == other.pos_ && dangerLevel_ == other.dangerLevel_;
    }
};


struct ComparePosition {
    bool operator()(const Position& a, const Position& b) {
        if(a.dangerLevel_ != b.dangerLevel_){
            return a.dangerLevel_ > b.dangerLevel_;
        }
        if(a.pos_.y != b.pos_.y){
            return a.pos_.y < b.pos_.y;
        }
        return a.pos_.x < b.pos_.x;
    }
};

int convertDangerLevel(const puzzle_t& puzzle, const Vector& pos, int stage){
    if(stage == 1){
        return (puzzle.at(pos.y).at(pos.x) - '0');
    }

    int x = pos.x % static_cast<int>(puzzle.at(0).length());
    int y = pos.y % static_cast<int>(puzzle.size());

    int tileX = pos.x / static_cast<int>(puzzle.at(0).length());
    int tileY = pos.y / static_cast<int>(puzzle.size());

    int increase = tileX+tileY;
    int lvl = puzzle.at(y).at(x) - '0';
    lvl += increase;
    lvl %= 9;

    // mod 9, but
    if(lvl == 0){
        return 9;
    }

    return lvl;
}


void solvePuzzle(const puzzle_t& puzzle, int stage){
    size_t maxY = puzzle.size() * (stage == 1 ? 1 : 5);
    size_t maxX = puzzle.at(0).length() * (stage == 1 ? 1 : 5);

    std::priority_queue<std::tuple<int, int, int>, std::vector<std::tuple<int, int, int>>, std::greater<> > queue;
    std::unordered_set<std::string> visited;


    // never entered
    queue.emplace(0, 0, 0); // start at (0, 0) with dangerLevel 0
    std::array<Vector, 4> directions = {
            Vector(0, 1),
            Vector(0, -1),
            Vector(1, 0),
            Vector(-1, 0)
    };

    while(!queue.empty()){
        auto [dangerLevel, x, y] = queue.top();
        queue.pop();
        Vector pos(x, y);
        if(visited.contains(pos.id())){
            continue;
        }
        if(pos.y == maxY-1 && pos.x == maxX - 1){
            std::cout << dangerLevel << "\n";
            return;
        }
        visited.insert(pos.id());
        for(auto& dir : directions){
            Vector newPos = pos + dir;

            if(newPos.x < 0 || newPos.x >= maxX || newPos.y < 0 || newPos.y >= maxX){
                continue;
            }
            int newDangerLvl = dangerLevel + convertDangerLevel(puzzle, newPos, stage);

            if(!visited.contains(newPos.id())){
                queue.emplace(newDangerLvl, newPos.x, newPos.y);
            }
        }
    }
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