cmake_minimum_required(VERSION 3.10)
project(AdventOfCode)

set(CMAKE_CXX_STANDARD 20)
# Specify the range of days
set(DAYS 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25)

# Include the Utils folder
set(UTILS_DIR ${CMAKE_SOURCE_DIR}/Utils/C++)
include_directories(${UTILS_DIR})

# Add the utils.cpp file to a shared library
add_library(Utils ${UTILS_DIR}/utils.cpp)

# Loop through all days and add a target for each
foreach(DAY IN LISTS DAYS)
    # Set the source directory for the current day
    set(SOURCE_DIR ${CMAKE_SOURCE_DIR}/${DAY}/C++)

    # Check if the main.cpp file exists for the day
    if(EXISTS ${SOURCE_DIR}/main.cpp)
        # Add an executable for the current day
        add_executable(${DAY} ${SOURCE_DIR}/main.cpp)

        # Link the Utils library to the current day's target
        target_link_libraries(${DAY} Utils)

        # Set the output directory to the 2021 folder
        set_target_properties(${DAY} PROPERTIES
                RUNTIME_OUTPUT_DIRECTORY ${SOURCE_DIR}
        )

        # Set the working directory for the target
        set_property(TARGET ${DAY} PROPERTY VS_DEBUGGER_WORKING_DIRECTORY ${SOURCE_DIR})
    else()
        message(WARNING "Skipping day ${DAY} because ${SOURCE_DIR}/main.cpp does not exist.")
    endif()
endforeach()
