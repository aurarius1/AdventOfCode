#include <iostream>
#include <chrono>
#include <regex>
#include <unordered_set>
#include <cmath>
#include "../Utils/utils.h"


int isLineLegal(const std::string& line){
  std::stack<char> symbols;
  for(const auto& symbol : line){
      switch(symbol){
          case '(':
          case '{':
          case '<':
          case '[':
              symbols.push(symbol);
              break;
          case ')':
              if(symbols.top() != '('){
                  return 3;
              }
              symbols.pop();
              break;
          case ']':
              if(symbols.top() != '['){
                  return 57;
              }
              symbols.pop();
              break;
          case '}':
              if(symbols.top() != '{'){
                  return 1197;
              }
              symbols.pop();
              break;
          case '>':
              if(symbols.top() != '<'){
                  return 25137;
              }
              symbols.pop();
              break;
          default:
              std::cout << "invalid char\n";
              exit(-1);
      }
  }
  return 0;
}


size_t autocomplete(const std::string& line){
    std::stack<char> symbols;
    int character = 0;
    for(;character < line.size(); character++){
        const char& symbol = line.at(character);
        switch(symbol){
            case '(':
            case '{':
            case '<':
            case '[':
                symbols.push(symbol);
                break;
            case ')':
            case ']':
            case '}':
            case '>':
                symbols.pop();
                break;
            default:
                std::cout << "invalid char\n";
                exit(-1);
        }
    }
    size_t points = 0;
    while(!symbols.empty()){
        char top = symbols.top();
        symbols.pop();
        points *= 5;
        switch(top){
            case '(':
                points += 1;
                break;
            case '[':
                points += 2;
                break;
            case '{':
                points += 3;
                break;
            case '<':
                points += 4;
                break;
            default:
                std::cerr << "Unexpected character on stack\n";
                exit(-1);
        }
    }

    return points;
}

void stage1(const std::vector<std::string>& puzzle){
    int points = 0;
    for(const auto& line : puzzle){
        points += isLineLegal(line);
    }
    std::cout << points << "\n";
}



void stage2(const std::vector<std::string>& puzzle){
    std::vector<size_t> incompleteLines;
    for(const auto& line : puzzle){
        if(isLineLegal(line) == 0){
            incompleteLines.push_back(autocomplete(line));
        }
    }
    std::sort(incompleteLines.begin(), incompleteLines.end(), std::greater<>());
    std::cout << incompleteLines.at(incompleteLines.size()/ 2) << "\n";
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