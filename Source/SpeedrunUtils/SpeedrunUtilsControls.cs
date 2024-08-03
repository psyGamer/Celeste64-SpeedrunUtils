namespace Celeste64.SpeedrunUtils;

public class SpeedrunUtilsControls
{
    public static readonly VirtualButton SaveState = new("SaveState");
    public static readonly VirtualButton LoadState = new("LoadState");
    public static readonly VirtualButton ClearState = new("ClearState");
    public static readonly VirtualButton ToggleSlowdown = new("ToggleSlowdown");
    
    public static bool SlotChangeModDown => SlotChangeMod.Down || SlotChangeMod.Bindings.Count == 0; 
    private static readonly VirtualButton SlotChangeMod = new("SlotChangeMod");
    public static readonly VirtualButton[] Slots = new VirtualButton[SpeedrunUtilsMod.MaxSaveStateSlots]
    {
        new("Slot0"),
        new("Slot1"),
        new("Slot2"),
        new("Slot3"),
        new("Slot4"),
        new("Slot5"),
        new("Slot6"),
        new("Slot7"),
        new("Slot8"),
        new("Slot9"),
    };
    
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
        foreach (var it in FindAction(config, "SpeedrunUtils_ToggleSlowdown"))
            it.BindTo(ToggleSlowdown);
        
        foreach (var it in FindAction(config, "SpeedrunUtils_SlotChangeMod"))
            it.BindTo(SlotChangeMod);
        for (int i = 0; i < Slots.Length; i++)
        {
            foreach (var it in FindAction(config, $"SpeedrunUtils_Slot{i}"))
                it.BindTo(Slots[i]);
        }
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
            ["SpeedrunUtils_ToggleSlowdown"] = [
                new (Keys.F3),
            ],
            
            ["SpeedrunUtils_SlotChangeMod"] = [
                new (Keys.LeftControl),
                new (Keys.RightControl),
            ],
            ["SpeedrunUtils_Slot0"] = [new (Keys.D1)],
            ["SpeedrunUtils_Slot1"] = [new (Keys.D2)],
            ["SpeedrunUtils_Slot2"] = [new (Keys.D3)],
            ["SpeedrunUtils_Slot3"] = [new (Keys.D4)],
            ["SpeedrunUtils_Slot4"] = [new (Keys.D5)],
            ["SpeedrunUtils_Slot5"] = [new (Keys.D6)],
            ["SpeedrunUtils_Slot6"] = [new (Keys.D7)],
            ["SpeedrunUtils_Slot7"] = [new (Keys.D8)],
            ["SpeedrunUtils_Slot8"] = [new (Keys.D9)],
            ["SpeedrunUtils_Slot9"] = [new (Keys.D0)],
		},
	};
}