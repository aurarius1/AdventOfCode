#include <iostream>
#include <chrono>
#include <queue>
#include <format>
#include <cmath>
#include "../Utils/utils.h"
#include "../Utils/types.h"

std::string addSnailfishNumbers(std::string n1, std::string n2){
    return std::format("[{},{}]", n1, n2);
}

int replaceNumber(std::string& number, int startIdx, int toAdd){
    int endIdx = startIdx;
    while(endIdx < number.length() && std::isdigit(number[endIdx])){
        endIdx++;
    }
    int length = endIdx-startIdx;
    std::string n = std::to_string(std::stoi(number.substr(startIdx, length)) + toAdd);
    number.replace(startIdx, length, n);
    return (int)n.length() - length;
}

void explode(std::string& number, int explosionIndex, int idxFirstNumberLeft){
    int numberEnd = explosionIndex;
    while(number[numberEnd] != ']'){
        numberEnd++;
    }

    int length = numberEnd-explosionIndex+1;
    std::vector<int> numbers;
    numbers.reserve(2);
    aoc_utils::splitString(number.substr(explosionIndex+1, length-1),',', numbers);
    number.replace(explosionIndex, length, "0");

    explosionIndex+=1;

    while(explosionIndex < number.length() && !std::isdigit(number[explosionIndex])){
        explosionIndex++;
    }

    if(explosionIndex < number.length()){
        replaceNumber(number, explosionIndex, numbers[1]);
    }

    if(idxFirstNumberLeft != -1){
        replaceNumber(number, idxFirstNumberLeft, numbers[0]);
    }
}

bool split(std::string& number, int startIdx){
    int endIdx = startIdx;
    // if the snailfish number is correctly build, we cannot go oob here
    while(std::isdigit(number[endIdx])){
        endIdx++;
    }
    int length = endIdx-startIdx;
    if(length <= 1){
        return false;
    }
    double n = std::stof(number.substr(startIdx, length))/2;
    int left = (int)std::floor(n);
    int right = (int)std::ceil(n);

    number.replace(startIdx, length, std::format("[{},{}]", left, right));
    return true;
}

void reduceSnailfishNumber(std::string& number){
    // test for explosion
    bool explosion, changed ;
    do{
        changed = false;
        do {
            explosion = false;
            int idxFirstRegularNumberLeft = -1;
            int depth = 0;
            for(int i = 0; i < number.length(); i++){
                switch(number[i]){
                    case '[':
                        depth++;
                        break;
                    case ']':
                        depth--;
                        continue;
                    case ',':
                        break;
                    default:
                        continue;
                }
                if(depth == 5){
                    explode(number, i, idxFirstRegularNumberLeft);
                    explosion = true;
                    break;
                }
                if(std::isdigit(number[i+1])){
                    idxFirstRegularNumberLeft = i+1;
                }
            }
        } while(explosion);
        for(int i = 0; i < number.length(); i++){
            if(split(number, i)){
                changed = true;
                break;
            }
        }
    } while(changed);
}

size_t magnitude(const std::string& number){
    if(number[0] != '['){
        return std::stoull(number);
    }
    int openedBrackets = 0;
    int i = 1;
    for(; i < number.length()-1; i++){
        switch(number[i]){
            case '[':
                openedBrackets++;
                continue;
            case ']':
                openedBrackets--;
                continue;
            case ',':
                break;
            default:
                continue;
        }

        if(openedBrackets == 0){
            break;
        }
    }
    std::string left = number.substr(1, i-1);
    std::string right = number.substr(i+1, number.length()-i-1);
    return magnitude(left)*3 + magnitude(right)*2;
}

void stage1(const puzzle_t& puzzle) {
    std::string curr = *puzzle.begin();
    for(auto it = puzzle.begin()+1; it < puzzle.end(); it++){
        curr = addSnailfishNumbers(curr, *it);
        reduceSnailfishNumber(curr);
    }
    std::cout << magnitude(curr) << "\n";
}
void stage2(const puzzle_t& puzzle) {
    std::string maxSum;
    size_t maxMagnitude = 0;

    for(int i = 0; i < puzzle.size(); i++){
        for(int j = 0; j < puzzle.size(); j++){
            if(i == j) {
                continue;
            }
            std::string currSum = addSnailfishNumbers(puzzle.at(i), puzzle.at(j));
            reduceSnailfishNumber(currSum);
            size_t currMagnitude = magnitude(currSum);

            if(currMagnitude > maxMagnitude){
                maxSum = currSum;
                maxMagnitude = currMagnitude;
            }
        }
    }
    std::cout << maxMagnitude << "\n";
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