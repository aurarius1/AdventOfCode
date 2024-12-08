
#ifndef ADVENTOFCODE_VECTOR_H
#define ADVENTOFCODE_VECTOR_H

#include <iostream>
#include <string>
#include <sstream>
#include <regex>

namespace aoc_utils{
    struct Vector{
        int x = 0;
        int y = 0;


        Vector() = default;

        explicit Vector(const std::string& coordinate);

        Vector(int p_x, int p_y) : x(p_x), y(p_y)
        {

        }

        bool operator!=(const Vector& other) const {
            return x != other.x || y != other.y;
        }

        bool operator==(const Vector& other) const {
            return x == other.x && y == other.y;
        }

        Vector& operator+=(const Vector& other) {
            x += other.x;
            y += other.y;
            return *this;
        }

        Vector operator +(const Vector& other) const{
            return {x + other.x, y + other.y};
        }

        void normalize();

        [[nodiscard]] std::string id() const;

        friend std::ostream& operator<<(std::ostream& os, const Vector& position);
    };


}

#endif //ADVENTOFCODE_VECTOR_H
