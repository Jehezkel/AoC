using System.Runtime.InteropServices;
// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Text.RegularExpressions;

var filePath = "./test_input.txt";
var prodFilePath = "./input.txt";
bool finalCheck = false;
finalCheck = true;

List<Queue<Char>> AllCratesQueues = new List<Queue<char>>();
List<Stack<Char>> AllCratesStacks = new List<Stack<char>>();

if (finalCheck)
{
    filePath = prodFilePath;
}
var reader = new System.IO.StreamReader(filePath);
string line = "";
// bool
while ((line = reader.ReadLine()) != null)
{
    Console.WriteLine($"Line: {line}");
    if (!line.StartsWith("move") && !line.StartsWith(" 1"))
    {
        setupCrateStacks(line);
    }
    else if (line.StartsWith(" 1"))
    {
        ConvertQueuesToStacks();
    }
    else if (line.StartsWith("move"))
    {
        moveLineParse(line);
    }

    // drawStacks();
}
var result = "";
Console.WriteLine("xD");
AllCratesStacks.ForEach(s => result += s.Pop());
Console.WriteLine(result);



void setupCrateStacks(string line)
{
    // lineLen = ColWidth x numberOfColumns + numberOfCols -1

    var numberOfColumns = (line.Length + 1) / 4;
    while (AllCratesQueues.Count < numberOfColumns)
    {
        AllCratesQueues.Add(new());
        Console.WriteLine($"adding crate stack place.. [{AllCratesQueues.Count}/{numberOfColumns}]");
    }
    for (int i = 0; i < numberOfColumns; i++)
    {
        int startIndex = i * 4 + 1;
        var currChar = line.Substring(startIndex, 1);
        if (!String.IsNullOrWhiteSpace(currChar.Trim()))
        {
            AllCratesQueues[i].Enqueue(Convert.ToChar(currChar));
        }
    }
}
void moveLineParse(string line)
{
    var regex = new Regex(@" \d+");
    var results = regex.Matches(line);
    int qty = Convert.ToInt32(results[0].Value);
    int from = Convert.ToInt32(results[1].Value) - 1;
    int to = Convert.ToInt32(results[2].Value) - 1;
    List<Char> movingList = new();
    for (int i = 0; i < qty; i++)
    {
        var crate = AllCratesStacks[from].Pop();
        movingList.Add(crate);
    }
    movingList.Reverse();
    movingList.ForEach(c => AllCratesStacks[to].Push(c));

}
void ConvertQueuesToStacks()
{
    foreach (var crateQueue in AllCratesQueues)
    {
        var crateStack = new Stack<Char>();
        var tempList = new List<Char>();
        foreach (var crate in crateQueue)
        {

            // Console.WriteLine(crate);
            tempList.Add(crate);

        }
        tempList.Reverse();
        tempList.ForEach(c => crateStack.Push(c));
        AllCratesStacks.Add(crateStack);
    }
}
void drawStacks()
{
    var maxHeight = 0;
    foreach (var stack in AllCratesQueues)
    {
        maxHeight = maxHeight > stack.Count ? maxHeight : stack.Count;
    }
    for (int i = 0; i < maxHeight; i++)
    {
        string cratesHorizontalLine = "";
        for (int j = 0; j < AllCratesQueues.Count; j++)
        {
            var stack = AllCratesQueues[j];
            if (stack.Count >= maxHeight - i)
            {
                int offset = maxHeight - stack.Count;
                char currentLetter = stack.Skip(i - offset).First();
                cratesHorizontalLine += $"[{currentLetter}] ";
            }
            else
            {
                cratesHorizontalLine += "    ";
            }
        }
        Console.WriteLine(cratesHorizontalLine);
    }
    var numbers = " ";
    Enumerable.Range(1, AllCratesQueues.Count).ToList().ForEach(i => numbers += $"{i}   ");
    Console.WriteLine(numbers);

}


