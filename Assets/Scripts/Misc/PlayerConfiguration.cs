using Enumerators;

public class PlayerConfiguration
{
    public PlayerIndex    PlayerIndex   { get; set; }
    public TankColor      Color         { get; set; }
    public TankType       TankType      { get; set; }
    public ControllerType Controller    { get; set; }
    public override string ToString() => Color.ToString();    //ToDo: Include Naming?
}