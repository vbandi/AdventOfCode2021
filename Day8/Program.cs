/*
--- Day 8: Seven Segment Search ---
You barely reach the safety of the cave when the whale smashes into the cave mouth, collapsing it. Sensors indicate another exit to this cave at a much greater depth, so you have no choice but to press on.

As your submarine slowly makes its way through the cave system, you notice that the four-digit seven-segment displays in your submarine are malfunctioning; they must have been damaged during the escape. You'll be in a lot of trouble without them, so you'd better figure out what's wrong.

Each digit of a seven-segment display is rendered by turning on or off any of seven segments named a through g:

  0:      1:      2:      3:      4:
 aaaa    ....    aaaa    aaaa    ....
b    c  .    c  .    c  .    c  b    c
b    c  .    c  .    c  .    c  b    c
 ....    ....    dddd    dddd    dddd
e    f  .    f  e    .  .    f  .    f
e    f  .    f  e    .  .    f  .    f
 gggg    ....    gggg    gggg    ....

  5:      6:      7:      8:      9:
 aaaa    aaaa    aaaa    aaaa    aaaa
b    .  b    .  .    c  b    c  b    c
b    .  b    .  .    c  b    c  b    c
 dddd    dddd    ....    dddd    dddd
.    f  e    f  .    f  e    f  .    f
.    f  e    f  .    f  e    f  .    f
 gggg    gggg    ....    gggg    gggg
So, to render a 1, only segments c and f would be turned on; the rest would be off. To render a 7, only segments a, c, and f would be turned on.

The problem is that the signals which control the segments have been mixed up on each display. The submarine is still trying to display numbers by producing output on signal wires a through g, but those wires are connected to segments randomly. Worse, the wire/segment connections are mixed up separately for each four-digit display! (All of the digits within a display use the same connections, though.)

So, you might know that only signal wires b and g are turned on, but that doesn't mean segments b and g are turned on: the only digit that uses two segments is 1, so it must mean segments c and f are meant to be on. With just that information, you still can't tell which wire (b/g) goes to which segment (c/f). For that, you'll need to collect more information.

For each display, you watch the changing signals for a while, make a note of all ten unique signal patterns you see, and then write down a single four digit output value (your puzzle input). Using the signal patterns, you should be able to work out which pattern corresponds to which digit.

For example, here is what you might see in a single entry in your notes:

acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |
cdfeb fcadb cdfeb cdbaf
(The entry is wrapped here to two lines so it fits; in your notes, it will all be on a single line.)

Each entry consists of ten unique signal patterns, a | delimiter, and finally the four digit output value. Within an entry, the same wire/segment connections are used (but you don't know what the connections actually are). The unique signal patterns correspond to the ten different ways the submarine tries to render a digit using the current wire/segment connections. Because 7 is the only digit that uses three segments, dab in the above example means that to render a 7, signal lines d, a, and b are on. Because 4 is the only digit that uses four segments, eafb means that to render a 4, signal lines e, a, f, and b are on.

Using this information, you should be able to work out which combination of signal wires corresponds to each of the ten digits. Then, you can decode the four digit output value. Unfortunately, in the above example, all of the digits in the output value (cdfeb fcadb cdfeb cdbaf) use five segments and are more difficult to deduce.

For now, focus on the easy digits. Consider this larger example:

be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb |
fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec |
fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef |
cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega |
efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga |
gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf |
gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf |
cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd |
ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg |
gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc |
fgae cfgab fg bagce
Because the digits 1, 4, 7, and 8 each use a unique number of segments, you should be able to tell which combinations of signals correspond to those digits. Counting only digits in the output values (the part after | on each line), in the above example, there are 26 instances of digits that use a unique number of segments (highlighted above).

In the output values, how many times do digits 1, 4, 7, or 8 appear?
*/

Dictionary<int, string> segments = new Dictionary<int, string>
{ 
    {0, "abcefg"},
    {1, "cf"},
    {2, "acdeg"},
    {3, "acdfg"},
    {4, "bcdf"},
    {5, "abdfg"},
    {6, "abdefg"},
    {7, "acf"},
    {8, "abcdefg"},
    {9, "abcdfg"}
};

Dictionary<int, int> uniqueSegmentCount = new Dictionary<int, int>();
uniqueSegmentCount.Add(1, segments[1].Length);
uniqueSegmentCount.Add(4, segments[4].Length);
uniqueSegmentCount.Add(7, segments[7].Length);
uniqueSegmentCount.Add(8, segments[8].Length);

int? GetDigitUniqueBySegmentCount(int segmentCount)
{
    var result = uniqueSegmentCount.FirstOrDefault(x => x.Value == segmentCount);
    if (result.Key == 0)
    {
        return null;
    }
    Console.WriteLine($"Found digit {result.Key} with {segmentCount} segments");
    return result.Key;
}

List<string[]> patterns;
List<string[]> outputs;

