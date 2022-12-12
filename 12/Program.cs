var filePath = args.Count() > 0 && args[0] == "prod" ? "./input.txt" : "./test_input.txt";
var fileContent = File.ReadAllLines(filePath);
var terrainMap = new TerrainMap(ParseInput(fileContent));
// foreach(var cell in )


void ScanMap(TerrainMap map){
    for(int irows = 0; irows<map.gridcells.Length; irows++){
        for(int icols = 0; icols< map.gridcells[0].Length; icols++){

        }
    }
}


GridCell[][] ParseInput(string[] input){
    return input.Select(line=> line.ToCharArray().Select(currentChar=> new GridCell(currentChar)).ToArray()).ToArray();
} 
record TerrainMap(GridCell[][] gridcells){
    private List<Direction> directions= new (){
        new Direction(0,1,"Right"),
        new Direction(0,-1,"Left"),
        new Direction(1,0,"Up"),
        new Direction(-1,0,"Down"),
    };
    List<GridCell> GetPossibleNeighbours(GridCell cell){
        GetNeighours(cell)
    }
    List<GridCell> GetNeighours(GridCell cell){
        var irow = cell.
        List<GridCell> result = new();
        foreach(var direction in directions){
            (int newRow, int newCol)= (irow+direction.drow,icol+direction.dcol);
            if(!CellExists(newRow,newCol)) continue;
            result.Add(gridcells[newRow][newCol]);
        }
        return result;
    }
    bool CellExists(int irow, int icol){
        return irow>=0 && icol>=0 && irow<gridcells.Length && icol< gridcells[0].Length;
    }

}
record GridCell(char letter, int irow, int icol){
    public int height { get{
        if(letter =='S') return(int)'a';
        if(letter =='E') return(int)'z';
        return (int)letter;
    } }
}
record Direction(int drow, int dcol, string name);