// See https://aka.ms/new-console-template for more information
var filePath = "./test_input.txt";
var prodFilePath = "./input.txt";
bool finalCheck = false;
finalCheck = true;


if (finalCheck)
{
    filePath = prodFilePath;
}
var fileContent = File.ReadAllLines(filePath);
var moveInstructions = fileContent.Select(l => l.Split()).ToList();
var headPosition = new Tuple<int, int>(0, 0);
// var tailsList = new List<Tuple<int, int>>();
var tailsList = Enumerable.Repeat(new Tuple<int, int>(0, 0), 9).ToList();
var totalList = new List<Tuple<int, int>>();
// Tuple<int, int> diff;
foreach (var moveLine in moveInstructions)
{
    var myMove = getMoveDirection(moveLine[0].First());
    var numOfMoves = Convert.ToInt32(moveLine[1]);
    Console.WriteLine($"{moveLine[0] + moveLine[1]}Moving {myMove} x {numOfMoves}");
    for (int i = 0; i < numOfMoves; i++)
    {
        headPosition = new Tuple<int, int>(headPosition.Item1 + myMove.Item1,
                                            headPosition.Item2 + myMove.Item2);
        // totalList.Add(headPosition);

        for (int j = 0; j < 9; j++)
        {

            var currTail = tailsList[j];
            Tuple<int, int> currHead;
            if (j == 0)
            {
                currHead = headPosition;
            }
            else
            {
                currHead = tailsList[j - 1];
            }
            var diff = calculateDiff(currHead, currTail);
            if (!isInContactZone(diff))
            {
                tailsList[j] = tailMove(currTail, diff);

                if (j == 8)
                {
                    totalList.Add(currTail);
                }
            }
        }

        // calculateDiff();
        // if (isInContactZone())
        // {
        //     Console.WriteLine("jest git");
        // }
        // else
        // {
        //     throw new Exception("Cos jest chujowo");
        // }
    }
}

Tuple<int, int> calculateDiff(Tuple<int, int> head, Tuple<int, int> tail)
{
    return new Tuple<int, int>(head.Item1 - tail.Item1,
             head.Item2 - tail.Item2);
}
Console.WriteLine(totalList.Distinct().Count() + 1);
Tuple<int, int> getMoveDirection(char letter)
{
    switch (letter)
    {
        case 'U':
            return new Tuple<int, int>(0, 1);
        case 'D':
            return new Tuple<int, int>(0, -1);
        case 'R':
            return new Tuple<int, int>(1, 0);
        case 'L':
            return new Tuple<int, int>(-1, 0);
        default:
            throw new Exception("you are wrong!");
    }
}
bool isInContactZone(Tuple<int, int> diff)
{
    if (diff.Item1 < -1 || diff.Item1 > 1) return false;
    if (diff.Item2 < -1 || diff.Item2 > 1) return false;
    return true;
}
Tuple<int, int> tailMove(Tuple<int, int> tail, Tuple<int, int> diff)
{
    if (diff.Item1 == 0 || diff.Item2 == 0)
    {
        Console.WriteLine("Same row or col");
        if (Math.Abs(diff.Item1) > 1)
        {
            var newItem1 = tail.Item1 + Math.Sign(diff.Item1);
            tail = new Tuple<int, int>(newItem1, tail.Item2);
            return tail;
        }
        else if (Math.Abs(diff.Item2) > 1)
        {
            var newItem2 = tail.Item2 + Math.Sign(diff.Item2);
            tail = new Tuple<int, int>(tail.Item1, newItem2);
            return tail;

        }
        throw new Exception("Bullsghit");
    }
    else
    {
        Console.WriteLine("DiffLineAnd height");
        var newItem1 = tail.Item1 + Math.Sign(diff.Item1);
        var newItem2 = tail.Item2 + Math.Sign(diff.Item2);
        tail = new Tuple<int, int>(newItem1, newItem2);
        return tail;

    }
}
// Tuple<int, int> getDiff()
// {
//     // return new Tuple<int, int>()
// }
// void ParseLine(string line)
// {
//     var lineArr = line.Split();
//     switch (lineArr[0])
//     {
//         U:
//     default:
//     }
// }