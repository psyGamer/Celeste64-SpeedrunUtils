namespace Celeste64.SpeedrunUtils;

public class SpeedrunUtilsControls
{
    public static readonly VirtualButton SaveState = new("SaveState");
    public static readonly VirtualButton LoadState = new("LoadState");
    public static readonly VirtualButton ClearState = new("ClearState");
    
    public static void Load(ControlsConfig? config = null)
    {
        // Taken from Controls.Load()
        static List<ControlsConfig.Binding> FindAction(ControlsConfig? config, string name)
        {
            if (config != null && config.Actions.TryGetValue(name, out var action))
                return action;
            if (Defaults.Actions.TryGetValue(name, out action))
                return action;
            throw new Exception($"Missing Action Binding for '{name}'");
        }
        
        foreach (var it in FindAction(config, "SpeedrunUtils_SaveState"))
            it.BindTo(SaveState);
        foreach (var it in FindAction(config, "SpeedrunUtils_LoadState"))
            it.BindTo(LoadState);
        foreach (var it in FindAction(config, "SpeedrunUtils_ClearState"))
            it.BindTo(ClearState);
    }
    
    public static ControlsConfig Defaults = new()
	{
		Actions = new() {
			["SpeedrunUtils_SaveState"] = [
                new(Keys.F7),
			],
			["SpeedrunUtils_LoadState"] = [
				new(Keys.F8),
			],
			["SpeedrunUtils_ClearState"] = [
				new(Keys.F4),
                new(Keys.F6),
			],
		},
	};
}