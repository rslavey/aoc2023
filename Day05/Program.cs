var files01 = new List<(string Title, string Path)> { ("Test Inputs", ".\\inputs\\inputTest01.txt"), ("Puzzle Inputs", ".\\inputs\\input.txt") };

for (var part = 1; part <= 2; part++)
{
	foreach (var file in files01)
	{
		var seeds = new List<long>();
		var lines = File.ReadAllLines(file.Path);
		Dictionary<int, List<long>> rangeMaps = new();
		List<Map> maps = new();

		var sectionId = 0;
		for (var i = 0; i < lines.Length; i++)
		{
			if (i == 0)
			{
				var seedInput = lines[i].Split(":")[1].Trim().Split(" ").Select(x => long.Parse(x)).ToList();
				if (part == 1)
				{
					seeds.AddRange(seedInput);
				}
				else
				{
					for (var ii = 0; ii < seedInput.Count; ii += 2)
					{
						seeds.Add(seedInput[ii]);
						seeds.Add(seedInput[ii] + seedInput[ii + 1] - 1);
					}
				}
				continue;
			}
			if (lines[i].Contains("map:"))
			{
				if (!rangeMaps.ContainsKey(sectionId))
				{
					rangeMaps.Add(sectionId, new());
				}
				while (++i < lines.Length && lines[i] != string.Empty)
				{
					var map = lines[i].Split(" ").Select(x => long.Parse(x)).ToList();
					maps.Add(new Map { Id = sectionId, StartSource = map[1], EndSource = map[1] + (map[2] - 1), StartDestination = map[0], EndDestination = map[0] + (map[2] - 1) });
					rangeMaps[sectionId].Add(map[1]);
					rangeMaps[sectionId].Add(map[1] + (map[2] - 1));
				}
				rangeMaps[sectionId].AddRange(new List<long> { 0, long.MaxValue, Math.Max(0, rangeMaps[sectionId].Min() - 1), Math.Min(long.MaxValue, rangeMaps[sectionId].Max() + 1) });
				sectionId++;
				continue;
			}
		}

		var lowestVal = Int64.MaxValue;

		if (part == 1)
		{
			foreach (var seed in seeds)
			{
				lowestVal = Math.Min(lowestVal, GetSeedLocation(seed, maps));
			}
			Console.WriteLine($"Part 1 Lowest Value {file.Title}: {lowestVal}");
		}
		else
		{
			List<long> validEndpoints = rangeMaps[6];
			for (var i = 5; i >= 0; i--)
			{
				List<long> newEndpoints = new();
				foreach (var range in validEndpoints)
				{
					if (maps.FirstOrDefault(xx => xx.Id == i && xx.IsInDestinationRange(range)) != default)
					{
						newEndpoints.Add(maps.FirstOrDefault(xx => xx.Id == i && xx.IsInDestinationRange(range)).GetSource(range));
					}
					else
					{
						newEndpoints.Add(range);
					}
				}
				validEndpoints = newEndpoints.Union(rangeMaps[i]).ToList();
			}
			
			List<long> validSeeds = new();
			foreach(var validEndpoint in validEndpoints)
			{
				for (var i = 0; i < seeds.Count; i += 2)
				{
					if (validEndpoint >= seeds[i] && validEndpoint <= seeds[i + 1])
					{
						validSeeds.Add(validEndpoint);
					}
				}
			}

			foreach (var seed in validSeeds)
			{
				lowestVal = Math.Min(lowestVal, GetSeedLocation(seed, maps));
			}
			Console.WriteLine($"Part 2 Lowest Value {file.Title}: {lowestVal}");
		}

	}

}

long GetSeedLocation(long seed, List<Map> maps)
{
	var curVal = seed;
	foreach (var mapTypes in maps.GroupBy(x => x.Id))
	{
		var map = mapTypes.FirstOrDefault(x => x.StartSource <= curVal && x.EndSource >= curVal);
		if (map != null)
		{
			curVal = map.StartDestination + (curVal - map.StartSource);
		}
	}
	return curVal;
}

class Map
{
	internal int Id { get; set; }
	internal Int64 StartSource { get; set; }
	internal Int64 EndSource { get; set; }
	internal Int64 StartDestination { get; set; }
	internal Int64 EndDestination { get; set; }

	internal long GetDestination(long source)
	{
		return StartDestination + (source - StartSource);
	}

	internal long GetSource (long destination)
	{
		return StartSource + (destination - StartDestination);
	}

	internal bool IsInDestinationRange(long destination)
	{
		return destination >= StartDestination && destination <= EndDestination;
	}
}
