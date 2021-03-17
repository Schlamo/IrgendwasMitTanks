using Enumerators;
using System.Collections.Generic;

public class GameSettings
{
    public IEnumerable<PlayerConfiguration> PlayerConfigurations { get; set; }
    public int      PlayerCount { get; set; }
    public int      BorderSteps { get; set; }
    public int      NitroCount { get; set; }
    public int      CrateCount { get; set; }
    public int      RockCount { get; set; }
    public int      TreeCount { get; set; }
    public int      MapSize { get; set; }
    public int      SnowIntensity { get; set; }
    public bool     DayNightCycle { get; set; }
    public float    StartingRadius { get; set; }
    public float    SupplyRefill { get; set; }
    public MapType  MapType { get; set; }
}
