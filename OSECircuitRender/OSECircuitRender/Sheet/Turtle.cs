using OSECircuitRender.Definitions;
using System;
using System.Threading.Tasks;

namespace OSECircuitRender.Sheet;

public class Turtle
{
    public Turtle(Coordinate start, Coordinate end, int[,] map)
    {
        Start = start;
        End = end;
        Map = map;
    }

    public static int MaxTries { get; set; } = 100;
    public Coordinate End { get; }
    public int[,] Map { get; }
    public Coordinate Start { get; }
    public int Tries { get; set; }

    public async Task Crawl()
    {
        var difference = Start.Substract(End);
        var currentPosition = new Coordinate(Start);

        await Move(currentPosition, difference);
    }

    private int GetMap(Coordinate direction)
    {
        var x = Convert.ToInt32(Math.Round(direction.X));
        var y = Convert.ToInt32(Math.Round(direction.Y));
        return Map[x, y];
    }

    private async Task Move(Coordinate currentPosition, Coordinate difference)
    {
        Tries++;
        if (Tries > MaxTries) return;

        var nextX = ToInt(currentPosition.X);
        var nextY = ToInt(currentPosition.Y);
        Map[nextX, nextY] = 999;

        if (difference.X < 0)
            if (Map[nextX + 1, nextY] == 0)
            {
            }
    }

    private int ToInt(float currentPosition)
    {
        return Convert.ToInt32(Math.Round(currentPosition));
    }
}