using System.Security.Cryptography.X509Certificates;
using System.Collections.Immutable;
using System.Linq;
var filePath = args.Count() > 0 && args[0] == "prod" ? "./input.txt" : "./test_input.txt";
var fileContent = File.ReadAllLines(filePath);
var terrainMap = new TerrainMap(ParseInput(fileContent));
var startCharacter = 'S';
var endCharacter = 'E';
// var edges = ScanMap(terrainMap);

//BFS
Dictionary<Coordinates, int> PathToDest = new();
Queue<Coordinates> queToCheck = new();
var myGoal = terrainMap.gridCells.Single(e => e.Value.letter == endCharacter);
myGoal.Value.PathToDest = 0;
myGoal.Value.Visited = true;
queToCheck.Enqueue(myGoal.Key);
GridCell resultCell = null;
while (queToCheck.Count > 0 && resultCell is null)
{
    var currCords = queToCheck.Dequeue();
    var currCell = terrainMap.gridCells[currCords];
    var pathTodest = currCell.PathToDest + 1;
    var neighbourCords = terrainMap.GetPossibleCells(currCords);
    foreach (var cord in neighbourCords)
    {
        var nextCell = terrainMap.gridCells[cord];

        if (nextCell.Visited) continue;
        nextCell.Visited = true;
        nextCell.PathToDest = pathTodest;
        if (nextCell.letter == 'a') resultCell = nextCell;
        queToCheck.Enqueue(cord);
    }
}
Console.WriteLine(resultCell);
// Console.WriteLine(terrainMap.gridCells.Single(x => x.Value.letter == startCharacter).Value);

Dictionary<Coordinates, GridCell> ParseInput(string[] input)
{

    return (
    from irow in Enumerable.Range(0, input.Length)
    from icol in Enumerable.Range(0, input[0].Length)
    select
    new KeyValuePair<Coordinates, GridCell>(
    new Coordinates(irow, icol), new GridCell(input[irow][icol])
    )).ToDictionary(x => x.Key, x => x.Value);
    // return input.Select(line => line.ToCharArray().Select(currentChar => new GridCell(currentChar)).ToArray()).ToArray();
}
record TerrainMap(Dictionary<Coordinates, GridCell> gridCells)
{
    private List<Direction> directions = new(){
        new Direction(0,1,"Right"),
        new Direction(0,-1,"Left"),
        new Direction(1,0,"Up"),
        new Direction(-1,0,"Down"),
    };

    // GridCell GetCell(int irow, int icol) => gridCells[new Coordinates(irow, icol)];
    GridCell? GetCell(Coordinates cord) => CellExists(cord) ? gridCells[cord] : null;
    public List<Coordinates> GetPossibleCells(Coordinates cords)
    {
        var currentCell = GetCell(cords);
        List<Coordinates> result = new();
        int icol = cords.icol;
        int irow = cords.irow;
        foreach (var direction in directions)
        {
            var newCords = cords + direction;
            if (!CellExists(newCords)) continue;
            var nextCell = GetCell(newCords);
            var heightDiff = currentCell.height - nextCell.height;
            if (heightDiff > 1) continue;
            result.Add(newCords);
        }
        return result;
    }
    bool CellExists(Coordinates cords)
    {
        return gridCells.ContainsKey(cords);
    }
    public void PrintMap()
    {
        var maxRows = gridCells.Keys.Max(x => x.irow);
        var maxCols = gridCells.Keys.Max(x => x.icol);
        for (int i = 0; i <= maxRows; i++)
        {
            var line = "";
            for (int j = 0; j <= maxCols; j++)
            {
                var cords = new Coordinates(i, j);
                var gc = gridCells[cords];
                line += $"[{gc.letter}:{gc.height:000}:{gc.PathToDest:00}]\t";
            }
            Console.WriteLine(line);
        }
    }

}
record GridCell(char letter)
{
    public int height
    {
        get
        {
            if (letter == 'S') return (int)'a';
            if (letter == 'E') return (int)'z';
            return (int)letter;
        }
    }
    public int PathToDest { get; set; } = 999;
    public bool Visited { get; set; } = false;
}
record Coordinates(int irow, int icol) : IComparable<Coordinates>
{
    public int CompareTo(Coordinates? other)
    {
        return irow > other.irow && icol > other.icol ? 1 : 0;
    }

    public static Coordinates operator +(Coordinates cords, Direction direction) =>
                            new Coordinates(cords.irow + direction.drow, cords.icol + direction.dcol);
}
record Direction(int drow, int dcol, string name);
