using Force.DeepCloner;

namespace Celeste64.SpeedrunUtils;

public static class SpeedrunUtilsMod
{
    private const float ToastTime = 1.0f;
    private static string toastText = string.Empty;
    private static float toastTimer = 0.0f;
        
    public const int MaxSaveStateSlots = 10;
    private static int currentSlot = 0;
    private static bool loadQueued = false;
    private static readonly SaveState?[] currentStates = new SaveState[MaxSaveStateSlots];
    
    private static bool slowdown = false;
    public static float GameSpeed => slowdown ? Time.Delta * Save.Instance.SpeedrunSlowdownFactor : Time.Delta;
    
    public static void Load()
    {
        SaveState.Configure();
    }
    
    public static void Unload()
    {
        
    }
    
    public static void Update()
    {
        if (SpeedrunUtilsControls.SaveState.ConsumePress())
        {
            currentStates[currentSlot]?.Clear();
            currentStates[currentSlot] = SaveState.Create();
            
            toastText = $"Saved save-state slot {currentSlot + 1}";
            toastTimer = ToastTime;
        }
        if (SpeedrunUtilsControls.LoadState.ConsumePress() || loadQueued)
        {
            if (!(Game.Instance.transitionStep == Game.TransitionStep.None ||
                  Game.Instance.transitionStep == Game.TransitionStep.FadeIn))
            {
                // Can't trigger transition currently, so queue the load.
                loadQueued = true;
            } else
            {
                currentStates[currentSlot]?.Load();
                loadQueued = false;
                
                toastText = $"Loaded save-state slot {currentSlot + 1}";
                toastTimer = ToastTime;
            }
        }
        if (SpeedrunUtilsControls.ClearState.ConsumePress())
        {
            currentStates[currentSlot]?.Clear();
            currentStates[currentSlot] = null;
            
            toastText = $"Cleared save-state slot {currentSlot + 1}";
            toastTimer = ToastTime;
        }
        
        if (SpeedrunUtilsControls.ToggleSlowdown.ConsumePress())
        {
            slowdown = !slowdown;
        }
        
        // Switch between save-state slots
        if (SpeedrunUtilsControls.SlotChangeModDown)
        {
            int oldSlot = currentSlot;
            for (int i = 0; i < MaxSaveStateSlots; i++)
            {
                if (SpeedrunUtilsControls.Slots[i].ConsumePress()) currentSlot = i;
            }
            if (oldSlot != currentSlot)
            {
                toastText = $"Selected save-state slot {currentSlot + 1}";
                toastTimer = ToastTime;
            }
        }
        
        toastTimer -= Time.Delta;
    }
    
    private static readonly Batcher batch = new();
    public static void Render(Target target)
    {
        if (toastTimer <= 0) return;
        
        batch.SetSampler(new TextureSampler(TextureFilter.Linear, TextureWrap.ClampToEdge, TextureWrap.ClampToEdge));
        var font = Assets.Fonts.First().Value;
        var bounds = new Rect(0, 0, target.Width, target.Height);
        
        UI.Text(batch, toastText, bounds.BottomLeft - new Vec2(-10, 10) * Game.RelativeScale, new Vec2(0, 1), Color.White);
        batch.Render(target);
        batch.Clear();
    }
}