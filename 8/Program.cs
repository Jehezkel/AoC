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
List<int[]> TreeArea = new();
int numOfRows = 0;
int numOfColumns = 0;
int currentMax = 0;
int visibleGridTrees = 0;
while ((line = reader.ReadLine()) != null)
{
    numOfRows++;
    if (numOfColumns == 0)
    {
        numOfColumns = line.Length;
    }
    // Console.WriteLine($"Line: {line}");
    var currentArray = line.ToList().Select(s => Convert.ToInt32(s.ToString())).ToArray();
    TreeArea.Add(currentArray);
}
for (int i = 1; i < TreeArea.Count() - 1; i++)
{
    var row = TreeArea[i];
    for (int j = 1; j < row.Length - 1; j++)
    {
        int currentVal = checkAxis(i, j);
        if (currentVal > currentMax) currentMax = currentVal;
    }
}


int checkAxis(int row, int column)
{

    var TreeToCheck = TreeArea[row][column];
    int treesTop = countTrees(TreeToCheck, TreeArea.Take(row).Select(row => row[column]).Reverse());
    int treesDown = countTrees(TreeToCheck, TreeArea.Skip(row + 1).Select(row => row[column]));
    int treesLeft = countTrees(TreeToCheck, TreeArea.Skip(row).First().ToList().Take(column).Reverse());
    int treesRight = countTrees(TreeToCheck, TreeArea.Skip(row).First().ToList().Skip(column + 1));

    return treesTop * treesDown * treesLeft * treesRight;
}
int countTrees(int currentTree, IEnumerable<int> trees)
{
    var result = 0;
    foreach (var tree in trees)
    {
        result++;
        if (tree >= currentTree) return result;
    }
    return result;
}
Console.WriteLine("Visible grid trees " + visibleGridTrees);
Console.WriteLine("rows " + numOfRows);
Console.WriteLine("Cols " + numOfColumns);
Console.WriteLine("AreaTrees" + (numOfRows * 2 + (numOfColumns - 2) * 2));
int result = numOfRows * 2 + (numOfColumns - 2) * 2 + visibleGridTrees;

Console.WriteLine("score:" + currentMax);



void parseArea()
{
    var upperRow = TreeArea.First();
    var lowerRow = TreeArea.Last();
    var currentRow = TreeArea.Skip(1).Take(1).First();
    int currRowVisTrees = 0;
    Console.WriteLine($"Line {numOfRows - 1} -:\n{upperRow}\n{currentRow}\n{lowerRow}");
    for (int i = 1; i < currentRow.Length - 1; i++)
    {
        List<int> treeToCompare = new();
        var leftTree = currentRow[i - 1];
        treeToCompare.Add(leftTree);
        var rightTree = currentRow[i + 1];
        treeToCompare.Add(rightTree);
        var upperTree = upperRow[i];
        treeToCompare.Add(upperTree);
        var lowerTree = lowerRow[i];
        treeToCompare.Add(lowerTree);
        int currentTree = currentRow[i];
        foreach (int Tree in treeToCompare)
        {
            if (currentTree > Tree)
            {
                currRowVisTrees++;
                Console.WriteLine(currentTree + " is visible (greter than " + Tree + ")");
                break;
            }
        }
    }
    visibleGridTrees += currRowVisTrees;
}