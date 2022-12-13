// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var filePath = args.Count() > 0 && args[0] == "prod" ? "./input.txt" : "./test_input.txt";
var input = File.ReadAllText(filePath);
var packetPairsList = ParseInput(input);
int i = 1;
int sum = 0;
foreach (var pair in packetPairsList)
{
    Console.WriteLine($"CHecking pair {i}");
    if (pair.left.content <= pair.right.content)
    {
        Console.WriteLine("Fine!");
        sum += i;
    }
    i++;
}
Console.WriteLine();
List<PacketPair> ParseInput(string input)
{
    List<PacketPair> result = new();
    var pairsOfPackets = input.Split("\n\n");
    foreach (var pair in pairsOfPackets)
    {
        var leftPacket = pair.Split('\n')[0];
        var rightPacket = pair.Split('\n')[1];
        var parsedL = new Packet(ParseList(leftPacket));
        var parsedR = new Packet(ParseList(rightPacket));
        result.Add(new PacketPair(parsedL, parsedR));
    }
    return result;
}
PacketList ParseList(string bracketsToParse)
{
    PacketList result = new();
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
            var listToAdd = listLength == 0 ? new PacketList() : ParseList(items.Substring(i, listLength));
            result.value.Add(listToAdd);
            i += listLength;
            continue;
        }
        if (Char.IsDigit(currChar)) numberAsString += currChar;
        if (currChar == ',' || i == items.Length - 1)
        {
            if (numberAsString.Length > 0) result.value.Add(new PacketValue(Convert.ToInt32(numberAsString)));
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
    public static bool operator <=(PacketElement a, PacketElement b)
    {
        var aObject = a as PacketList;
        var bObject = b as PacketList;
        return aObject <= bObject;
    }
    public static bool operator >=(PacketElement a, PacketElement b)
    {
        var aObject = a as PacketList;
        var bObject = b as PacketList;
        return aObject >= bObject;
    }
    public static implicit operator PacketList(PacketElement d) => new(new List<PacketElement>() { d });
};
record PacketValue(int value) : PacketElement
{
    public static bool operator <=(PacketValue a, PacketValue b)
    {
        return a.value <= b.value;
    }
    public static bool operator >=(PacketValue a, PacketValue b)
    {
        return a.value >= b.value;
    }

}
record PacketList : PacketElement
{
    public PacketList() { }
    public PacketList(List<PacketElement> value) => this.value = value;
    public List<PacketElement> value { get; set; } = new();
    public static bool operator >=(PacketValue a, PacketList b)
    {
        return (PacketList)a >= b;
    }
    public static bool operator <=(PacketValue a, PacketList b)
    {
        return (PacketList)a <= b;
    }
    public static bool operator <=(PacketList a, PacketList b)
    {
        if (a.value.Count() > b.value.Count()) return false;
        if (a.value.Count() < b.value.Count()) return true;
        for (int i = 0; i < a.value.Count(); i++)
        {
            var aVal = a.value[i];
            var bVal = b.value[i];
            if (a.value[i] <= b.value[i]) continue;
            return false;

        }
        return true;
    }
    public static bool operator >=(PacketList a, PacketList b)
    {
        if (a.value.Count() < b.value.Count()) return false;
        if (a.value.Count() > b.value.Count()) return true;
        for (int i = 0; i < a.value.Count(); i++)
        {
            if (a.value[i] >= b.value[i]) continue;
            return false;
        }
        return true;
    }
    // public static explicit operator PacketList(PacketValue d) => new(new List<PacketElement>() { d });
    public static implicit operator PacketList(PacketValue d) => new(new List<PacketElement>() { d });



}

record Packet(PacketList content);
record PacketPair(Packet left, Packet right);
