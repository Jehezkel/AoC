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
bool first = true;
int prevSum = 0;
int total = 0;
Queue<int> triplets = new();
while ((line = reader.ReadLine()) != null)
{
    Console.WriteLine($"Line: {line}");
    var val = Convert.ToInt32(line);
    triplets.Enqueue(val);
    if (triplets.Count() < 3)
    {
        continue;
    }
    if (triplets.Count() > 3) triplets.Dequeue();
    if (first)
    {
        prevSum = triplets.Sum();
        first = false;
        continue;
    }
    if (triplets.Sum() > prevSum)
    {
        total++;
    }
    prevSum = triplets.Sum();

}
Console.WriteLine(total);