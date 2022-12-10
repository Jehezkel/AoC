using System.Linq;
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

int registerX = 1;

var ListOfSignalStrength = new List<int>();
var QueOfOperations = new Queue<AddxOperation>();
var Sprite = new Sprite();
var Crt = new Crt(Sprite);
for (int cycle = 1; cycle <= 240; cycle++)
{

    if (QueOfOperations.Count > 0)
    {
        Crt.Draw(cycle);
        var currOperation = QueOfOperations.Dequeue();
        var oldValue = registerX;
        registerX += currOperation.vValue;
        Console.WriteLine($"End of cycle\t{cycle}: finish executing addx {currOperation.vValue} (Register X is now {registerX})");
        Sprite.Move(registerX);
        Crt.PrintSpritPosition();
        Console.WriteLine();
        continue;
    }
    var currOperationLine = reader.ReadLine();
    if (currOperationLine is null) break;
    var command = currOperationLine!.Split()[0];
    if (command == "noop")
    {
        Console.WriteLine($"Start cycle\t{cycle}: begin executing noop");
        Crt.Draw(cycle);
        Console.WriteLine($"End of cycle\t{cycle}: finish executing noop");
    }
    else
    {
        Console.WriteLine($"Start cycle\t{cycle}: begin executing {currOperationLine}");
        var value = ((IConvertible)currOperationLine.Split(" ")[1]).ToInt32(default);
        QueOfOperations.Enqueue(new AddxOperation(value));
        Crt.Draw(cycle);

    }
    Console.WriteLine();
}
Crt.PrintImage();



class AddxOperation
{
    public AddxOperation(int vValue)
    {
        this.vValue = vValue;
    }
    public int vValue { get; set; }
}

class Sprite
{
    public Sprite()
    {
        this._position = 1;
    }
    private int _position { get; set; }
    public void Move(int registerX)
    {
        this._position = registerX;
    }
    public int Start { get { return this._position - 1; } }
    public int End { get { return this._position + 1; } }

}

class Crt
{
    public Crt(Sprite sprite)
    {
        this.Sprite = sprite;
    }
    public bool[,] PixelsRows { get; set; } = new bool[40, 6];
    public Sprite Sprite { get; set; }
    public void Draw(int cycle)
    {
        // var cycleForArray = cycle - 1;
        var rowNumber = getRowNum(cycle);
        var rowPosition = getRowPosition(cycle);
        var toBeDrawn = isSpriteVisible(rowPosition);
        var character = GetCharacter(toBeDrawn);
        Console.WriteLine($"During cycle\t{cycle}: CRT draws pixel in position {rowPosition}: character {character}");
        this.PixelsRows[rowPosition, rowNumber] = toBeDrawn;
        this.PrintCurrentRow(rowNumber);
    }
    private int getRowNum(int cycle)
    {
        return (int)Math.Floor((cycle - 1) / 40d);
    }
    private string GetCharacter(bool toBeDrawn)
    {
        return toBeDrawn ? "██" : "⠀⠀";
        // return toBeDrawn ? "#" : " ";
    }
    private int getRowPosition(int cycle)
    {
        return ((cycle - 1) % 40);
    }
    private bool isSpriteVisible(int rowPosition)
    {
        // var rowNumOffset = rowNum * 40;
        // var valueCheck = cycle - rowNumOffset;
        if (rowPosition >= this.Sprite.Start && rowPosition <= this.Sprite.End) return true;
        return false;
    }
    public void PrintImage()
    {
        Console.WriteLine();
        var numOfRows = this.PixelsRows.GetLength(1);
        var positionInRow = this.PixelsRows.GetLength(0);
        for (int i = 0; i < numOfRows; i++)
        {
            var line = "";
            // var currentRow = this.PixelsRows[i]
            for (int j = 0; j < positionInRow; j++)
            {

                line += GetCharacter(this.PixelsRows[j, i]);
            }
            Console.WriteLine(line);
        }
        Console.WriteLine();
    }
    public void PrintCurrentRow(int rowNum)
    {
        var positionInRow = this.PixelsRows.GetLength(0);
        var line = "";
        // var currentRow = this.PixelsRows[i]
        for (int j = 0; j < positionInRow; j++)
        {

            line += GetCharacter(this.PixelsRows[j, rowNum]);
        }
        Console.WriteLine($"Current CRT row: {line}");
    }
    public void PrintSpritPosition()
    {
        var positionInRow = this.PixelsRows.GetLength(0);
        var resultLine = "";
        for (int j = 0; j < positionInRow; j++)
        {
            var currentChar = GetCharacter(j >= this.Sprite.Start && j <= this.Sprite.End);
            resultLine += currentChar;
        }
        Console.WriteLine($"Sprite position: {resultLine} {this.Sprite.Start}-{this.Sprite.End}");
    }
}