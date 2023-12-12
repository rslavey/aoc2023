var files01 = new List<(string Title, string Path)>
{
    ("Test Inputs 01", ".\\inputs\\inputTest01_01.txt"),
    ("Puzzle Inputs", ".\\inputs\\input.txt")
};

for (var part = 1; part <= 2; part++)
{
    foreach (var file in files01)
    {
        Universe universe = new(File.ReadAllLines(file.Path).SelectMany((y, i) => y.ToCharArray().Select((x, ii) => new Loc { P = x, X = ii, Y = i })));
        Console.WriteLine($"Part {part.ToString().PadLeft(2, '0')}: Distance sums: {universe.GetDistance(part == 1 ? 2 : 1000000)?.SelectMany((x, i) => x[i..]).Sum()}");
    }
}

internal class Universe
{
    public List<Galaxy> Galaxies { get; set; }
    public List<Loc> Locations { get; set; }
    public List<int> DeadColumns { get; set; } = new();
    public List<int> DeadRows { get; set; } = new();

    public Universe(IEnumerable<Loc> input)
    {
        Locations = new List<Loc>(input);
        ExpandUniverse();
        Galaxies = Locations.Where(x => x.P == '#').Select(x => new Galaxy(x)).ToList();
    }

    internal long[][]? GetDistance(long age)
    {
        return Galaxies.Select(x =>
            Galaxies.Select(xx =>
                {
                    var deadColCount = DeadColumns.Count(dc => x.Location.X != xx.Location.X && Enumerable.Range(Math.Min(x.Location.X, xx.Location.X), Math.Abs(x.Location.X - xx.Location.X)).Contains(dc));
                    var deadRowCount = DeadRows.Count(dr => x.Location.Y != xx.Location.Y && Enumerable.Range(Math.Min(x.Location.Y, xx.Location.Y), Math.Abs(x.Location.Y - xx.Location.Y)).Contains(dr));

                    return (long)(
                        Math.Abs(x.Location.X - xx.Location.X) +
                        (
                            deadColCount * age - deadColCount
                        )
                    ) +
                    (long)(
                        Math.Abs(x.Location.Y - xx.Location.Y) +
                        (
                            deadRowCount * age - deadRowCount
                        )
                    );
                }
            )
            .ToArray()
        )
        .ToArray();
    }

    private void ExpandUniverse()
    {
        DeadRows.AddRange(
            Locations.GroupBy(x => x.Y).Where(x => x.All(xx => xx.P == '.')).Select(x => x.Key)
        );
        DeadColumns.AddRange(
            Locations.GroupBy(x => x.X).Where(x => x.All(xx => xx.P == '.')).Select(x => x.Key)
        );
    }
}

internal class Loc
{
    public int X { get; set; }
    public int Y { get; set; }
    public char P { get; set; }
}

internal class Galaxy
{
    public Loc Location { get; set; }

    public Galaxy(Loc loc)
    {
        Location = loc;
    }
}
