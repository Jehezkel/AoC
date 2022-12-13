// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var filePath = args.Count() > 0 && args[0] == "prod" ? "./input.txt" : "./input.txt";
var input = File.ReadAllText(filePath);
var packetPairsList = ParseInput(input);
int i = 1;
int sum = 0;
List<int> results = new();
foreach (var pair in packetPairsList)
{
    Console.WriteLine($"== Pair {i} ==");
    if (pair.left < pair.right)
    {
        Console.WriteLine("Fine!");
        results.Add(i);
    }
    i++;
    Console.WriteLine();
}
Console.WriteLine(results.Sum());
List<PacketPair> ParseInput(string input)
{
    List<PacketPair> result = new();
    var pairsOfPackets = input.Split("\n\n");
    foreach (var pair in pairsOfPackets)
    {
        var leftPacket = pair.Split('\n')[0];
        var rightPacket = pair.Split('\n')[1];
        var parsedL = ParseList(leftPacket);
        var parsedR = ParseList(rightPacket);
        result.Add(new PacketPair(parsedL, parsedR));
    }
    return result;
}
BucketOfElements ParseList(string bracketsToParse)
{
    BucketOfElements result = new();
    //Remove outer brackets
    var items = bracketsToParse.Substring(1, bracketsToParse.Length - 2);
    string numberAsString = "";
    for (int i = 0; i < items.Length; i++)
    {
        var currChar = items[i];
        if (currChar == '[')
        {
            var closingBracketIndex = GetClosingIndex(items.Substring(i));
            var listLength = closingBracketIndex + 1;
            var listToAdd = listLength == 0 ? new BucketOfElements() : ParseList(items.Substring(i, listLength));
            result.Elements.Add(listToAdd);
            i += listLength;
            continue;
        }
        if (Char.IsDigit(currChar)) numberAsString += currChar;
        if (currChar == ',' || i == items.Length - 1)
        {
            if (numberAsString.Length > 0) result.Elements.Add(new DirectValueElement(Convert.ToInt32(numberAsString)));
            numberAsString = "";
        }
    }
    return result;
}
int GetClosingIndex(string line)
{
    // each open bracket delays our closing bracket by one.
    var regex = new Regex(@"(?<open>\[)|(?<close>\])");
    var allBrackets = regex.Matches(line);
    (int openBracketsCounter, int matchesIterator) = (0, 0);
    Match myClosingBracket;
    do
    {
        myClosingBracket = allBrackets[matchesIterator];
        openBracketsCounter = myClosingBracket.Value == "[" ? openBracketsCounter + 1 : openBracketsCounter - 1;
        matchesIterator++;
    } while (openBracketsCounter > 0);
    return myClosingBracket.Index;
}

abstract record PacketElement
{
    public static bool operator >(PacketElement a, PacketElement b)
    {

        if (a is DirectValueElement && b is DirectValueElement)
        {
            var dA = a as DirectValueElement;
            var dB = b as DirectValueElement;
            return dA! > dB!;
        }

        var bA = ConvertToBucket(a);
        var bB = ConvertToBucket(b);
        return bA > bB;
    }
    public static bool operator <(PacketElement a, PacketElement b)
    {
        if (a is DirectValueElement && b is DirectValueElement)
        {
            var dA = a as DirectValueElement;
            var dB = b as DirectValueElement;
            return dA! < dB!;
        }

        var bA = ConvertToBucket(a);
        var bB = ConvertToBucket(b);
        return bA < bB;
    }
    private static BucketOfElements ConvertToBucket(PacketElement p)
    {
        if (p is BucketOfElements) return (BucketOfElements)p;

        var dP = p as DirectValueElement;
        var bP = (BucketOfElements)dP;
        Console.WriteLine($"Mixed types; convert left to {bP.ToString()} and retry comparison");
        return bP;
    }
    // public abstract void Print();
}
record DirectValueElement(int value) : PacketElement
{
    public static bool operator >(DirectValueElement a, DirectValueElement b)
    {
        Console.WriteLine($"Compare {a.ToString()} vs {b.ToString()}");
        return a.value > b.value;
    }
    public static bool operator <(DirectValueElement a, DirectValueElement b)
    {
        Console.WriteLine($"Compare {a.ToString()} vs {b.ToString()}");
        return a.value < b.value;
    }
    // public static bool operator ==(DirectValueElement a, DirectValueElement b)
    // {
    //     Console.WriteLine($"Compare {a.ToString()} vs {b.ToString()}");
    //     return a.value < b.value;
    // }
    public override string ToString() => value.ToString();
}
record BucketOfElements : PacketElement
{
    public BucketOfElements() { }
    public BucketOfElements(DirectValueElement pe) { this.Elements.Add(pe); }
    public List<PacketElement> Elements { get; set; } = new();
    public static explicit operator BucketOfElements(DirectValueElement pe) => new(pe);
    public static bool operator >(BucketOfElements left, BucketOfElements right)
    {
        Console.WriteLine($"Compare left{left.ToString()} vs {right.ToString()}");
        for (int i = 0; i < left.Elements.Count; i++)
        {
            var leftElement = left.Elements[i];
            //right is out of items = left>right
            if (i >= right.Elements.Count) return true;
            var rightElement = right.Elements[i];
            if (leftElement > rightElement) return true;
        }
        return false;
    }
    public static bool operator <(BucketOfElements left, BucketOfElements right)
    {
        Console.WriteLine($"Compare left{left.ToString()} vs {right.ToString()}");

        for (int i = 0; i < left.Elements.Count; i++)
        {
            var leftElement = left.Elements[i];
            //right is out of items = left>right
            if (i >= right.Elements.Count)
            {
                Console.WriteLine("Right side ran out of items, so inputs are not in the right order");
                return false;
            }
            var rightElement = right.Elements[i];
            if (rightElement.Equals(leftElement)) continue;
            if (leftElement < rightElement)
            {
                Console.WriteLine("Left side is smaller, so inputs are in the right order");
                return true;
            }
            if (leftElement > rightElement)
            {
                Console.WriteLine("Right side is smaller, so inputs are not in the right order");
                return false;
            };
        }
        if (left.Elements.Count < right.Elements.Count)
        {
            Console.WriteLine("Left side ran out of items, so inputs are in the right order");
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        var innerItems = String.Join(',', this.Elements.Select(e => e.ToString()));
        return $"[{innerItems}]";
    }
}
record PacketPair(BucketOfElements left, BucketOfElements right);
