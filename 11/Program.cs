using System.Linq.Expressions;
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
List<string> monkeySpec = new();
List<Monkey> MonkeyList = new();
bool isInSpec = false;
while ((line = reader.ReadLine()) != null)
{
    Console.WriteLine($"Line: {line}");
    if (line.StartsWith("Monkey"))
    {
        isInSpec = true;
        continue;
    }
    if (!String.IsNullOrEmpty(line))
    {
        monkeySpec.Add(line);
    }
    if (isInSpec && (String.IsNullOrEmpty(line) || reader.EndOfStream))
    {
        var monkey = new Monkey(monkeySpec);
        MonkeyList.Add(monkey);
        monkeySpec = new List<string>();
    }

}
int mostCommonDivisor = 1;
MonkeyList.ForEach(m => mostCommonDivisor *= m.TestDivisor);
MonkeyList.ForEach(m => m.MostCommonDivisor = mostCommonDivisor);

for (int roundsIterator = 1; roundsIterator <= 10000; roundsIterator++)
{
    for (int monkeyIterator = 0; monkeyIterator < MonkeyList.Count(); monkeyIterator++)
    {
        Console.WriteLine($"Monkey {monkeyIterator}:");
        MonkeyList[monkeyIterator].TakeTurn(MonkeyList);
    }
    Console.WriteLine($"After round {roundsIterator}, the monkeys are holding items with these worry levels:");
    printRound();

}
var topMonkey = MonkeyList.OrderByDescending(m => m.InspectedItemsCount).First().InspectedItemsCount;
var secondOne = MonkeyList.OrderByDescending(m => m.InspectedItemsCount).Skip(1).First().InspectedItemsCount;
Console.WriteLine("DONE: " + (topMonkey * secondOne));

void printRound()
{
    for (int i = 0; i < MonkeyList.Count; i++)
    {
        var currMonkey = MonkeyList[i];
        Console.WriteLine($"Monkey {i}: {String.Join(", ", currMonkey.Items)}");
    }
}
class Monkey
{
    public int MostCommonDivisor { get; set; }
    public Queue<long> Items { get; set; } = new Queue<long>();
    public int TestDivisor { get; set; }
    public int TrueMonkey { get; set; }
    public int FalseMonkey { get; set; }
    public long InspectedItemsCount { get; set; } = 0;
    public Func<long, long> InspectionWorryFormula { get; set; }
    private long CalculateWorryAfterInspection(long MyWorryLevel)
    {
        Console.WriteLine($"Monkey inspects an item with a worry level of {MyWorryLevel}.");
        var worryLevelAfterInspection = checked(this.InspectionWorryFormula(MyWorryLevel));
        Console.WriteLine($"Worry level changed to {worryLevelAfterInspection}.");
        worryLevelAfterInspection = worryLevelAfterInspection % this.MostCommonDivisor;
        this.InspectedItemsCount++;

        return worryLevelAfterInspection;
    }
    private long CalulateBored(long InputWorryLevel)
    {
        var newWorryLevel = (InputWorryLevel / 3);
        Console.WriteLine($"Monkey gets bored with item. Worry level is divided by 3 to {newWorryLevel}");
        return newWorryLevel;
    }
    public Monkey(List<string> input)
    {
        foreach (var line in input)
        {
            var lineArray = line.Trim().Split(':');
            var property = lineArray[0].Trim();
            var value = lineArray[1].Trim();
            if (property.Contains("Starting items")) SetItems(value);
            if (property.Contains("Operation")) SetOperation(value);
            if (property.Contains("Test")) SetDivisor(value);
            if (property.Contains("If true")) SetMonkeyTargets(value, true);
            if (property.Contains("If false")) SetMonkeyTargets(value, false);

        }
    }
    private void SetItems(string line)
    {
        line.Split(',').ToList().ForEach(c => this.Items.Enqueue(Convert.ToInt64(c)));
    }
    private void SetOperation(string line)
    {

        var paramName = "old";
        var expressionArray = line.Split('=')[1].Trim().Split(' ');
        var InputParameter = Expression.Parameter(typeof(long), paramName);
        Expression a;
        Expression b;
        Expression operation;
        if (expressionArray[0] == paramName)
        {
            a = InputParameter;
        }
        else
        {
            a = Expression.Constant(Convert.ToInt64(expressionArray[0]));
        }
        if (expressionArray[2] == paramName)
        {
            b = InputParameter;
        }
        else
        {
            b = Expression.Constant(Convert.ToInt64(expressionArray[2]));

        }
        if (expressionArray[1] == "+")
        {
            operation = Expression.Add(a, b);
        }
        else
        {
            operation = Expression.Multiply(a, b);
        }
        var lambda = Expression.Lambda<Func<long, long>>(operation, false, InputParameter);
        this.InspectionWorryFormula = lambda.Compile();
    }
    private void SetDivisor(string line)
    {
        this.TestDivisor = Convert.ToInt32(line.Split(' ').Last());
    }
    private void SetMonkeyTargets(string line, bool result)
    {
        var value = Convert.ToInt32(line.Split(' ').Last());
        if (result)
        {
            this.TrueMonkey = value;
        }
        else
        {
            this.FalseMonkey = value;
        }
    }
    public void TakeTurn(List<Monkey> MonkeyList)
    {
        while (this.Items.Count > 0)
        {
            var currItem = Items.Dequeue();
            Console.WriteLine($"Monkey inspects an item with a worry level of {currItem}");
            var worryAfterCalculation = this.CalculateWorryAfterInspection(currItem);
            var monkeyTarget = this.GetThrowTarget(worryAfterCalculation);
            Console.WriteLine($"Item with worry level {worryAfterCalculation} is thrown to monkey {monkeyTarget}.");
            MonkeyList[monkeyTarget].Items.Enqueue(worryAfterCalculation);
        }
    }
    private int GetThrowTarget(long ItemWorryLevel)
    {
        if (ItemWorryLevel % (long)this.TestDivisor == 0)
        {
            Console.WriteLine($"Current worry level is divisible by {this.TestDivisor}.");
            return this.TrueMonkey;
        }
        Console.WriteLine($"Current worry level is not divisible by {this.TestDivisor}.");
        return this.FalseMonkey;
    }
}