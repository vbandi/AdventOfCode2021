/*
--- Day 5: Hydrothermal Venture ---
You come across a field of hydrothermal vents on the ocean floor! These vents constantly produce large, opaque clouds, so it would be best to avoid them if possible.

They tend to form in lines; the submarine helpfully produces a list of nearby lines of vents (your puzzle input) for you to review. For example:

0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2
Each line of vents is given as a line segment in the format x1,y1 -> x2,y2 where x1,y1 are the coordinates of one end the line segment and x2,y2 are the coordinates of the other end. These line segments include the points at both ends. In other words:

An entry like 1,1 -> 1,3 covers points 1,1, 1,2, and 1,3.
An entry like 9,7 -> 7,7 covers points 9,7, 8,7, and 7,7.
For now, only consider horizontal and vertical lines: lines where either x1 = x2 or y1 = y2.

So, the horizontal and vertical lines from the above list would produce the following diagram:

.......1..
..1....1..
..1....1..
.......1..
.112111211
..........
..........
..........
..........
222111....
In this diagram, the top left corner is 0,0 and the bottom right corner is 9,9. Each position is shown as the number of lines which cover that point or . if no line covers that point. The top-left pair of 1s, for example, comes from 2,2 -> 2,1; the very bottom row is formed by the overlapping lines 0,9 -> 5,9 and 0,9 -> 2,9.

To avoid the most dangerous areas, you need to determine the number of points where at least two lines overlap. In the above example, this is anywhere in the diagram with a 2 or larger - a total of 5 points.

Consider only horizontal and vertical lines. At how many points do at least two lines overlap?
*/

var sampleInput =
    @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";

int[,] diagram = ParseInput(sampleInput);
var sampleResult = diagram.Cast<int>().Count(x => x >= 2);
Console.WriteLine($"Sample result: {sampleResult}");

diagram = ParseInput(File.ReadAllText("input.txt"));
var result = diagram.Cast<int>().Count(x => x >= 2);
Console.WriteLine($"Result: {result}");

int[,] ParseInput(string txt)
{
    var reader = new StringReader(txt);
    var lines = new List<(int fromX, int fromY, int toX, int toY)>();

    string? textLine;
    while ((textLine = reader.ReadLine()) != null)
    {
        var parts = textLine.Split(" -> ");
        var from = parts[0].Split(",");
        var to = parts[1].Split(",");
        
        var x1 = int.Parse(from[0]);
        var y1 = int.Parse(from[1]);
        var x2 = int.Parse(to[0]);
        var y2 = int.Parse(to[1]);
        
        // only consider horizontal and vertical lines
        if (x1 == x2 || y1 == y2)
        {
            lines.Add((Math.Min(x1, x2), Math.Min(y1, y2), Math.Max(x1, x2), Math.Max(y1, y2)));
        }
    }

    var maxX = lines.Select(x=> x.toX).Concat(lines.Select(x => x.fromX)).Max();
    var maxY = lines.Select(x => x.toY).Concat(lines.Select(x => x.fromY)).Max();
    var result = new int[maxX + 1, maxY + 1];

    foreach (var line in lines)
    {
        for (var x = line.fromX; x <= line.toX; x++)
        {
            for (var y = line.fromY; y <= line.toY; y++)
            {
                result[x, y]++;
            }
        }
    }

    return result;
}
    
/*
--- Part Two ---
Unfortunately, considering only horizontal and vertical lines doesn't give you the full picture; you need to also consider diagonal lines.

Because of the limits of the hydrothermal vent mapping system, the lines in your list will only ever be horizontal, vertical, or a diagonal line at exactly 45 degrees. In other words:

An entry like 1,1 -> 3,3 covers points 1,1, 2,2, and 3,3.
An entry like 9,7 -> 7,9 covers points 9,7, 8,8, and 7,9.
Considering all lines from the above example would now produce the following diagram:

1.1....11.
.111...2..
..2.1.111.
...1.2.2..
.112313211
...1.2....
..1...1...
.1.....1..
1.......1.
222111....
You still need to determine the number of points where at least two lines overlap. In the above example, this is still anywhere in the diagram with a 2 or larger - now a total of 12 points.

Consider all of the lines. At how many points do at least two lines overlap?
*/

Console.WriteLine("--- Part 2 ---");

diagram = ParseInputPart2(sampleInput);
sampleResult = diagram.Cast<int>().Count(x => x >= 2);
Console.WriteLine($"Sample result: {sampleResult}");

diagram = ParseInputPart2(File.ReadAllText("input.txt"));
result = diagram.Cast<int>().Count(x => x >= 2);
Console.WriteLine($"Result: {result}");

int[,] ParseInputPart2(string txt)
{
    var reader = new StringReader(txt);
    var lines = new List<(int fromX, int fromY, int toX, int toY)>();
    var diagonalLines = new List<(int fromX, int fromY, int toX, int toY)>();

    var maxX = 0;
    var maxY = 0;
    
    string? textLine;
    while ((textLine = reader.ReadLine()) != null)
    {
        var parts = textLine.Split(" -> ");
        var from = parts[0].Split(",");
        var to = parts[1].Split(",");
        
        var x1 = int.Parse(from[0]);
        var y1 = int.Parse(from[1]);
        var x2 = int.Parse(to[0]);
        var y2 = int.Parse(to[1]);
        
        // only consider horizontal and vertical lines
        if (x1 == x2 || y1 == y2)
        {
            lines.Add((Math.Min(x1, x2), Math.Min(y1, y2), Math.Max(x1, x2), Math.Max(y1, y2)));
        }
        
        // only consider diagonal lines
        if (Math.Abs(x1 - x2) == Math.Abs(y1 - y2))
        {
            diagonalLines.Add((x1, y1, x2, y2));
        }
        
        maxX = Math.Max(maxX, Math.Max(x1, x2));
        maxY = Math.Max(maxY, Math.Max(y1, y2));
    }
    
    var result = new int[maxX + 1, maxY + 1];

    foreach (var line in lines)
    {
        for (var x = line.fromX; x <= line.toX; x++)
        {
            for (var y = line.fromY; y <= line.toY; y++)
            {
                result[x, y]++;
            }
        }
    }
    
    foreach (var line in diagonalLines)
    {
        var xDirection = line.fromX < line.toX ? 1 : -1;
        var yDirection = line.fromY < line.toY ? 1 : -1;
        var steps = Math.Abs(line.fromX - line.toX);

        for (var i = 0; i <= steps; i++)
        {
            var x = line.fromX + i * xDirection;
            var y = line.fromY + i * yDirection;
            result[x, y]++;
        }
    }

    return result;
}
