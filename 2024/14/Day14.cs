
using _2024.Utils;
using System.Drawing;

namespace _2024._14;

public struct Robot(ValueTuple<int, int> position, ValueTuple<int, int> velocity)
{
    public ValueTuple<int, int> Position { get; } = position;
    public ValueTuple<int, int> Velocity { get; }= velocity;

    public ValueTuple<int, int> Move(int steps, int ySize, int xSize)
    {
        return (
            ((Position.Item1 + steps * Velocity.Item1) % ySize + ySize) % ySize,
            ((Position.Item2 + steps * Velocity.Item2) % xSize + xSize) % xSize
        );
    }

    public override string ToString()
    {
        return $"Position: {Position}, velcoity: {Velocity})";
    }
}

public class Day14 : Base
{
    public Day14()
    {
        Day = "14";
    }

    private (int, int) ParseInput(string[] input, out List<Robot> robots)
    {
        int ySize = 0, xSize = 0;
        robots = [];
        foreach (string line in input)
        {
            string[] splitted = line.Split(" ");
            string[] position = splitted[0].Replace("p=", "").Split(",");
            string[] velocity = splitted[1].Replace("v=", "").Split(",");
            
            robots.Add(new Robot(
                (int.Parse(position[1]), int.Parse(position[0])),
                (int.Parse(velocity[1]), int.Parse(velocity[0]))
            ));

            ySize = Math.Max(ySize, robots[^1].Position.Item1);
            xSize = Math.Max(xSize, robots[^1].Position.Item2);
        }

        return (ySize+1, xSize+1);
    }

    public override object PartOne(bool example)
    {
        string[] input = ReadInput(example);
        
        (int ySize, int xSize) = ParseInput(input, out List<Robot> robots);
        int[] quadrants = [0, 0, 0, 0];
        foreach ((int, int) newPosition in robots.Select(robot => robot.Move(100, ySize, xSize)))
        {
            int xQuadrant = newPosition.Item1 < (ySize - 1) / 2 ? 0 : 1;
            int yQuadrant = newPosition.Item2 < (xSize - 1) / 2 ? 0 : 2;

            if (newPosition.Item1 == (ySize - 1) / 2 || newPosition.Item2 == (xSize - 1) / 2)
            {
                continue;
            }
            
            quadrants[xQuadrant+yQuadrant]++;
        }
        return quadrants.Aggregate(1, (acc, robotCount) => acc * robotCount);
    }

    public override object PartTwo(bool example)
    {
        string[] input = ReadInput(example);
        (int ySize, int xSize) = ParseInput(input, out List<Robot> robots);
        string bmpPath = Path.Combine(ClassPath, "bmps");
        Directory.CreateDirectory(bmpPath);
        for(int step = 1; step <= ySize*xSize; step++)
        {
            using Bitmap image1 = new (xSize, ySize);
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    image1.SetPixel(x, y, Color.Black);
                }
            }
            foreach ((int, int) newPosition in robots.Select(robot => robot.Move(step, ySize, xSize)))
            {
                image1.SetPixel(newPosition.Item2, newPosition.Item1, Color.White);
            }
            
            image1.Save(Path.Combine(bmpPath, $"{step}.bmp"));
        }
        
        return 0;
    }


    public override void Reset()
    {

    }
}

