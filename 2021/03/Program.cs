using System.Reflection;
using System.Text;
// See https://aka.ms/new-console-template for more information
var filePath = "./test_input.txt";
var prodFilePath = "./input.txt";
bool finalCheck = false;
finalCheck = true;


if (finalCheck)
{
    filePath = prodFilePath;
}


var fileContent = File.ReadAllLines(filePath);
var listOfBoolArrays = fileContent.ToList().Select(line => getBoolArrayFromString(line)).ToList();
var mostCommonCollection = listOfBoolArrays;
var leastCommonCollection = listOfBoolArrays;
var arrLen = listOfBoolArrays[0].Length;
int oxygenRating = 0;
int co2Rating = 0;
string res = "";
for (int i = 0; i < arrLen; i++)
{
    if (mostCommonCollection.Count > 1)
    {
        var mostCommonOxy = mostCommonCollection.Select(a => Convert.ToInt32(a[i])).Average() >= 0.5 ? 1 : 0;
        mostCommonCollection = mostCommonCollection.Where(c => c[i] == (mostCommonOxy == 1)).ToList();

    }
    if (leastCommonCollection.Count > 1)
    {
        var leastCommonC2O = leastCommonCollection.Select(a => Convert.ToInt32(a[i])).Average() >= 0.5 ? 0 : 1;
        leastCommonCollection = leastCommonCollection.Where(c => c[i] == (leastCommonC2O == 1)).ToList();
    }
    if (mostCommonCollection.Count == 1 && leastCommonCollection.Count == 1) break;
}
oxygenRating = intFromBoolArray(mostCommonCollection.First());
co2Rating = intFromBoolArray(leastCommonCollection.First());
Console.WriteLine(oxygenRating * co2Rating);

bool[] getBoolArrayFromString(string line)
{
    var result = line.ToCharArray().Select(c => c == '1').ToArray();
    return result;
}
int intFromBoolArray(bool[] bArray)
{
    return Convert.ToInt32(String.Concat(bArray.Select(x => x ? '1' : '0')), 2);
}


