var files01 = new List<(string Title, string Path)>
{
    ("Test Inputs 01", ".\\inputs\\inputTest01_01.txt"),
    ("Test Inputs 02", ".\\inputs\\inputTest01_02.txt"),
    ("Puzzle Inputs", ".\\inputs\\input.txt")
};

foreach (var file in files01)
{
    Grid grid = new(File.ReadAllLines(file.Path).SelectMany((x, i) => x.ToCharArray().Select((xx, ii) => new Pipe { P = xx, X = ii, Y = i })).ToList());
    Console.WriteLine($"File {file.Title}: Route Length: {Math.Ceiling(grid.Route.Count / 2.0)}");
}

var files02 = new List<(string Title, string Path)>
{
    ("Test Inputs 01", ".\\inputs\\inputTest02_01.txt"),
    ("Test Inputs 02", ".\\inputs\\inputTest02_02.txt"),
    ("Puzzle Inputs", ".\\inputs\\input.txt")
};

foreach (var file in files02)
{
    Grid grid = new(File.ReadAllLines(file.Path).SelectMany((x, i) => x.ToCharArray().Select((xx, ii) => new Pipe { P = xx, X = ii, Y = i })).ToList());
    Console.WriteLine($"File {file.Title}: Den Count: {grid.IsDen()}");
}

internal class Pipe
{
    internal int X { get; set; }
    internal int Y { get; set; }
    internal char P { get; set; }
}

internal class Grid
{
    public List<Pipe> Route { get; set; } = new();
    internal List<Pipe> Pipes { get; set; } = new();
    internal Grid(List<Pipe> pipes)
    {
        Pipes = pipes;
        GetRoute();
    }
    internal Pipe GetStart()
    {
        return Pipes.First(x => x.P == 'S');
    }

    internal void GetRoute()
    {
        Route.Add(GetStart());
        do
        {
            var next = GetNextPipe(Route.Last());
            if (next != null)
            {
                Route.Add(next);
            }
            else
            {
                break;
            }
        }
        while (true);
    }

    private Pipe? GetNextPipe(Pipe pipe)
    {
        return GetConnectedPipes(pipe).FirstOrDefault(x => !Route.Contains(x));
    }

    private List<Pipe> GetConnectedPipes(Pipe pipe)
    {
        return Pipes.Where(x =>
            x != pipe &&
            (
                x.X == pipe.X && x.Y == pipe.Y - 1 && !"-7F".Contains(pipe.P) && "|F7".Contains(x.P) ||
                x.X == pipe.X && x.Y == pipe.Y + 1 && !"-JL".Contains(pipe.P) && "|LJ".Contains(x.P) ||
                x.Y == pipe.Y && x.X == pipe.X - 1 && !"|LF".Contains(pipe.P) && "-FL".Contains(x.P) ||
                x.Y == pipe.Y && x.X == pipe.X + 1 && !"|J7".Contains(pipe.P) && "-J7".Contains(x.P)
            )
        ).ToList();
    }

    internal int IsDen()
    {
        return Pipes.Where(x => !Route.Contains(x)).Sum(point => IsInside(point, Route.ToArray()) ? 1 : 0);
    }

    private static bool IsInside(Pipe p, Pipe[] route)
    {
        return route.Where((x, i) =>
            {
                var ii = i - 1 < 0 ? route.Length - 1 : i - 1;
                return (x.Y > p.Y) != (route[ii].Y > p.Y) &&
                p.X < (route[ii].X - x.X) * (p.Y - x.Y) / (route[ii].Y - x.Y) + x.X;
            }
        ).Count() % 2 != 0;
    }
}
