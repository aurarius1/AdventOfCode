#include <iostream>
#include <chrono>
#include "../Utils/utils.h"

struct BitCounter {
    int zero = 0;
    int one = 0;
};

BitCounter countBits(const std::vector<std::string>& numbers, int position, const std::string& pattern = ""){
    BitCounter bit;
    for(auto& number : numbers){
        if(position < number.length() && position >= 0){

            if(!number.starts_with(pattern) && !pattern.empty()){
                continue;
            }

            if(number.at(position) == '1'){
                bit.one++;
            } else {
                bit.zero++;
            }
        }
    }
    return bit;
}

void stage1(const std::vector<std::string>& puzzle){
    int gamma = 0, epsilon = 0;
    int digits = puzzle.at(0).length();
    for(int i = 0; i < digits; i++){
        BitCounter bit = countBits(puzzle, i);
        int pow = 1 << (digits - 1 - i); // 2^X
        if(bit.one >= bit.zero){
            gamma += pow;
            epsilon += 0;
        } else {
            gamma += 0;
            epsilon += pow;
        }
    }
    std::cout << gamma * epsilon << "\n";
}

void stage2(const std::vector<std::string>& puzzle) {
    std::vector<std::string> oxygen_numbers(puzzle), co2_numbers(puzzle);
    int digits = puzzle.at(0).length();

    // this is the same for oxygen and co2;
    std::string oxygen_pattern, co2_pattern;
    for (int i = 0; i < digits; i++) {

        if (oxygen_numbers.size() > 1) {
            BitCounter first_bit = countBits(oxygen_numbers, i);
            if (first_bit.one >= first_bit.zero) {
                oxygen_pattern += "1";
            } else {
                oxygen_pattern += "0";
            }
            std::erase_if(oxygen_numbers, [&oxygen_pattern](const std::string &s) {
                return !s.starts_with(oxygen_pattern);
            });
        }

        if (co2_numbers.size() > 1) {
            BitCounter first_bit = countBits(co2_numbers, i);
            if (first_bit.one < first_bit.zero) {
                co2_pattern += "1";
            } else {
                co2_pattern += "0";
            }
            std::erase_if(co2_numbers, [&co2_pattern](const std::string &s) {
                return !s.starts_with(co2_pattern);
            });
        }

    }
    int oxygen = std::stoi(oxygen_numbers.at(0), nullptr, 2);
    int co2 = std::stoi(co2_numbers.at(0), nullptr, 2);
    std::cout << oxygen * co2 << "\n";
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