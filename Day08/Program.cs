Console.WriteLine("Part 01");

var files01 = new List<(string Title, string Path)>
{
	("Test Inputs 01", ".\\inputs\\inputTest01_01.txt"),
	("Test Inputs 02", ".\\inputs\\inputTest01_02.txt"),
	("Puzzle Inputs", ".\\inputs\\input.txt")
};

foreach (var file in files01)
{
	var instructions = File.ReadAllLines(file.Path)[0];
	var nodes = GetNodes(file);
	var curLoc = nodes["AAA"];
	var steps = 0;
	var instruction = 0;

	while (curLoc != nodes["ZZZ"])
	{
		curLoc = instructions[instruction] switch
		{
			'L' => nodes[curLoc.lNode],
			'R' => nodes[curLoc.rNode],
			_ => throw new Exception("Invalid instruction")
		};
		instruction = ++instruction == instructions.Length ? 0 : instruction;
		steps++;
	}

	Console.WriteLine($"Input {file.Title}, Steps: {steps}");
}

Console.WriteLine("\nPart 01");

var files02 = new List<(string Title, string Path)>
{
	("Test Inputs 01", ".\\inputs\\inputTest02_01.txt"),
	("Puzzle Inputs", ".\\inputs\\input.txt")
};

foreach (var file in files02)
{
	var instructions = File.ReadAllLines(file.Path)[0];
	var nodes = GetNodes(file);
	var startLoc = nodes.Where(x => x.Key.EndsWith('A')).ToArray();
	long totalSteps = 1;
	
	for (var i = 0; i < startLoc.Length; i++)
	{
		long steps = 0;
		var instruction = 0;
		var curLoc = startLoc[i];
		while (!curLoc.Key.EndsWith('Z'))
		{
			curLoc = instructions[instruction] switch
			{
				'L' => nodes.FirstOrDefault(x => x.Key == curLoc.Value.lNode),
				'R' => nodes.FirstOrDefault(x => x.Key == curLoc.Value.rNode),
				_ => throw new Exception("Invalid instruction")
			};
			instruction = ++instruction == instructions.Length ? 0 : instruction;
			steps++;
		}
		totalSteps = LCM(steps, totalSteps);
	}

	Console.WriteLine($"Input {file.Title}, Steps: {totalSteps}");
}

static Dictionary<string, (string lNode, string rNode)> GetNodes((string Title, string Path) file)
{
	var lines = File.ReadAllLines(file.Path);
	return lines[2..].ToDictionary(x => x.Split('=', StringSplitOptions.TrimEntries)[0], x =>
	{
		var rl = x.Split('=', StringSplitOptions.TrimEntries)[1].Split(',').Select(x => x.Replace("(", "").Replace(")", "").Trim()).ToArray();
		return (rl[0], rl[1]);
	});
}

static long LCM(long a, long b)
{
	return (a / GCD(a, b)) * b;
}

static long GCD(long a, long b)
{
	while (a != 0 && b != 0)
	{
		if (a > b)
			a %= b;
		else
			b %= a;
	}
	return a | b;
}
