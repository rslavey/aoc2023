Console.WriteLine("Part 01");
var files01 = new List<(string Title, string Path)> { ("Test Inputs", ".\\inputs\\inputTest01.txt"), ("Puzzle Inputs", ".\\inputs\\input.txt") };
foreach (var file in files01)
{
	Console.Write(file.Title + ":\t");
	Console.WriteLine(File.ReadAllLines(file.Path).Select(line => int.Parse(line.First(c => char.IsNumber(c)).ToString() + line.Last(c => char.IsNumber(c)).ToString())).Sum());
}

Console.WriteLine("\nPart 02");
var numberWords = new Dictionary<string, int?>
{
	{ "one", 1 },
	{ "two", 2 },
	{ "three", 3 },
	{ "four", 4 },
	{ "five", 5 },
	{ "six", 6 },
	{ "seven", 7 },
	{ "eight", 8 },
	{ "nine", 9 }
};

var files02 = new List<(string Title, string Path)> { ("Test Inputs", ".\\inputs\\inputTest02.txt"), ("Puzzle Inputs", ".\\inputs\\input.txt") };
foreach (var file in files02)
{
	Console.Write(file.Title + ":\t");
	Console.WriteLine(
		File.ReadAllLines(file.Path).Select(line =>
			int.Parse(
				line.Select((c, i) =>
						char.IsNumber(c) ? c.ToString() : numberWords.FirstOrDefault(nw => line[i..].StartsWith(nw.Key)).Value.ToString() ?? string.Empty
				).First(x => x != string.Empty)
				+
				line.Select((c, i) =>
						char.IsNumber(c) ? c.ToString() : numberWords.FirstOrDefault(nw => line[i..].StartsWith(nw.Key)).Value.ToString() ?? string.Empty
				).Last(x => x != string.Empty)
			)
		).Sum()
	);
}
