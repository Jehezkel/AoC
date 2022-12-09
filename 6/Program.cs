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
int? currentCharCode;
Queue<char> itemsQue = new();
var tries = 0;
while ((currentCharCode = reader.Read()) != -1)
{
    tries++;
    char currentChar = (char)currentCharCode;
    Console.WriteLine($"Current character: {currentChar}");
    if (itemsQue.Count > 13)
        itemsQue.Dequeue();
    if (itemsQue.Count == 13 && !itemsQue.Contains(currentChar) && itemsQue.Distinct().Count() == itemsQue.Count())
    {
        Console.WriteLine("result is " + tries);
        break;
    }
    itemsQue.Enqueue(currentChar);
}

