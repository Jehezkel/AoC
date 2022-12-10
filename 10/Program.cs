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

int registerX = 1;



// while ((line = reader.ReadLine()) != null)
// {
//     Console.WriteLine($"Line: {line}");


// }
var ListOfSignalStrength = new List<int>();
var QueOfOperations = new Queue<AddxOperation>();
for (int cycle = 1; cycle <= 220; cycle++)
{
    if ((cycle - 20) % 40 == 0)
    {
        var signalStrength = getSignalStrength(registerX, cycle);
        Console.WriteLine($"ImportantCycle - {cycle} - Strength: {signalStrength}");
        ListOfSignalStrength.Add(signalStrength);
    }
    if (QueOfOperations.Count > 0)
    {
        var currOperation = QueOfOperations.Dequeue();
        registerX += currOperation.vValue;
        // Console.WriteLine("Adding cu")
        continue;
    }
    var currOperationLine = reader.ReadLine();
    if (currOperationLine is null) break;
    // Console.WriteLine("Reading ops: " + currOperationLine);
    // if (currOperationLine is null) throw new Exception("shouldnt be null");
    var command = currOperationLine!.Split()[0];
    if (command == "noop") { continue; }
    else
    {
        var value = ((IConvertible)currOperationLine.Split()[1]).ToInt32(default);
        QueOfOperations.Enqueue(new AddxOperation(value));
    }

}
int sum = ListOfSignalStrength.Sum();
Console.WriteLine("final sum:" + sum);

int getSignalStrength(int xValue, int cycle)
{
    return xValue * cycle;
}
class AddxOperation
{
    public AddxOperation(int vValue)
    {
        this.vValue = vValue;
    }
    public int vValue { get; set; }
}