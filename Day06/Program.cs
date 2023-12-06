Console.WriteLine("Part 01");

var files01 = new List<(string Title, string Path)>
{
    ("Test Inputs", ".\\inputs\\inputTest01.txt"),
    ("Puzzle Inputs", ".\\inputs\\input.txt")
};

foreach (var file in files01) { 
    Dictionary <int,int> races = new();
    var lines = File.ReadAllLines(file.Path);
    var times = lines[0][(lines[0].IndexOf(":") + 1)..].Split(' ',StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToArray();
    var records = lines[1][(lines[1].IndexOf(":") + 1)..].Split(' ',StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToArray();

    var prodWin = 1;
    for (var i = 0; i < times.Count(); i++) 
    { 
        var waysToWin = 0;
        for (var t = 0; t <= times[i]; t++)
        {
            var distance = (times[i] - t) * t;
            waysToWin += distance > records[i] ? 1 : 0;
        }
        prodWin *= waysToWin;
    }

    Console.WriteLine($"{file.Title}: {prodWin}");
}

Console.WriteLine("\nPart 02");

var files02 = new List<(string Title, string Path)>
{
    ("Test Inputs", ".\\inputs\\inputTest02.txt"),
    ("Puzzle Inputs", ".\\inputs\\input02.txt")
};

foreach (var file in files02)
{
    var lines = File.ReadAllLines(file.Path);
    var times = lines[0][(lines[0].IndexOf(":") + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim())).ToArray();
    var records = lines[1][(lines[1].IndexOf(":") + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim())).ToArray();

    var prodWin = 1;
    for (var i = 0; i < times.Count(); i++)
    {
        var waysToWin = 0;
        for (var t = (long)Math.Floor(times[i]/2.0); t >= 0; t--)
        {
            if ((times[i] - t) * t < records[i])
            {
                break;
            }
            waysToWin++;
        }
        prodWin *= waysToWin * 2 + (times[i] % 2 == 0 ? -1 : 0);
    }

    Console.WriteLine($"{file.Title}: {prodWin}");
}
