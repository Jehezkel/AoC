using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;

var fileContent = File.ReadAllLines("./test_input.txt");
var valvesDictionary = ParseInput(fileContent);
var totalMinutes = 30;
Console.WriteLine();
var startingPoint = valvesDictionary["AA"];

// 30 - path to element* flow rate 
var currentDirectory = startingPoint;
var totalSum = 0;

var nextDestinationSet = false;
var mostOptimalNextTarget = startingPoint;
var moveDirections = new Queue<Valve>();
startingPoint.visited = true;
for (int i = 1; i <= totalMinutes; i++)
{
    Console.WriteLine($"\n== Minute {i} ==");
    if (!nextDestinationSet)
    {
        mostOptimalNextTarget = mostOptimalNode(currentDirectory, totalMinutes - (i + 1));
        moveDirections = getShortestPathToNode(currentDirectory, mostOptimalNextTarget);
        nextDestinationSet = true;
    }
    if (currentDirectory == mostOptimalNextTarget && nextDestinationSet)
    {
        Console.WriteLine($"You open valve {currentDirectory.name}.");
        currentDirectory.isOpen = true;
        totalSum += (totalMinutes - (i + 1)) * currentDirectory.flowRate;
        nextDestinationSet = false;
        continue;
    }
    else
    {
        var next = moveDirections.Dequeue();
        Console.WriteLine($"You move to valve {next.name}");
        currentDirectory = next;
    }
}
Console.WriteLine(totalSum);
Valve getNextTarget(Valve currentDir)
{
    return currentDir.neighbours.Select(v => valvesDictionary[v]).Where(v => !v.isOpen).OrderByDescending(v => v.flowRate).First();
}

Dictionary<string, Valve> ParseInput(string[] input)
{
    var result = new Dictionary<string, Valve>();
    var regex = new Regex(@"Valve (?<source>[A-Z]{2}).*rate=(?<rate>\d+);.*valves* (?<dest>.*)");
    foreach (var line in fileContent)
    {
        var matches = regex.Match(line);
        var src = matches.Groups["source"].Value;
        var rate = Convert.ToInt32(matches.Groups["rate"].Value);
        var destList = matches.Groups["dest"].Value.Split(',').ToList().Select(v => v.Trim()).ToArray();
        result.Add(src, new Valve(src, rate, destList));
    }
    return result;
}
Valve mostOptimalNode(Valve currentDir, int minsLeft)
{
    scanMap(currentDir, minsLeft);
    return valvesDictionary.Values.OrderByDescending(v => v.returnValue).First();

}
void scanMap(Valve startingPoint, int minutesLeft)
{
    Queue<Valve> itemsToCheck = new Queue<Valve>();
    itemsToCheck.Enqueue(startingPoint);
    var path = 0;
    while (itemsToCheck.Count > 0 && minutesLeft > path)
    {
        path++;
        var currentItem = itemsToCheck.Dequeue();
        currentItem.visited = true;
        currentItem.neighbours.Select(n => valvesDictionary[n])
            .Where(v => !v.visited && !v.isOpen).ToList().ForEach(v =>
            {
                v.shortestPath = path;
                v.returnValue = (minutesLeft - path) * v.flowRate;

                itemsToCheck.Enqueue(v);
            });
    }
}
Queue<Valve> getShortestPathToNode(Valve startingDir, Valve dest)
{
    Queue<Valve> itemsToCheck = new Queue<Valve>();
    var dictionaryCopy = valvesDictionary;
    dictionaryCopy.Values.ToList().ForEach(e => e.visited = false);
    itemsToCheck.Enqueue(dest);
    var path = 0;
    while (itemsToCheck.Count > 0)
    {
        path++;
        var currentItem = itemsToCheck.Dequeue();
        if (currentItem == startingDir) break;
        currentItem.visited = true;
        currentItem.neighbours.Select(n => dictionaryCopy[n])
            .Where(v => !v.visited).ToList().ForEach(v =>
            {
                v.shortestPath = path;
                itemsToCheck.Enqueue(v);
            });
    }
    return reconstructPath(dictionaryCopy, startingDir, dest);
}
Queue<Valve> reconstructPath(Dictionary<string, Valve> copyOfdic, Valve start, Valve end)
{
    var result = new Queue<Valve>();
    var current = start;
    while (current != end)
    {
        result.Enqueue(current);
        current = current.neighbours.Select(n => copyOfdic[n]).OrderBy(p => p.shortestPath).First();
    }
    result.Enqueue(current);
    result.Dequeue();
    return result;
}

record Valve(string name, int flowRate, string[] neighbours)
{
    public bool isOpen { get; set; } = false;
    public int shortestPath { get; set; } = -999;
    public bool visited { get; set; } = false;
    public int returnValue { get; set; }
}