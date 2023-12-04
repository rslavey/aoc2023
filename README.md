# Advent of Code 2023

## Day One

A simple enough start to this year's challenge, but I admit I got snookered by the allowance of run-on words in Part 2 (e.g., oneleven is valid for both 1 and 11). I started with a simple text replacement from a dictionary, but that wasn't going to work with words merged together like this, so I had to do a text matching with StartsWith instead.

Also, using a test set in Part 2 that was not valid in Part 1 ("eightwothree") was kind of a pain as I had to separate my test sets into two groups to run parts 1 and 2 at the same time.

### TIL (or was reminded)

- Read the requirements a few times to be sure you truly understand them
- Index ranges in arrays

```cs
var test = "advent of code";
Console.WriteLine(test[0..6]);  // "advent"
Console.WriteLine(test[3..]);  // "ent of code"
Console.WriteLine(test[..9]); // "advent of"
```

- A Dictionary<string,int> will return 0 not null in a FirstOrDefault() situation when an item does not exist in its IEnumerable

```cs
// Bad Code
var test = new Dictionary<string, int> { { "zero", 0 }, { "one", 1 } };
var expectNull = numberWords.FirstOrDefault(numberWords => numberWords.Key == "bbb").Value; // Expect null, returns 0, as the default value of an int is zero
var expectZero = numberWords.FirstOrDefault(numberWords => numberWords.Key == "zero").Value; // Returns 0

//Good Code
var test = new Dictionary<string, int?> { { "zero", 0 }, { "one", 1 } }; // Note the nullabe int
var expectNull = numberWords.FirstOrDefault(numberWords => numberWords.Key == "bbb").Value; // Expect null, returns null, as the default value of an int? is null
var expectZero = numberWords.FirstOrDefault(numberWords => numberWords.Key == "zero").Value; // Returns 0

```

## Day Two

I found this much simpler than Day One. Most of the time was taken parsing the text.

For Part 2, I wrote a test to ensure that every line had at least one draw of each color. I've learned from previous years and other challenges that they can throw bad data at you, and since the value they were looking for was a product, any missing values was going to return 0.

### TIL (or was reminded)

- Nothing, really. Knowledge of LINQ's "All" statement made this a snap.

## Day Three

More dictionaries. No time to share thoughts tonight; day job work to do.

### TIL (or was reminded)

- Sometimes good enough is good enough. 100% sure this is not optimal code, but it'll do, pig.
