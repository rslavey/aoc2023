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

## Day Four

My mind immediately went to recursion when I read Part 02, but after further consideration, it looked like a dictionary was once again sufficient. Pretty happy with the simplicity of this one and how quickly it runs.

### TIL (or was reminded)

- ```StringSplitOptions.RemoveEmptyEntries``` for those pesky spaces

## Day Five

I knew going into this one we were going to have to use long (Int64) variables. That means no built-in Range functionality. I also assumed brute force wasn't going to cut it once I saw the test data. Sure enough, Part 2 having billions of numbers was going to be a deal breaker for testing every value.

I'll admit this one stumped me for quite a while. I knew *what* to do, but my numbers were just never adding up, so I did what I always do and broke it all down into distinct variables so I could step through it. Sure enough, I was not setting my ranges correctly (my end values were inclusive). Once I fixed that, I was getting the correct answer, and, more importantly, in milliseconds.

### TIL (or was reminded)

- Aggregate works great in most cases, but I probably forced it here where a for loop would've been just fine.
- Classes and methods are your friends. Kept rewriting the source/destination logic until I realized I could just dump it into a method and never think about it again.
- Ranges; oof. Every time I think I'm a decent programmer, it's shit like this that reminds me why I'm a data guy and not a math guy.

## Day Six

A pleasant respite after yesterday. Probably would've been at least close to the top 100, but I waited until 30m prior to the challenge to install Visual Studio on my laptop. Who'd've thought that wouldn't be enough time to install and configure it.

### TIL (or was reminded)

- You'd think I would have learned my lesson when it was just two three days ago that I said, "Good enough is good enough." Got the answers, then went back and refactored it to reduce the time by...about 300ms.

## Day Seven

A custom IComparer and a quick fix for the exception "JJJJJ". Got stuck for a while because I had 

```cs
int GetCardValue (int a)
```
instead of

```cs
int GetCardValue (char a)
```

and the code was happy to take 'K' as an int and use its ascii value

### TIL (or was reminded)

- If you want two comparers that are just a little different, you can pass a value when you new it up and use that to tweak the Compare() method

## Day Eight

There are a few functions you should always have on hand when doing programming challenges because it is inevitable that at some point you're going to need a quick prime number check or a greatest common denominator.

I'm not quite satisfied with this one. I have to do a lookup on the Dictionary to return the whole KeyValuePair so I can do a string search of the key. I'm sure there's a better way to still get the value of the hash of the Dictionary, but a couple of seconds was fast enough.

### TIL (or was reminded)

- Always have your GCD, LCM, and IsPrime functions handy
- Lambda switches are fun

## Day Nine

This one was kind of weird. I thought there'd be some confusing thing about it not working with large sets unless you do some math or something, but all I did was follow the instructions in the challenge and immediately got the right answers on both parts.

### TIL (or was reminded)

- Follow the directions?