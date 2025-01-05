#include <iostream>
#include <chrono>
#include <queue>
#include "../Utils/utils.h"
#include "../Utils/types.h"

void stage1(const puzzle_t& puzzle) {

    // if init_y > 0, y = 0 can be reached within -init_y steps
    // this means the first position where y < 0 is (-init_y-1)
    // y = zero means we hit the starting row again. so in the next step we want to maximize our fall to the lowest
    // point inside the target area. this would be min_target_y (or the bottom edge)
    // as y < 0 ==> -init_y-1 we can say: -init_y-1 = min_target_y
    // therefore we can calculate our initial velocity: init_y = abs(min_target_y) - 1
    // the maximum height is just the sum of all numbers between 1 and init_y:
    // vy_init + (vy_init-1) + ... + 1 + 0 --> height = vy_init*(vy_init+1)/2
    // now substitute vy_init with abs(min_target_y)-1: (abs(min_target_y)-1)*(abs(min_target_y)-1+1)/2
    // => (abs(min_target_y)-1)*(abs(min_target_y))/2
    // see this: (https://gist.github.com/mdarrik/72835482b47e9b3e2827faa5789f8e6a)
    std::string y = puzzle.at(0).substr(puzzle.at(0).find("y=") + 2);
    int yMin = std::stoi(y.substr(0, y.find('.')));
    std::cout << (std::abs(yMin)-1)*(std::abs(yMin))/2 << "\n";
}
void stage2(const puzzle_t& puzzle) {
    std::string y = puzzle.at(0).substr(puzzle.at(0).find("y=") + 2);
    std::string x = puzzle.at(0).substr(puzzle.at(0).find("x=") + 2, puzzle.at(0).find(','));

    int yMin = std::stoi(y.substr(0, y.find('.')));
    int yMax = std::stoi(y.substr(y.find('.')+2));
    int xMin = std::stoi(x.substr(0, x.find('.')));
    int xMax = std::stoi(x.substr(x.find('.')+2));

    int points = 0;
    // y boundaries: yMin (either we throw directly at the bottom edge of the target area) to solution from assignment 1
    // x boundaries:
    // if x < 0, go until zero --> never able to hit the target, wrong direction
    // if x > xMax --> never able to hit the target, overthrown

    for(int yAccel = yMin; yAccel <= (std::abs(yMin)-1)*(std::abs(yMin))/2; yAccel++){
        for(int xAccel = 0; xAccel <= xMax; xAccel++){
            int startX = 0;
            int startY = 0;

            int currXAccel = xAccel;
            int currYAccel = yAccel;
            while(startX <= xMax && startY >= yMin){
                if(startX >= xMin && startX <= xMax && startY >= yMin && startY <= yMax){
                    points++;
                    break;
                }
                startX += currXAccel;
                startY += currYAccel;
                if(currXAccel > 0){
                    currXAccel--;
                }
                currYAccel--;
            }
        }
    }
    std::cout << points << "\n";
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