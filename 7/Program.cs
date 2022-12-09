using System.ComponentModel;
using System;
using System.IO;
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
Directory root = new Directory("/");
Directory currentDirectory = root;
int totalSum = 0;
while ((line = reader.ReadLine()) != null)
{
    Console.WriteLine($"Line: {line}");
    if (line == "$ ls")
    {
        Console.WriteLine("ignored");
        continue;
    }
    else
    if (line.StartsWith("$ cd"))
    {
        Console.WriteLine("Its move line");
        ExecuteMove(line);
    }
    else if (!line.StartsWith("$"))
    {
        AddChild(line);
    }
}
var currentTotalUsed = root.Size;
var neededSpace = 30000000;
var totalSpace = 70000000;
var spaceToBeFreed = neededSpace - (totalSpace - currentTotalUsed);
List<int> allDirsTofix = new();
TraverseResults(root);

Console.WriteLine(allDirsTofix.OrderBy(d => d).First());

void TraverseResults(Directory dir)
{
    int currDirSize = dir.Size;
    if (currDirSize >= spaceToBeFreed) allDirsTofix.Add(currDirSize);
    dir.ItemList.Where(i => i is Directory).ToList().ForEach(d => TraverseResults(d as Directory));
}


void AddChild(string line)
{
    Item newItem;
    string itemName = line.Split(' ')[1];
    if (line.StartsWith("dir"))
    {
        Console.WriteLine("adding directory " + itemName);
        newItem = new Directory(itemName, currentDirectory);

    }
    else
    {

        var itemSize = Convert.ToInt32(line.Split(' ')[0]);
        Console.WriteLine("adding file " + itemName + " size: " + itemSize);

        newItem = new File(itemName, itemSize);
    }
    currentDirectory.ItemList.Add(newItem);
}
void ExecuteMove(string Line)
{
    string param = Line.Split(" ").Last();
    Directory? newDirectory;
    if (param == "..")
    {
        newDirectory = currentDirectory.ParentDirectory;
    }
    else if (param == "/")
    {
        return;
    }
    else
    {
        newDirectory = (Directory)(currentDirectory.ItemList.Where(item => item.Name == param && item is Directory).FirstOrDefault());
    }
    if (newDirectory == null) throw new Exception("There is no parent.");
    currentDirectory = newDirectory;
}
// var it = new FileInfo("Asd").len;



abstract class Item
{
    public string Name { get; set; }
    public Directory ParentDirectory { get; set; }
    public abstract int Size { get; }
}
class File : Item
{
    public File(string FileName, int Size)
    {
        this._size = Size;
    }
    private int _size;
    public override int Size { get { return this._size; } }
    // public int Size { set; }
}
class Directory : Item
{
    public Directory(string Name)
    {
        this.Name = Name;
    }
    public Directory(string Name, Directory parent)
    {
        this.Name = Name;
        this.ParentDirectory = parent;
    }
    public List<Item> ItemList { get; private set; } = new();
    public override int Size
    {
        get
        {
            int sum = 0;
            this.ItemList.ForEach(i => sum += i.Size);
            return sum;
        }
    }
}