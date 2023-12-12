var files01 = new List<(string Title, string Path)>
{
    ("Test Inputs 01", ".\\inputs\\inputTest01_01.txt"),
    ("Puzzle Inputs", ".\\inputs\\input.txt")
};

foreach (var file in files01)
{
	Universe universe = new(File.ReadAllLines(file.Path).SelectMany((y, i) => y.ToCharArray().Select((x, ii) => new Loc { P = x, X = ii, Y = i })));
	var distances = universe.GetDistances();
	var test = distances.SelectMany((x, i) => x[i..]);
	var distanceSums = distances.SelectMany((x,i) => x[i..]).Sum();
}

var files02 = new List<(string Title, string Path)>
{
    ("Test Inputs 01", ".\\inputs\\inputTest01_01.txt"),
    ("Puzzle Inputs", ".\\inputs\\input.txt")
};

foreach (var file in files02)
{
	Universe universe = new(File.ReadAllLines(file.Path).SelectMany((y, i) => y.ToCharArray().Select((x, ii) => new Loc { P = x, X = ii, Y = i})));
	Solver solver = new Solver(universe.Galaxies.Count - 1, 6, universe.GetDistances(), seed: 99);
	Console.WriteLine("\nInitial population: ");
	solver.Show();

	Console.WriteLine("\nBegin search ");
	solver.Solve(1000);
	Console.WriteLine("Done ");

	Console.WriteLine("\nFinal population: ");
	solver.Show();

	Console.WriteLine("\nEnd demo ");
	Console.ReadLine();
}

internal class Universe
{
	public List<Galaxy> Galaxies { get; set; }
	public List<Loc> Locations { get; set; }

	public Universe(IEnumerable<Loc> input)
	{
		Locations = new List<Loc>(input);
		ExpandUniverse();
		Galaxies = Locations.Where(x => x.P == '#').Select(x => new Galaxy(x)).ToList();
	}

	internal int[][]? GetDistances()
	{
		return Galaxies.Select(x => Galaxies.Select(xx => Math.Abs(x.Location.X - xx.Location.X) + Math.Abs(x.Location.Y - xx.Location.Y)).ToArray()).ToArray();
	}

	private void ExpandUniverse()
	{
		foreach (var row in Locations.GroupBy(x => x.Y))
		{
			if (row.All(x => x.P == '.'))
			{
				var curRow = row.Key;
				foreach(var loc in Locations.Where(x => x.Y > curRow))
				{
					loc.Y += 1;
				}
				for (var i = 0; i < row.Count(); i++)
				{
					Locations.Add(new Loc { X = i, Y = curRow + 1, P = '.' });
				}
			}
		}
		for (var y = 0; y <= Locations.Max(x => x.Y); y++)
		{
			Console.WriteLine();
			for (var x = 0; x <= Locations.Max(x => x.X); x++)
			{
				Console.Write(Locations.First(loc => loc.X == x && loc.Y == y).P.ToString());
			}
		}
		Console.WriteLine();
		foreach (var col in Locations.GroupBy(x => x.X))
		{
			if (col.All(x => x.P == '.'))
			{
				var curCol = col.Key;
				foreach (var loc in Locations.Where(x => x.X > curCol))
				{
					loc.X += 1;
				}
				for (var i = 0; i < col.Count(); i++)
				{
					Locations.Add(new Loc { X = curCol + 1, Y = i, P = '.' });
				}
			}
		}
		for (var y = 0; y <= Locations.Max(x => x.Y); y++)
		{
			Console.WriteLine();
			for (var x = 0; x <= Locations.Max(x => x.X); x++)
			{
				Console.Write(Locations.First(loc => loc.X == x && loc.Y == y).P.ToString());
			}
		}
	}
}

class Solver
{
	public Random rnd;
	public int numCities;
	public int numPop; // should be even

	public int[][] distances;

	public int[][] pop;  // array of solns[]
	public double[] errs;

	public int[] bestSoln;
	public double bestErr;

	public Solver(int numCities, int numPop, int[][] distances, int seed)
	{
		this.rnd = new Random(seed);
		this.numCities = numCities;
		this.numPop = numPop;
		this.bestErr = 0.0;
		this.distances = distances;

		this.pop = new int[numPop][];  // allocate
		for (int i = 0; i < numPop; ++i)
			this.pop[i] = new int[numCities];
		this.errs = new double[numPop];

		for (int i = 0; i < numPop; ++i)  // init
		{
			for (int j = 0; j < numCities; ++j)
			{
				this.pop[i][j] = j;  // [0, 1, 2, . . ]
			}
			this.Shuffle(this.pop[i]);  // 
			this.errs[i] = this.ComputeError(this.pop[i]);
		}

		Array.Sort(this.errs, this.pop);  // parallel sort by err

		this.bestSoln = new int[numCities];
		for (int j = 0; j < this.numCities; ++j)
			this.bestSoln[j] = this.pop[0][j];
		this.bestErr = this.errs[0];
	} // ctor

