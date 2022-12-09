using System.Globalization;
// See https://aka.ms/new-console-template for more information
var filePath = "./test_input.txt";
var prodFilePath = "./input.txt";
bool finalCheck = false;
finalCheck = true;


if (finalCheck)
{
    filePath = prodFilePath;
}
var reader = new System.IO.StreamReader(filePath);
string line = "";
int inclusivePairs = 0;
while ((line = reader.ReadLine()) != null)
{
    Console.WriteLine($"Line: {line}");
    if (pairCheck(line))
        inclusivePairs++;

}
Console.WriteLine(inclusivePairs);

bool pairCheck(string elvePair)
{
    string elvOneString = elvePair.Split(",")[0];
    var elveOne = new Range(elvOneString);
    string elvTWoString = elvePair.Split(",")[1];
    var elveTwo = new Range(elvTWoString);
    return elveOne.OverLaps(elveTwo) || elveTwo.OverLaps(elveOne);
}

class Range
{
    public int Start { get; set; }
    public int End { get; set; }
    public Range(string elveRange)
    {
        this.Start = ((IConvertible)elveRange.Split("-")[0]).ToInt32(default);
        this.End = ((IConvertible)elveRange.Split("-")[1]).ToInt32(default);
    }
    public bool Contains(Range other)
    {
        return this.Start <= other.Start && this.End >= other.End;
    }
    public bool OverLaps(Range other)
    {
        return this.ContainsPoint(other.Start) || this.ContainsPoint(other.End);
    }
    public bool ContainsPoint(int point)
    {
        return point >= this.Start && point <= this.End;
    }
}