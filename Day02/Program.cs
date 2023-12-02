Console.WriteLine("Part 01");
Dictionary<string, int?> bagContents = new()
{
    {"red", 12 }
    ,{"green", 13 }
    ,{"blue", 14 }
};

foreach (var file in new List<(string Title, string Path)> { ("Test Inputs", ".\\inputs\\inputTest01.txt"), ("Puzzle Inputs", ".\\inputs\\input.txt") })
{
    Console.WriteLine(file.Title);
    var score = 0;
    foreach (var game in File.ReadLines(file.Path))
    {
        var gameNum = int.Parse(game.Substring(0, game.IndexOf(':')).Replace("Game", "", StringComparison.InvariantCultureIgnoreCase));

        score += game[(game.IndexOf(':') + 1)..]
                .Split(';')
                .All(draw =>
                    draw
                    .Trim()
                    .Split(',')
                    .Select(cube =>
                        cube
                        .Trim()
                        .Split(' ')
                    )
                    .Select(cubeval =>
                        new KeyValuePair<string, int>(cubeval[1], int.Parse(cubeval[0])))
                    .All(cubeKV =>
                        cubeKV.Value <= bagContents[cubeKV.Key.Trim()]
                    )
                ) ? gameNum : 0;
    }
    Console.WriteLine($"Score: {score}\n");
}

Console.WriteLine("\nPart 02");
foreach (var file in new List<(string Title, string Path)> { ("Test Inputs", ".\\inputs\\inputTest01.txt"), ("Puzzle Inputs", ".\\inputs\\input.txt") })
{
    Console.WriteLine(file.Title);
    var score = 0;
    foreach (var game in File.ReadLines(file.Path))
    {
        Dictionary<string, int> minRequired = new()
        {
            {"red", 0 }
            ,{"green", 0 }
            ,{"blue", 0 }
        };

        var draws = game[(game.IndexOf(':') + 1)..]
                .Split(';')
                .Select(draw =>
                    draw
                    .Trim()
                    .Split(',')
                    .Select(cube =>
                        cube
                        .Trim()
                        .Split(' ')
                    )
                    .Select(cubeval =>
                        new KeyValuePair<string, int>(cubeval[1], int.Parse(cubeval[0])))
                    );

        foreach (var cube in draws.SelectMany(x => x))
        {
            minRequired[cube.Key] = Math.Max(minRequired[cube.Key], cube.Value);
        }

        score += minRequired.Aggregate(1, (x, y) => x * y.Value);
    }
    Console.WriteLine($"Score: {score}\n");
}
