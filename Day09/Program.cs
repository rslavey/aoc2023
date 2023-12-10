var files01 = new List<(string Title, string Path)>
{
	("Test Inputs 01", ".\\inputs\\inputTest01_01.txt"),
	("Puzzle Inputs", ".\\inputs\\input.txt")
};

foreach (var file in files01)
{
	var lines = File.ReadAllLines(file.Path);
	var total01 = 0;
	var total02 = 0;
	foreach(var line in lines)
	{
		var histories = new int[][] { line.Split(' ').Select(x => int.Parse(x)).ToArray()};
		while (!histories.Last().All(x => x == 0))
		{
            histories = histories.Append(histories.Last().Where((x, i) => i < histories.Last().Length - 1).Select((x, i) => histories.Last()[i + 1] - x).ToArray()).ToArray();
        }
        total01 += histories.Skip(1).Select(x => x.Last()).Sum() + histories.First().Last();
        total02 += histories.First().First() - (histories.Skip(1).Reverse().Select(x => x.First()).Aggregate(0, (x, y) => y - x));
    }
    Console.WriteLine($"{file.Title}: Part 01: {total01}");
    Console.WriteLine($"{file.Title}: Part 02: {total02}");
}