	public void Show()
	{
		for (int i = 0; i < this.numPop; ++i)
		{
			for (int j = 0; j < this.numCities; ++j)
			{
				Console.Write(this.pop[i][j] + " ");
			}
			Console.WriteLine(" | " + this.errs[i].ToString("F4"));
		}

		Console.WriteLine("-----");
		for (int j = 0; j < this.numCities; ++j)
			Console.Write(this.bestSoln[j] + " ");
		Console.WriteLine(" | " + this.bestErr.ToString("F4"));
	}

	public void Shuffle(int[] soln)
	{
		// Fisher-Yates algorithm
		int n = soln.Length;
		for (int i = 0; i < n; ++i)
		{
			int ri = this.rnd.Next(i, n);  // random index
			int tmp = soln[ri];
			soln[ri] = soln[i];
			soln[i] = tmp;
		}
	}

	public double ComputeError(int[] soln)
	{
		double d = 0.0;  // total distance between cities
		int n = soln.Length;  // aka numCities
		for (int i = 0; i < n - 1; ++i)
		{
			int fromCity = soln[i];
			int toCity = soln[i + 1];
			d += this.distances[fromCity][toCity];
		}
		return d;
	}

	public int[] MakeChild(int idx1, int idx2)  // crossover
	{
		int[] parent1 = this.pop[idx1];
		int[] parent2 = this.pop[idx2];
		int[] result = new int[this.numCities];
		int[] used = new int[this.numCities];
		int[] candidates = new int[2 * this.numCities];

		int k = 0;
		for (int i = 0;
		  i < this.numCities / 2; ++i)  // left of parent1
		{
			candidates[k++] = parent1[i];
		}

		for (int i = this.numCities / 2;
		  i < this.numCities; ++i)  // right parent2
		{
			candidates[k++] = parent2[i];
		}

		for (int i = 0;
		  i < this.numCities / 2; ++i)  // left parent2
		{
			candidates[k++] = parent2[i];
		}

		for (int i = this.numCities / 2;
		  i < this.numCities; ++i)  // right parent1, never needed
		{
			candidates[k++] = parent1[i];
		}

		k = 0;
		for (int i = 0; i < this.numCities; ++i)
		{
			while (used[candidates[k]] == 1)  //  advance to unused
				k += 1;
			result[i] = candidates[k];
			used[candidates[k]] = 1;
		}

		return result;
	} // MakeChild

	public void Mutate(int[] soln)
	{
		int idx = this.rnd.Next(0, this.numCities - 1);
		int tmp = soln[idx];
		soln[idx] = soln[idx + 1];
		soln[idx + 1] = tmp;
	}

	public void Solve(int maxGen)
	{
		for (int gen = 0; gen < maxGen; ++gen)
		{
			// 1. pick parent indexes
			int idx1, idx2;
			int flip = this.rnd.Next(2);
			if (flip == 0)
			{

				idx1 = this.rnd.Next(0, this.numPop / 2);
				idx2 = this.rnd.Next(this.numPop / 2, this.numPop);
			}
			else
			{
				idx2 = this.rnd.Next(0, this.numPop / 2);
				idx1 = this.rnd.Next(this.numPop / 2, this.numPop);
			}

			// 2. create a child
			int[] child = MakeChild(idx1, idx2);
			Mutate(child);
			double childErr = this.ComputeError(child);

			// 3. found new best?
			if (childErr < this.bestErr)
			{
				Console.WriteLine("New best soltn found at generation " + gen);
				for (int j = 0; j < child.Length; ++j)
					this.bestSoln[j] = child[j];
				this.bestErr = childErr;
			}

			// 4. replace weak soln
			int idx = this.rnd.Next(this.numPop / 2, this.numPop);
			this.pop[idx] = child;
			this.errs[idx] = childErr;

			// 5. create an immigrant
			int[] imm = new int[this.numCities];
			for (int j = 0; j < this.numCities; ++j)
				imm[j] = j;
			this.Shuffle(imm);
			double immErr = this.ComputeError(imm);

			// found new best?
			if (immErr < this.bestErr)
			{
				Console.WriteLine("New best (immigrant) at generation " + gen);
				for (int j = 0; j < imm.Length; ++j)
					this.bestSoln[j] = imm[j];
				this.bestErr = immErr;
			}

			// 4. replace weak soln
			idx = this.rnd.Next(this.numPop / 2, this.numPop);
			this.pop[idx] = imm;
			this.errs[idx] = immErr;

			// 5. sort the new population
			Array.Sort(this.errs, this.pop);

		} // gen
	} // Solve

	//public static void ShowSoln(int[] soln)
	//{
	//  for (int i = 0; i < soln.Length; ++i)
	//  {
	//    Console.Write(soln[i] + " ");
	//  }
	//  Console.WriteLine("");
	//}

} // Solver

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
