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
int totalDepth = 0;
int totalHorizontal = 0;
int aim = 0;
while ((line = reader.ReadLine()) != null)
{
    Console.WriteLine($"Line: {line}");
    var commandArray = line.Split(' ');
    var direction = commandArray[0];
    var value = Convert.ToInt32(commandArray[1]);
    switch (direction)
    {
        case "forward":
            totalHorizontal += value;
            totalDepth += (aim * value);
            break;
        case "up":
            // totalDepth -= value;
            aim -= value;
            break;
        case "down":
            // totalDepth += value;
            aim += value;
            break;
    }
}

Console.WriteLine(totalDepth * totalHorizontal);