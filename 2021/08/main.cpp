#include <iostream>
#include <chrono>
#include <regex>
#include <unordered_set>
#include <cmath>
#include "../Utils/utils.h"

// maps active lines to a number (first is number of active lines, second is actual digit)
std::map<int, int> activeLinesPerDigit = {
        {7, 8},
        {3, 7},
        {4, 4},
        {2, 1}
};


/*
    a
  ------
  |    |
 f|  g |b
  ------
  |    |
 e|    |c
  ------
    d
 */
struct Segment {
    std::unordered_set<char> a, b, c, d, e, f, g;

    int getDigit(const std::string& active){
        int digitLength = static_cast<int>(active.length());
        switch(digitLength){
            case 6:
                if(active.find(*b.begin()) == std::string::npos){
                    return 6;
                }
                // nine does not contain e
                if(active.find(*e.begin()) == std::string::npos){
                    return 9;
                }
                // zero else
                return 0;
            case 5:
                if(active.find(*b.begin()) == std::string::npos){
                    return 5;
                }
                // two does not contain c
                if(active.find(*c.begin()) == std::string::npos){
                    return 2;
                }
                // three else
                return 3;
            case 1:
                std::cerr << "Error. One segment should never be active alone\n";
                return -1;
            default:
                return activeLinesPerDigit.at(digitLength);
        }
    }
};

int assignZero(Segment& segment, const std::vector<int>& possibleIndices, const std::vector<std::string>& segments){
    for(auto& zero : possibleIndices){
        int included = 0;
        char includedChar;
        for(auto& c : segment.g){
            if(segments.at(zero).find(c) == std::string::npos){
                continue;
            }
            included++;
            includedChar = c;
        }
        // this is the zero
        if(included == 1){
            segment.g.erase(includedChar);
            segment.f = {includedChar};
            return zero;
        }
    }
    return -1;
}

int assignNine(Segment& segment, const std::vector<int>& possibleIndices, const std::vector<std::string>& segments){
    for(auto& nine : possibleIndices){
        int included = 0;
        for(auto& c : segment.b){
            if(segments.at(nine).find(c) != std::string::npos){
                included++;
            }
        }
        // this is the nine
        if(included == 2){
            for(auto& c : segment.e){
                if(segments.at(nine).find(c) == std::string::npos){
                    segment.d.erase(c);
                    segment.e = {c};
                    return nine;
                }
            }
            return -1;
        }
    }
    return -1;
}

void assignSix(Segment& segment, const std::vector<int>& possibleIndices, const std::vector<std::string>& segments){
    for(auto& i : possibleIndices){
        int included = 0;
        for(auto& c : segment.e){
            if(segments.at(i).find(c) != std::string::npos){
                included++;
            }
        }
        // this is the six
        if(included == 1){
            for(auto& c : segment.b){
                if(segments.at(i).find(c) == std::string::npos){
                    segment.c.erase(c);
                    segment.b = {c};
                    break;
                }
            }
            break;
        }
    }
}

Segment solveSegments(const std::vector<std::string>& segments){
    Segment ret;
    // this works because the segments are sorted by their length
    const std::string& eight = segments.back();
    const std::string& one = segments.front();
    const std::string& seven = segments.at(1);
    const std::string& four = segments.at(2);

    // facts
    for(auto& c : one){
        ret.b.insert(c);
        ret.c.insert(c);
    }
    for(auto& c : four){
        if(!ret.b.contains(c))
        {
            ret.f.insert(c);
            ret.g.insert(c);
        }
    }
    for(auto& c : seven){
        if(!ret.b.contains(c))
        {
            ret.a.insert(c);
        }
    }
    for(auto& c : eight){
        if(ret.g.contains(c) || ret.a.contains(c) || ret.b.contains(c) || ret.c.contains(c) || ret.f.contains(c)){
            continue;
        }
        ret.d.insert(c);
        ret.e.insert(c);
    }

    std::vector<int> indices = {6, 7, 8};
    int zero = assignZero(ret, indices, segments);
    indices.erase(std::remove(indices.begin(), indices.end(), zero), indices.end());

    int nine = assignNine(ret, indices, segments);
    indices.erase(std::remove(indices.begin(), indices.end(), nine), indices.end());

    assignSix(ret, indices, segments);
    return ret;
}

void solvePuzzle(const std::vector<std::string>& puzzle, int stage){
    std::vector<std::string> outputDigits;
    std::vector<std::string> segments;
    std::string delimiter = " | ";
    int count = 0;
    for(auto& line : puzzle){
        outputDigits.clear();
        segments.clear();

        size_t pos = line.find(delimiter);
        if (pos == std::string::npos) {
            continue;
        }
        std::string first = line.substr(0, pos);
        std::string second = line.substr(pos + delimiter.length());
        aoc_utils::splitString(second, ' ', outputDigits);

        if(stage == 1){
            count += std::count_if(outputDigits.begin(), outputDigits.end(), [&](const auto& digit) {
                return activeLinesPerDigit.contains(static_cast<int>(digit.length()));
            });
            continue;
        }

        aoc_utils::splitString(first, ' ', segments);
        std::sort(segments.begin(), segments.end(), [](const std::string& a, const std::string& b) {
            return a.size() < b.size();
        });
        Segment segment = solveSegments(segments);

        int number = 0;
        for(int i = 0; i < outputDigits.size(); i++){
            size_t pow = outputDigits.size() - i - 1;
            number += segment.getDigit(outputDigits.at(i))*static_cast<int>(std::pow(10, pow));
        }
        count += number;

    }
    std::cout << count << "\n";
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