void ParseInput(string input)
{
    patterns = new List<string[]>();
    outputs = new List<string[]>();
 
    var lines = input.Split('\n');
    foreach (var line in lines)
    {
        var segments = line.Split('|');
        patterns.Add(segments[0].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        outputs.Add(segments[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }
}

var sampleInput = @"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce";

ParseInput(sampleInput);

var sampleResult = outputs.Sum(x => x.Count(y => GetDigitUniqueBySegmentCount(y.Length) != null));
Console.WriteLine(sampleResult);

var input = File.ReadAllText("input.txt");
ParseInput(input);

var result = outputs.Sum(x => x.Count(y => GetDigitUniqueBySegmentCount(y.Length) != null));
Console.WriteLine(result);

/*
--- Part Two ---
Through a little deduction, you should now be able to determine the remaining digits. Consider again the first example above:

acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |
cdfeb fcadb cdfeb cdbaffor
After some careful analysis, the mapping between signal wires and segments only make sense in the following configuration:

 dddd
e    a
e    a
 ffff
g    b
g    b
 cccc
So, the unique signal patterns would correspond to the following digits:

acedgfb: 8
cdfbe: 5
gcdfa: 2
fbcad: 3
dab: 7
cefabd: 9
cdfgeb: 6
eafb: 4
cagedb: 0
ab: 1
Then, the four digits of the output value can be decoded:

cdfeb: 5
fcadb: 3
cdfeb: 5
cdbaf: 3
Therefore, the output value for this entry is 5353.

Following this same process for each entry in the second, larger example above, the output value of each entry can be determined:

fdgacbe cefdb cefbgd gcbe: 8394
fcgedb cgb dgebacf gc: 9781
cg cg fdcagb cbg: 1197
efabcd cedba gadfec cb: 9361
gecf egdcabf bgf bfgea: 4873
gebdcfa ecba ca fadegcb: 8418
cefg dcbef fcge gbcadfe: 4548
ed bcgafe cdgba cbgef: 1625
gbdfcae bgc cg cgb: 8717
fgae cfgab fg bagce: 4315
Adding all of the output values in this larger example produces 61229.

For each entry, determine all of the wire/segment connections and decode the four-digit output values. What do you get if you add up all of the output values?
*/

/*

Digits / number of active segments:

  0/6:   1/2:     2/5:    3/5:    4/4:
 aaaa    ....    aaaa    aaaa    ....
b    c  .    c  .    c  .    c  b    c
b    c  .    c  .    c  .    c  b    c
 ....    ....    dddd    dddd    dddd
e    f  .    f  e    .  .    f  .    f
e    f  .    f  e    .  .    f  .    f
 gggg    ....    gggg    gggg    ....

  5/5:    6/6:    7/3:    8/7:    9/6:
 aaaa    aaaa    aaaa    aaaa    aaaa
b    .  b    .  .    c  b    c  b    c
b    .  b    .  .    c  b    c  b    c
 dddd    dddd    ....    dddd    dddd
.    f  e    f  .    f  e    f  .    f
.    f  e    f  .    f  e    f  .    f
 gggg    gggg    ....    gggg    gggg
*/

result = 0;

for (int i = 0; i < patterns.Count; i++)
{
    var pattern = patterns[i];
    var output = outputs[i];

    var entry = pattern.Concat(output).Select(s => new string(s.OrderBy(sss => sss).ToArray()));

    var candidates = new Dictionary<char, List<char>>();
    candidates['a'] = "abcdefg".ToList();
    candidates['b'] = "abcdefg".ToList();
    candidates['c'] = "abcdefg".ToList();
    candidates['d'] = "abcdefg".ToList();
    candidates['e'] = "abcdefg".ToList();
    candidates['f'] = "abcdefg".ToList();
    candidates['g'] = "abcdefg".ToList();
    
    
    // for every unique number, remove the segments not in the pattern from the candidate list
    var uniqueNumbers = uniqueSegmentCount.Keys.ToList();
    foreach (var uniqueNumber in uniqueNumbers)
    {
        var entriesForUniqueNumber = entry.Where(x => x.Length == segments[uniqueNumber].Length).Distinct().ToList();
        var charsForUniqueNumber = string.Join("", entriesForUniqueNumber).Distinct();

        foreach (var c in segments[uniqueNumber])
        {
            candidates[c].RemoveAll(x => !charsForUniqueNumber.Contains(x));
        }
    }
    
    // 7 has one additional segment to 1: a
    candidates['a'].RemoveAll(x => candidates['c'].Contains(x));
    
    // since now we know the wire for a, we can remove it from all other candidates
    foreach (var c in candidates.Keys)
    {
        if (c != 'a')
        {
            candidates[c].RemoveAll(x => candidates['a'].Contains(x));
        }
    }
    
    // since we have 2 candidates for c and f, remove these from the other candidates
    foreach (var c in candidates.Keys)
    {
        if (c != 'c' && c != 'f')
        {
            candidates[c].RemoveAll(x => candidates['c'].Contains(x));
        }
    }
    
    // 0, 9 and 6 have 6 active segments. 9 and 0 both have the segments from 1 (c, f) active, 6 does not
    var possible096 = entry.Where(x => x.Count() == 6).Distinct().ToList();
    var possible6 = possible096.Where(x => !x.Contains(candidates['c'][0]) || !x.Contains(candidates['c'][1])).ToList();
    var possible6Chars = string.Join("", possible6).Distinct();
    candidates['c'] = candidates['c'].Except(possible6Chars.ToList()).ToList(); 
    candidates['f'] = candidates['f'].Intersect(possible6Chars.ToList()).ToList();
    
    candidates['e'].RemoveAll(x => candidates['b'].Contains(x));
    candidates['e'].RemoveAll(x => candidates['d'].Contains(x));
    candidates['g'].RemoveAll(x => candidates['b'].Contains(x));
    candidates['g'].RemoveAll(x => candidates['d'].Contains(x));
    
    // we have a, c, f wires at this point, the rest still have 2 candidates
    // g is active in all numbers with 5 active segments
    var possible235 = entry.Where(x => x.Count() == 5).Distinct().ToList();
    candidates['g'] = candidates['g'].Where(x => possible235.All(y => y.Contains(x))).ToList();
    candidates['e'] = candidates['e'].Except(candidates['g']).ToList();

    // b is only active in one number with 5 active segments
    candidates['b'] = candidates['b'].Where(x => possible235.Count(y => y.Contains(x)) == 1).ToList();
    candidates['d'] = candidates['d'].Except(candidates['b']).ToList();

    // verify all candidates have only one character left
    foreach (var c in candidates.Keys)
    {
        if (candidates[c].Count != 1)
        {
            throw new Exception("Candidates for " + c + " have " + candidates[c].Count + " characters left");
        }
    }
    
    // decode the output
    
    var map = candidates.ToDictionary(x => x.Value[0], x => x.Key);
    var segmentsToDigits = segments.ToDictionary(x => x.Value, x => x.Key);

    var decodedNumber = 0;
    foreach (var o in output)
    {
        var decodedSegments = new string(o.Select(x => map[x]).OrderBy(x => x).ToArray());
        var decodedDigit = segmentsToDigits[decodedSegments];
        decodedNumber = decodedNumber * 10 + decodedDigit;
    }
    
    Console.WriteLine(decodedNumber);
    result += decodedNumber;
    
    //
    //
    //
    //
    //
    //
    //
    //
    // foreach (var candidate in candidates)
    // {
    //     Console.WriteLine($"{candidate.Key} = {new string(candidate.Value.ToArray())}");
    // }
}

Console.WriteLine(result);


/*
    // for every unique number, remove the segments not in the pattern from the candidate list
    var uniqueNumbers = uniqueSegmentCount.Keys.ToList();
    foreach (var uniqueNumber in uniqueNumbers)
    {
        var entriesForUniqueNumber = entry.Where(x => x.Length == segments[uniqueNumber].Length).Distinct().ToList();
        var charsForUniqueNumber = string.Join("", entriesForUniqueNumber).Distinct();

        foreach (var c in segments[uniqueNumber])
        {
            candidates[c].RemoveAll(x => !charsForUniqueNumber.Contains(x));
        }
    }
    
    // 7 has one additional segment to 1: a
    candidates['a'].RemoveAll(x => candidates['c'].Contains(x));
    
    // since now we know the wire for a, we can remove it from all other candidates
    foreach (var c in candidates.Keys)
    {
        if (c != 'a')
        {
            candidates[c].RemoveAll(x => candidates['a'].Contains(x));
        }
    }
    
    // since we have 2 candidates for c and f, remove these from the other candidates
    foreach (var c in candidates.Keys)
    {
        if (c != 'c' && c != 'f')
        {
            candidates[c].RemoveAll(x => candidates['c'].Contains(x));
        }
    }
    
    // 0, 9 and 6 have 6 active segments. 9 and 0 both have the segments from 1 (c, f) active, 6 does not
    var possible096 = entry.Where(x => x.Length == 6).Distinct().ToList();
    var possible6 = possible096.Where(x => !x.Contains(candidates['c'][0]) || !x.Contains(candidates['c'][1])).ToList();
    var possible6Chars = string.Join("", possible6).Distinct();
    candidates['c'] = candidates['c'].Except(possible6Chars.ToList()).ToList(); 
    candidates['f'] = candidates['f'].Intersect(possible6Chars.ToList()).ToList();
    
    candidates['e'].RemoveAll(x => candidates['b'].Contains(x));
    candidates['e'].RemoveAll(x => candidates['d'].Contains(x));
    candidates['g'].RemoveAll(x => candidates['b'].Contains(x));
    candidates['g'].RemoveAll(x => candidates['d'].Contains(x));
    
    // we have a, c, f wires at this point, the rest still have 2 candidates
    // b is active in 4, but not in 7
    var possible7 = entry.Where(x => x.Length == segments[7].Length).Distinct().ToList();
    var possible7Chars = string.Join("", possible7).Distinct();
    candidates['b'].RemoveAll(x => possible7Chars.Contains(x));
    candidates['d'].RemoveAll(x => candidates['b'].Contains(x));
    
*/    