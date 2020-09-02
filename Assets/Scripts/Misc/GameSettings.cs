using Enumerators;

public class GameSettings
{
    public int      PlayerCount { get; set; }
    public int      BorderSteps { get; set; }
    public int      NitroCount { get; set; }
    public int      CrateCount { get; set; }
    public int      Rocks { get; set; }
    public int      Trees { get; set; }
    public int      MapSize { get; set; }
    public int      SnowIntensity { get; set; }
    public bool     UseKeyboard { get; set; }
    public bool     DayNightCycle { get; set; }
    public float    StartingRadius { get; set; }
    public float    SupplyRefill { get; set; }
    public float    SupplyTimer { get; set; }
    public MapType  Biome { get; set; }
}
