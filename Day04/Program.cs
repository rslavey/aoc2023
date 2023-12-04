Console.WriteLine("Part 01\n");
var files01 = new List<(string Title, string Path)> { ("Test Inputs", ".\\inputs\\inputTest01.txt"), ("Puzzle Inputs", ".\\inputs\\input.txt") };
foreach (var file in files01)
{
	Console.WriteLine(file.Title);
	Console.WriteLine($"Points: {File.ReadLines(file.Path).Select(x => GetWins(x)).Sum(x => x > 0 ? (int)Math.Pow(2, x - 1) : 0)}\n");
}

Console.WriteLine("Part 02");
var files02 = new List<(string Title, string Path)> { ("Test Inputs", ".\\inputs\\inputTest02.txt"), ("Puzzle Inputs", ".\\inputs\\input.txt") };
foreach (var file in files02)
{
	Console.WriteLine(file.Title);
	Dictionary<int, int> cards = new();

	foreach(var c in File.ReadLines(file.Path).Select((x,i) => (x, i)))
	{
		cards[c.i] = cards.ContainsKey(c.i) ? cards[c.i] + 1 : 1;
		var wins = GetWins(c.x);
		for (var ii = 1; ii <= wins; ii++)
		{
			cards[c.i + ii] = cards.ContainsKey(c.i + ii) ? cards[c.i + ii] + cards[c.i] : cards[c.i];
		}
	}

	Console.WriteLine($"Points: {cards.Sum(x => x.Value)}\n");
}

static int GetWins(string card)
{
	var nums = card[(card.IndexOf(':') + 1)..].Split("|", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
	return nums[0].Intersect(nums[1]).Count();
}