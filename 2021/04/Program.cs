using System.Diagnostics;
// See https://aka.ms/new-console-template for more information
var filePath = "./test_input.txt";
var prodFilePath = "./input.txt";
var finalCheck = false;
// finalCheck = true;


if (finalCheck)
{
    filePath = prodFilePath;
}
var input = File.ReadAllText(filePath);

var inputParted = input.Split("\n\n");
var drawnNumbers = inputParted.First().Split(',').Select(n => Convert.ToInt32(n)).ToArray();
var boards = ParseInput(inputParted.Skip(1).ToArray());
Console.Write("asd");
Board[] ParseInput(string[] input)
{
    return input.Select(boardSection =>
                            boardSection.Split('\n')
                                .Select(line => line.Split(' ').Select(num => new BingoItem(num)).ToArray()
                    ).ToArray())
                    .Select(partial => new Board(partial)).ToArray();
}

record Board(BingoItem[][] items, bool won = false)
{
    private int boardSize = 5;
    public void check(string number)
    {
        this.items.Forea
    }
}
[DebuggerDisplay("{display()}")]
record BingoItem(string number, bool isChecked = false)
{
    public string display() => this.isChecked ? $"[{this.number}]" : $" {this.number} ";
}