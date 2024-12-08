#include <iostream>
#include <chrono>
#include <fstream>
#include <unordered_set>
#include <unordered_map>
#include <queue>
#include <numeric>
#include "../Utils/utils.h"
#include "../Utils/Vector.h"
typedef std::vector<std::string> puzzle_t;

struct Packet {
    Packet() = default;

    Packet(int version, int typeId) : version_(version), typeId_(typeId) {}

    int accumulateVersions(){
        int version = this->version_;
        for(auto& packet : this->subPackets_){
            version += packet.accumulateVersions();
        }
        return version;
    }

    size_t calculateTransmission(){
        if(typeId_ == 4){
            return value_;
        }

        // this is done, to clean up the switch case, it adds space complexity, but is easier to read
        // and we have space
        std::vector<size_t> values;
        values.reserve(this->subPackets_.size());
        for (auto& subPacket : this->subPackets_) {
            values.push_back(subPacket.calculateTransmission());
        }

        switch(this->typeId_){
            case 0:
                return std::accumulate(values.begin(), values.end(), size_t(0), std::plus<>());
            case 1:
                return std::accumulate(values.begin(), values.end(), size_t(1), std::multiplies<>());
            case 2:
                return *std::min_element(values.begin(), values.end());
            case 3:
                return *std::max_element(values.begin(), values.end());
            case 5:
                return values[0] > values[1];
            case 6:
                return values[0] < values[1];
            case 7:
                return values[0] == values[1];
            default:
                return 0;
        }
    }

    unsigned int version_ : 3 = 0;
    unsigned int typeId_ : 3 = 0;
    size_t packetBitLength_ = 0;
    size_t value_ = 0;
    std::vector<Packet> subPackets_;
};

Packet parse(const std::vector<unsigned char>& bits, size_t start){
    // first 3 bits are version number, second three bits are typeId
    int version = (bits[start] << 2) | (bits[start+1] << 1) | bits[start+2];
    int typeId = (bits[start+3] << 2) | (bits[start+4] << 1) | bits[start+5];
    Packet curr(version, typeId);
    if(typeId == 4){
        size_t packetCnt = 0;
        for (size_t i = start + 6; i < bits.size(); i += 5) {
            packetCnt++;
            size_t groupStart = i + 1;
            for (size_t bit = 0; bit < 4; ++bit) {
                curr.value_ = (curr.value_ << 1) | bits[groupStart + bit];
            }
            if (bits[i] == 0) {
                break; // Last group
            }
        }
        curr.packetBitLength_ = 6 + packetCnt*5;
        return curr;
    }

    size_t length = 0;
    size_t bitsLength = bits[start + 6] == 0 ? 15 : 11;
    size_t newStart = start + 7;

    for (size_t i = 0; i < bitsLength; ++i) {
        length = (length << 1) | bits[newStart++];
    }

    if(bitsLength == 11){
        curr.subPackets_.reserve(length);
        for(int i = 0; i < length; i++){
            curr.subPackets_.emplace_back(parse(bits, newStart));
            newStart += curr.subPackets_.back().packetBitLength_;
        }
    } else {
        size_t end = newStart + length;
        while(newStart < end){
            curr.subPackets_.emplace_back(parse(bits, newStart));
            newStart += curr.subPackets_.back().packetBitLength_;
        }
    }
    curr.packetBitLength_ = newStart - start;
    return curr;
}

void solvePuzzle(const puzzle_t& puzzle, int stage){
    std::vector<unsigned char> bits(puzzle.at(0).length()*4);
    for(int currDigit = 0; currDigit < puzzle.at(0).length(); currDigit++){
        size_t x = std::stoi(puzzle.at(0).substr(currDigit, 1), nullptr, 16);
        for(int bit = 0; x > 0; bit++) {
            bits[(currDigit*4) + 4-bit-1] = x%2;
            x /= 2;
        }
    }
    Packet packet = parse(bits, 0);
    if(stage == 1){
        std::cout << packet.accumulateVersions() << "\n";
    } else {
        std::cout << packet.calculateTransmission() << "\n";
    }
}

void stage1(const puzzle_t& puzzle) {
    solvePuzzle(puzzle, 1);
}
void stage2(const puzzle_t& puzzle) {
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