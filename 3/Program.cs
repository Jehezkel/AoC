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
List<char> totalErrorList = new();
int sum = 0;
while ((line = reader.ReadLine()) != null)
{
    Console.WriteLine($"Line: {line}");
    sum += GetDuplicatedItemsValue(line);
}
Console.WriteLine(sum);





int GetDuplicatedItemsValue(string line)
{
    int splitPoint = line.Length / 2;
    string firstPart = line.Substring(0, splitPoint);
    var firstCopartment = (long)0;
    firstPart.ToList().ForEach(c =>
        firstCopartment |= (long)1 << GetNumericValue(c));
    string secondPart = line.Substring(splitPoint, splitPoint);

    foreach (var letter in secondPart.ToList())
    {
        var currItemValue = GetNumericValue(letter);
        Console.WriteLine(Convert.ToString(((long)1 << currItemValue), 2));
        if ((firstCopartment & ((long)1 << currItemValue)) != 0)
        {
            Console.WriteLine($"Its a hit {letter} with value {currItemValue}");
            return currItemValue;
        }
    }
    return 0;
}
int GetNumericValue(char letter)
{
    var letterValue = (int)letter;
    int baseValue = 0;
    if (letterValue >= (int)'A' && letterValue <= (int)'Z')
    {
        baseValue = (int)'A' - 2 - ((int)'z' - (int)'a');
    }
    else
    {
        baseValue = (int)'a' - 1;
    }
    var resultValue = letterValue - baseValue;
    Console.WriteLine($"letter:\t{letter} has value:\t{resultValue}");
    return resultValue;
}