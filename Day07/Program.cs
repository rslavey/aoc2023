Console.WriteLine("Part 01");

var files01 = new List<(string Title, string Path)>
{
    ("Test Inputs", ".\\inputs\\inputTest01.txt"),
    ("Puzzle Inputs", ".\\inputs\\input.txt")
};

for (var part = 1; part <= 2; part++)
{
    Console.WriteLine($"Part {part}");
    foreach (var file in files01)
    {
        var lines = File.ReadAllLines(file.Path);

        Console.WriteLine($@"Score: {lines.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
			.Select(x => new CamelHand { Hand = x[0].Trim(), Bid = int.Parse(x[1].Trim()) })
			.OrderBy(x => x, new HigherHand(part == 2))
			.Select((x, i) => x.Bid * (i + 1))
			.ToList()
			.Sum()}");
    }
}


public class CamelHand
{
    public string Hand { get; set; }
    public int Bid { get; set; }
}

public class HigherHand : IComparer<CamelHand>
{
    private bool _isWild = false;

    public HigherHand(bool isWild)
    {
        _isWild = isWild;
    }

    public int Compare(CamelHand x, CamelHand y)
    {
        var xHand = GetHand(x.Hand);
        var yHand = GetHand(y.Hand);
        var xval = _isWild ? GetHandValue(GetWildHandRanking(x.Hand)) : GetHandValue(xHand);
        var yval = _isWild ? GetHandValue(GetWildHandRanking(y.Hand)) : GetHandValue(yHand);

        if (xval > yval)
        {
            return 1;
        }
        else if (xval < yval)
        {
            return -1;
        }
        else
        {
            for (var i = 0; i < xHand.Length; i++)
            {
                if (xHand[i] > yHand[i])
                {
                    return 1;
                }
                else if (xHand[i] < yHand[i])
                {
                    return -1;
                }
            }
        }
        return 0;
    }
    int GetCardValue (char a)
    {
        return a == 'A' ? 14 : a == 'K' ? 13 : a == 'Q' ? 12 : a == 'J' ? 11 : a == 'T' ? 10 : int.Parse(a.ToString());
    }
    int[] GetHand(string v)
    {
        return v.Select(a => _isWild ? GetCardValue(a) == 11 ? 1 : GetCardValue(a) : GetCardValue(a)).ToArray();
    }
    int GetHandValue(int[] a)
    {
        var numCount = a.Distinct().Count();
        var numFreq = a.GroupBy(x => x).Max(x => x.Count());
		return 5 - numCount + numFreq;
    }
    int[] GetWildHandRanking(string v)
    {
        if (v == "JJJJJ")
        {
            return GetHand("AAAAA");
        }
        var natCardsBest = v.Replace("J", "").GroupBy(x => x).OrderByDescending(x => x.Count()).ThenByDescending(x => GetCardValue(x.Key)).First().Key;

        return GetHand(v.Replace('J', natCardsBest));
    }

}
