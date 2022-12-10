// See https://aka.ms/new-console-template for more information
var filePath = "./test_input.txt";
var prodFilePath = "./input.txt";
bool finalCheck = false;
finalCheck=true;


if (finalCheck)
{
    filePath = prodFilePath;
}
var reader = new System.IO.StreamReader(filePath);
string line = "";
while ((line = reader.ReadLine()) != null)
{
    Console.WriteLine($"Line: {line}");


}