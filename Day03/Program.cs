Console.WriteLine("Part 01");
var files01 = new List<(string Title, string Path)> { ("Test Inputs", ".\\inputs\\inputTest01.txt"), ("Puzzle Inputs", ".\\inputs\\input.txt") };
foreach (var file in files01)
{
    Console.Write(file.Title + ":\t");

    var grid = File.ReadAllLines(file.Path).Select(line => line.ToCharArray()).ToArray();

    var count = 0;
    foreach (var line in grid.Select((val, i) => new { val, i }))
    {
        for (var ci = 0; ci < line.val.Length; ci++)
        {
            if (char.IsDigit(line.val[ci]))
            {
                var numberText = line.val[ci..].TakeWhile(linechar => char.IsDigit(linechar)).ToArray();

                count += grid
                        .Where((gridline, linei) =>
                            linei >= line.i - 1 && linei <= line.i + 1
                        )
                        .SelectMany(gridchars =>
                            gridchars.Where((gridchar, i) =>
                                i >= ci - 1 && i <= ci + 1 + (numberText.Length - 1)
                            )
                        )
                        .Any(surchar =>
                            !char.IsDigit(surchar) && surchar != '.')
                        ? int.Parse(numberText) : 0;

                ci += numberText.Length - 1;
            }

        }
    }
    Console.WriteLine($"Total: {count}\n");
}

Console.WriteLine("Part 01");
var files02 = new List<(string Title, string Path)> { ("Test Inputs", ".\\inputs\\inputTest01.txt"), ("Puzzle Inputs", ".\\inputs\\input.txt") };
foreach (var file in files02)
{
    Console.Write(file.Title + ":\t");

    var grid = File.ReadAllLines(file.Path).Select(line => line.ToCharArray()).ToArray();
    Dictionary<(int x, int y), int> nums = new();
    Dictionary<(int x, int y), List<KeyValuePair<(int x, int y), int>>> gears = new();
    var count = 0;
    foreach (var line in grid.Select((val, i) => new { val, i }))
    {
        for (var ci = 0; ci < line.val.Length; ci++)
        {
            if (char.IsDigit(line.val[ci]))
            {
                var numberText = line.val[ci..].TakeWhile(linechar => char.IsDigit(linechar)).ToArray();
                nums.Add((ci, line.i), int.Parse(numberText));
                ci += numberText.Length - 1;
            }
            else if (line.val[ci] == '*')
            {
                gears.Add((ci, line.i), new());
            }
        }
    }
    foreach (var gear in gears)
    {
        foreach (var num in nums)
        {
            if (gear.Key.x >= num.Key.x - 1 && gear.Key.x <= num.Key.x + (num.Value.ToString().Length) && gear.Key.y >= num.Key.y - 1 && gear.Key.y <= num.Key.y + 1)
            {
                if (!gear.Value.Contains(num))
                {
                    gears[gear.Key].Add(num);
                }
            }
        }
    }
    count = gears.Where(x => x.Value.Count == 2).Select(x => x.Value.Select(xx => xx.Value).Aggregate((a, b) => a * b)).Aggregate((a,b) => a + b);
    Console.WriteLine($"Total: {count}\n");
}
