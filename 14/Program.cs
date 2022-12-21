using System.Net;
var fileContent = File.ReadAllText("./input.txt");
var LineList = ParseInput(fileContent);
var filledSpace = LineList.Select(l => l.ToFullListOfPoints()).Aggregate(new HashSet<Coordinates>(), (l, c) => { l.UnionWith(c); return l; });
Console.WriteLine(LineList);
int sandUnits = 1;
int abyssLevel = filledSpace.Max(x => x.depth);
int infiniteFloorLevel = filledSpace.Max(x => x.depth + 1);
Coordinates? currentSand = Newsand();
var sandStartingPoint = Newsand();
while (true)
{
    var oldSand = currentSand;
    currentSand = SandMove(currentSand);
    if (currentSand == sandStartingPoint) break;
    if (oldSand == currentSand)
    {
        filledSpace.Add(currentSand);
        currentSand = Newsand();
        sandUnits++;
    }
}
Console.WriteLine($"abyss reached after {sandUnits}");
List<Line> ParseInput(string fileContent)
{
    var lines = fileContent.Split("\n");
    List<Line> result = new List<Line>();
    foreach (var line in lines)
    {
        var cLine = line.Split("->")
        .Select(p => new Coordinates(p))
        .Aggregate(new Line(), (l, c) => { l.Points.Add(c); return l; });
        result.Add(cLine);
    }
    return result;
}
Coordinates SandMove(Coordinates sandCords)
{
    List<Coordinates> PossibleMoves = new(){
        new Coordinates(1,0),
        new Coordinates(1,-1),
        new Coordinates(1,1)
};
    if (sandCords.depth == infiniteFloorLevel) return sandCords;
    foreach (var move in PossibleMoves)
    {
        if (filledSpace.Contains(sandCords + move)) continue;
        sandCords += move;
        break;
    }
    return sandCords;
}
Coordinates Newsand() => new Coordinates(0, 500);
// class Sand
// {

// }

record Coordinates(int depth, int xpos)
{
    public Coordinates() : this(0, 0) { }
    public Coordinates(string line) : this()
    {
        var lineSplit = line.Split(',');
        depth = Convert.ToInt32(lineSplit[1]);
        xpos = Convert.ToInt32(lineSplit[0]);
    }
    public static bool operator >(Coordinates left, Coordinates right) => left.xpos > right.xpos || left.depth > right.depth ? true : false;
    public static bool operator <(Coordinates left, Coordinates right) => left.xpos < right.xpos || left.depth < right.depth ? true : false;
    public static Coordinates operator +(Coordinates left, Coordinates right) => new Coordinates(left.depth + right.depth, left.xpos + right.xpos);
    // public static bool operator ==(Coordinates left, Coordinates right) => left.depth == right.depth && left.xpos == right.xpos;
}
record Line
{
    public List<Coordinates> Points { get; set; } = new();
    public HashSet<Coordinates> ToFullListOfPoints()
    {
        HashSet<Coordinates> result = new();
        for (int i = 1; i < Points.Count(); i++)
        {
            var lineEnd = Points[i] > Points[i - 1] ? Points[i] : Points[i - 1];
            var lineStart = Points[i] < Points[i - 1] ? Points[i] : Points[i - 1];
            var cPairPoints = from irow in Enumerable.Range(lineStart.depth, lineEnd.depth - lineStart.depth + 1)
                              from icol in Enumerable.Range(lineStart.xpos, lineEnd.xpos - lineStart.xpos + 1)
                              select new Coordinates(irow, icol);
            result.UnionWith(cPairPoints);
        }

        return result;
    }
}