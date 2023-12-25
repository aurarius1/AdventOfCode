#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <time.h>
#include <pthread.h>

#define NUM_NUMBERS 9
static const char* numbers[9] = {
        "one","two","three","four",
        "five","six", "seven","eight","nine"
};
int searchNumberInLine(char* line, int fromBack, int stage)
{
    while(*line != '\n')
    {
        int num = -1;
        if(*line >= '1' && *line <= '9')
        {
            num = *line-'0';
        }
        if(stage == 2)
        {
            for(int i = 0; i < NUM_NUMBERS; i++)
            {
                if(strncmp(line, numbers[i], strlen(numbers[i])) == 0)
                {
                    num = i+1;
                }
            }
        }
        if(num != -1)
        {
            if(fromBack)
            {
                int found = searchNumberInLine(++line, fromBack, stage);
                if(found != -1)
                {
                    return found;
                }
            }
            return num;
        }

        line++;
    }
    return -1;
}


int calculateCalibrationValue(char* line, int stage)
{
    int first = searchNumberInLine(line, 0, stage);
    int last = searchNumberInLine(line, 1, stage);

    return first*10+last;
}


int day1(int stage)
{
    FILE *file;
    file = fopen("config", "r");
    if(file == NULL)
        return 0;
    char* line;
    size_t len;
    int value = 0;
    while(getline(&line, &len, file) != -1)
    {
        value += calculateCalibrationValue(line, stage);
        free(line);
        line = NULL;
    }

    fclose(file);
    return value;
}




int main(int argc, char* argv[]) {

    int stage = 1;
    if(argc > 1)
    {
        stage = *argv[1] - '0';
    }
    printf("Stage: %d\n", stage);

    double time1 = (double) clock();
    time1 = time1 / CLOCKS_PER_SEC;
    printf("Result Stage %d: %d\n", stage, day1(stage));
    double time_diff = (((double) clock()) / CLOCKS_PER_SEC) - time1;
    printf("The elapsed time is %lf seconds\n", time_diff);
    return 0;
}
