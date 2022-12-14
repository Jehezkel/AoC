// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var filePath = args.Count() > 0 && args[0] == "prod" ? "./input.txt" : "./input.txt";
var input = File.ReadAllText(filePath);
var packetList = ParseInput(input);
var distressPackets = new List<BucketOfElements>(){
    new BucketOfElements(new BucketOfElements(new DirectValueElement(2))),
    new BucketOfElements(new BucketOfElements(new DirectValueElement(6)))
};

packetList.AddRange(distressPackets);
var x = packetList.OrderBy(p=> p).ToList();
string[] final=x.Select(x=> x.ToString()).ToArray();
int decoderKey = 1;
 distressPackets.ForEach(p=> decoderKey *=x.IndexOf(p)+1);
Console.WriteLine("Final");

Console.WriteLine($"{String.Join('\n',final)}");
Console.WriteLine("Decoder key: "+decoderKey);
List<BucketOfElements> ParseInput(string input)
{
    List<BucketOfElements> result = new();
    var pairsOfPackets = input.Split("\n\n");
    foreach (var pair in pairsOfPackets)
    {
        var leftPacket = pair.Split('\n')[0];
        var rightPacket = pair.Split('\n')[1];
        var parsedL = ParseList(leftPacket);
        result.Add(parsedL);
        var parsedR = ParseList(rightPacket);
        result.Add(parsedR);
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

abstract record PacketElement : IComparable<PacketElement>
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

    public int CompareTo(PacketElement? other)
    {
        // return this >other ? 1:0;
        if(this < other) return -1;
        if(other < this) return 1;
        return 0;
    }
}
record DirectValueElement(int value) : PacketElement
{
    public static bool operator >(DirectValueElement a, DirectValueElement b) => a.value > b.value;
    public static bool operator <(DirectValueElement a, DirectValueElement b) => a.value < b.value;
    public override string ToString() => value.ToString();
}
record BucketOfElements : PacketElement
{
    public BucketOfElements() { }
    public BucketOfElements(PacketElement pe) { this.Elements.Add(pe); }
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
            if (rightElement.Equals(leftElement)) continue;
            if (leftElement > rightElement)
            {
                Console.WriteLine("Left side is smaller, so inputs are in the right order");
                return true;
            }
            if (leftElement < rightElement)
            {
                Console.WriteLine("Right side is smaller, so inputs are not in the right order");
                return false;
            };
        }
        return false;
    }
    public static bool operator <(BucketOfElements left, BucketOfElements right)
    {
        var lx = left.ToString();
        var rx = right.ToString();
        Console.WriteLine($"Compare {left.ToString()} vs {right.ToString()}");

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
            Console.WriteLine($"Compare {leftElement.ToString()} vs {rightElement.ToString()}");
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
    
    public int CompareTo(BucketOfElements? other)
    {
        if(this < other) return -1;
        if(other < this) return 1;
        return 0;
        // if(this = other) return -0;
    }
}
record PacketPair(BucketOfElements left, BucketOfElements right);
