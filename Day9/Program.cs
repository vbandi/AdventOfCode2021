/*
--- Day 9: Smoke Basin ---
These caves seem to be lava tubes. Parts are even still volcanically active; small hydrothermal vents release smoke into the caves that slowly settles like rain.

If you can model how the smoke flows through the caves, you might be able to avoid it and be that much safer. The submarine generates a heightmap of the floor of the nearby caves for you (your puzzle input).

Smoke flows to the lowest point of the area it's in. For example, consider the following heightmap:

2199943210
3987894921
9856789892
8767896789
9899965678

Each number corresponds to the height of a particular location, where 9 is the highest and 0 is the lowest a location can be.

Your first goal is to find the low points - the locations that are lower than any of its adjacent locations. Most locations have four adjacent locations (up, down, left, and right); locations on the edge or corner of the map have three or two adjacent locations, respectively. (Diagonal locations do not count as adjacent.)

In the above example, there are four low points, all highlighted: two are in the first row (a 1 and a 0), one is in the third row (a 5), and one is in the bottom row (also a 5). All other locations on the heightmap have some lower adjacent location, and so are not low points.

The risk level of a low point is 1 plus its height. In the above example, the risk levels of the low points are 2, 1, 6, and 6. The sum of the risk levels of all low points in the heightmap is therefore 15.

Find all of the low points on your heightmap. What is the sum of the risk levels of all low points on your heightmap?
*/

var sampleInput = @"2199943210
3987894921
9856789892
8767896789
9899965678";

Part1(sampleInput);
Part1(File.ReadAllText("input.txt"));

void Part1(string txt)
{
    var map = GetMap(txt);

    // print map
    // void PrintMap()
    // {
    //     foreach (var t in map)
    //     {
    //         Console.WriteLine(string.Join("", t));
    //     }
    // }

    // PrintMap();

    // find low points
    var lowPoints = GetLowPoints(map);

    // print low points
    Console.WriteLine($"Low points: {string.Join(", ", lowPoints.Select(x => $"({x.x}, {x.y})"))}");


    // find risk level (1 plus height)
    var riskLevel = lowPoints.Sum(x => map[x.x][x.y] + 1);

    // print risk level
    Console.WriteLine($"Risk level: {riskLevel}");
}

List<(int x, int y)> GetLowPoints(int[][] ints)
{
    var lowPoints = new List<(int x, int y)>();

    for (int i = 1; i < ints.Length - 1; i++)
        for (int j = 1; j < ints[i].Length - 1; j++)
            if (GetNeighbors(i, j).All(x => ints[x.x][x.y] > ints[i][j]))
                lowPoints.Add((i, j));

    return lowPoints;
}

IEnumerable<(int x, int y)> GetNeighbors(int x, int y)
{
    yield return (x - 1, y);
    yield return (x + 1, y);
    yield return (x, y - 1);
    yield return (x, y + 1);
}

int[][] GetMap(string s)
{
    var ints = s.Split("\r\n").Select(x => x.ToCharArray().Select(y => y - '0').ToArray()).ToArray();

    // add 9s to the edges
    for (int i = 0; i < ints.Length; i++)
    {
        ints[i] = new[] { 9 }.Concat(ints[i]).Concat(new[] { 9 }).ToArray();
    }

    // add 9s to top and bottom
    var top = new[] { Enumerable.Repeat(9, ints[0].Length).ToArray() };
    var bottom = new[] { Enumerable.Repeat(9, ints[0].Length).ToArray() };
    ints = top.Concat(ints).Concat(bottom).ToArray();
    return ints;
}

/*
--- Part Two ---
Next, you need to find the largest basins so you know what areas are most important to avoid.

A basin is all locations that eventually flow downward to a single low point. Therefore, every low point has a basin, although some basins are very small. Locations of height 9 do not count as being in any basin, and all other locations will always be part of exactly one basin.

The size of a basin is the number of locations within the basin, including the low point. The example above has four basins.

The top-left basin, size 3:

2199943210
3987894921
9856789892
8767896789
9899965678
The top-right basin, size 9:

2199943210
3987894921
9856789892
8767896789
9899965678
The middle basin, size 14:

2199943210
3987894921
9856789892
8767896789
9899965678
The bottom-right basin, size 9:

2199943210
3987894921
9856789892
8767896789
9899965678
Find the three largest basins and multiply their sizes together. In the above example, this is 9 * 14 * 9 = 1134.
*/

Part2(sampleInput);
Part2(File.ReadAllText("input.txt"));

void Part2(string txt)
{
    var map = GetMap(txt);
    var lowPoints = GetLowPoints(map);
    
    var basinSizes = new Dictionary<(int x, int y), int>();
    
    // find basins for each low point
    foreach (var lowPoint in lowPoints)
    {
        basinSizes[lowPoint] = GetBasinSize(map, lowPoint);
    }
    
    // print basins
    // Console.WriteLine($"Basins: {string.Join(", ", basins.Select(x => $"({x.Key.x}, {x.Key.y}): {x.Value}"))}");
    
    // find three largest basins
    var largestBasins = basinSizes.OrderByDescending(x => x.Value).Take(3).ToList();
    
    // print largest basins
    // Console.WriteLine($"Largest basins: {string.Join(", ", largestBasins.Select(x => $"({x.Key.x}, {x.Key.y}): {x.Value}"))}");
    
    // multiply largest basins
    var result = largestBasins.Aggregate(1, (x, y) => x * y.Value);
    
    // print result
    Console.WriteLine($"Result: {result}");
}

// find basin for a low point
int GetBasinSize(int[][] map, (int x, int y) lowPoint)
{
    var pointsInBasin = new HashSet<(int x, int y)>();
    pointsInBasin.Add(lowPoint);

    int count;

    do
    {
        count = pointsInBasin.Count;
        var newPointsInBasin = pointsInBasin.SelectMany(x => GetNeighbors(x.x, x.y).Where(y => map[y.x][y.y] < 9)).ToHashSet();
        pointsInBasin.UnionWith(newPointsInBasin);
    } while (count < pointsInBasin.Count);
    
    return pointsInBasin.Count;
}