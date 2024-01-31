namespace Celeste64.SpeedrunUtils;

public class SpeedrunUtilsControls
{
    public static readonly VirtualButton SaveState = new("SaveState");
    public static readonly VirtualButton LoadState = new("LoadState");
    public static readonly VirtualButton ClearState = new("ClearState");
    
    public static void Load()
    {
        SaveState.Clear();
        SaveState.Add(Keys.F7);
        
        LoadState.Clear();
        LoadState.Add(Keys.F8);
        
        ClearState.Clear();
        ClearState.Add(Keys.F4);
    }
}