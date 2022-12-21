using System.Runtime.CompilerServices;
using System.Globalization;
using System.Text.RegularExpressions;
var filePath = "./input.txt";
var fileContent = File.ReadAllLines(filePath);
var maxXanY = 4000000;

var SensorDataInfo = ParseInput(fileContent).ToList();
var listOfallItems = new Dictionary<int, List<XRange>>();
//Sensor data to Dictonary of deadzone ranges.
SensorDataInfo.ForEach(l =>
    {
        l.ListAllFromDeadZone().ToList().ForEach(p =>
            {
                if (listOfallItems.ContainsKey(p.Key))
                {
                    listOfallItems[p.Key].Add(p.Value);

                }
                else
                {
                    listOfallItems[p.Key] = new() { p.Value };
                }
            });
        listOfallItems[l.beacon.y].Add(new XRange(l.beacon.x, l.beacon.x));
        listOfallItems[l.sensor.y].Add(new XRange(l.sensor.x, l.sensor.x));
    }
);

var searchedCords = new Coordinate(-1, -1);

foreach (var irow in listOfallItems.Where(i => i.Key <= maxXanY && i.Key >= 0))
{
    var mergedRange = irow.Value.OrderBy(irange => irange.start).First();
    irow.Value.OrderBy(irange => irange.start).ToList().ForEach(irange =>
    {
        if (mergedRange.ContainsOneEndOf(irange) || irange.ContainsOneEndOf(mergedRange)) mergedRange += irange;
        else
        {
            searchedCords = new Coordinate(mergedRange.start + 1, irow.Key);
        }
    });
    if (searchedCords.x != -1) break;
    mergedRange = new XRange(
       Math.Max(0, mergedRange.start), Math.Min(maxXanY, mergedRange.end)

    );
}


long result = (long)4000000 * searchedCords.x + searchedCords.y;
Console.WriteLine("result: " + result);














IEnumerable<SensorBeaconPair> ParseInput(string[] input)
{
    var regex = new Regex(@"-*\d+");
    return input
    .Select(l => regex.Matches(l))
    .Select(m => new SensorBeaconPair(
        new Coordinate(Convert.ToInt32(m[0].Value), Convert.ToInt32(m[1].Value)),
        new Coordinate(Convert.ToInt32(m[2].Value), Convert.ToInt32(m[3].Value))
    ));

}

record Coordinate(int x, int y);
record XRange(int start, int end)
{
    public static XRange operator +(XRange left, XRange right)
    {
        return new XRange(
            left.start <= right.start ? left.start : right.start,
            left.end >= right.end ? left.end : right.end
            );
    }
    public bool ContainsOneEndOf(XRange other)
    {
        return (this.start - 1 <= other.start && this.end + 1 >= other.start) ||
        (this.start - 1 <= other.end && this.end + 1 >= other.end);
    }

}

record SensorBeaconPair(Coordinate sensor, Coordinate beacon)
{
    public int Distance
    {
        get
        {
            return Math.Abs(this.sensor.x - this.beacon.x) +
            Math.Abs(this.sensor.y - this.beacon.y);
        }
    }
    public IEnumerable<KeyValuePair<int, XRange>> ListAllFromDeadZone()
    {
        return from iy in Enumerable.Range(this.sensor.y - Distance, 2 * Distance + 1)
               select new KeyValuePair<int, XRange>(iy,
               new XRange(this.sensor.x - (Distance - Math.Abs(this.sensor.y - iy)),
               //Start + span
                          this.sensor.x - (Distance - Math.Abs(this.sensor.y - iy)) + 2 * (Distance - Math.Abs(this.sensor.y - iy))));

    }
}