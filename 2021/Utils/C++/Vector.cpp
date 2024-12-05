
#include "Vector.h"

namespace aoc_utils {
    Vector::Vector(const std::string& coordinate){
        std::stringstream coordinateStream(std::regex_replace(coordinate, std::regex(","), " "));
        coordinateStream >> x >> y;
    }

    void Vector::normalize() {
        if(this->x != 0){
            this->x = this->x / (std::abs(this->x));
        }

        if(this->y != 0){
            this->y = this->y / (std::abs(this->y));
        }
    }

    std::ostream &operator<<(std::ostream &os, const Vector &position) {
        os << "(" << position.x << "," << position.y << ")";
        return os;
    }
}