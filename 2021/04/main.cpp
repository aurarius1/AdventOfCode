#include <iostream>
#include <chrono>
#include "../Utils/utils.h"

struct Number {
    int value = -1;
    bool marked = false;
};

struct BingoBoard {
    int dimensions = 0;
    bool won = false;
    std::vector<Number> numbers;

    int countUnmarked(){
        int cnt = 0;
        for(int row = 0; row < dimensions; row++){
            for(int col = 0; col < dimensions; col++){
                if(!numbers.at(row*dimensions+col).marked){
                    cnt += numbers.at(row*dimensions+col).value;
                }
            }
        }
        return cnt;
    }

    void markNumber(int number){
        int idx = 0;
        for(; idx < dimensions*dimensions; idx++){
            if(numbers.at(idx).value == number){
                numbers.at(idx).marked = true;
                break;
            }
        }
        if(idx == dimensions*dimensions){
            return;
        }

        int row = static_cast<int>(idx/dimensions);
        int col = idx % dimensions;

        // check if this row is marked completely
        int check = 0;
        bool row_marked = true, col_marked = true;
        for(; check < dimensions; check++) {
            row_marked &= numbers.at(row * dimensions + check).marked;
            col_marked &= numbers.at(check * dimensions + col).marked;
        }
        won = row_marked || col_marked;
    }

    friend std::ostream& operator<<(std::ostream& os, const BingoBoard& board);
};

std::ostream &operator<<(std::ostream &os, const BingoBoard &board) {
    for(int row = 0; row < board.dimensions; row++){
        for(int col = 0; col < board.dimensions; col++){
            if(!board.numbers.at(row*board.dimensions + col).marked){
                os << std::setw(2) <<  board.numbers.at(row*board.dimensions + col).value << " ";
            } else {
                os << std::setw(2) <<  board.numbers.at(row*board.dimensions + col).marked << " ";
            }
        }
        os << "\n";
    }
    return os;
}


void stage(const std::vector<std::string>& puzzle, int stage = 1){
    std::vector<BingoBoard> boards;
    std::vector<int> draws;

    aoc_utils::splitString(puzzle.at(0), ',', draws);
    int num_boards = 0, board_dim = 0; //board dim could be initialized to 5 and be done with it, assignment states 5x5

    for(size_t i = 1; i < puzzle.size(); i++){
        const std::string& line = puzzle.at(i);
        if(line.empty()){
            num_boards++;
            continue;
        }
        board_dim++;
    }
    board_dim /= num_boards;

    boards.resize(num_boards);
    for(size_t i = 1, curr_board = -1; i < puzzle.size(); i++){
        const std::string& line = puzzle.at(i);
        if(line.empty()){
            curr_board++;
            boards.at(curr_board).dimensions = board_dim;
            boards.at(curr_board).numbers.resize(board_dim*board_dim);
            continue;
        }
        size_t curr_row = (i-1-(curr_board+1))%board_dim;
        std::stringstream lineStream(line);
        for(size_t j = board_dim*curr_row; j < board_dim*curr_row+5; j++){
            lineStream >> boards.at(curr_board).numbers.at(j).value;
        }
    }
    int score = 0;
    for(auto& draw : draws){
        for(auto& board : boards){
            if(stage == 2 && board.won){
                continue;
            }
            board.markNumber(draw);
            if(board.won){
                score = board.countUnmarked() * draw;
                if(stage == 1){
                    std::cout << score << "\n";
                    return;
                }
            }
        }
    }
    std::cout << score << "\n";
}

int main(){
    std::vector<std::string> puzzle;
    std::string input = "input";
    aoc_utils::load_puzzle(input, puzzle);

    auto start = std::chrono::high_resolution_clock::now();
    stage(puzzle, 1);
    auto end = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> elapsed = end - start;
    std::cout << "Stage 1 took " << elapsed.count() << " seconds\n";

    start = std::chrono::high_resolution_clock::now();
    stage(puzzle, 2);
    end = std::chrono::high_resolution_clock::now();
    elapsed = end - start;
    std::cout << "Stage 2 took " << elapsed.count() << " seconds\n";
}