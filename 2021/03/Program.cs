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

var arrLen = listOfBoolArrays[0].Length;
uint gamma = 0;
uint sigma = 0;
string res = "";
for (int i = 0; i < arrLen; i++)
{
    var mostCommon = Math.Round(listOfBoolArrays.Select(a => Convert.ToInt32(a[i])).Average());
    var leastCommon = mostCommon == 1 ? 0 : 1;
    gamma |= (uint)mostCommon << arrLen - (i + 1);
    sigma |= (uint)leastCommon << arrLen - (i + 1);
    Console.WriteLine(Convert.ToString(gamma, 2));
    res = mostCommon + res;
}
// Console.WriteLine(gamma);
// Console.WriteLine(sigma);
Console.WriteLine(gamma * sigma);
bool[] getBoolArrayFromString(string line)
{
    var result = line.ToCharArray().Select(c => c == '1').ToArray();
    return result;
}


