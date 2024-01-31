using Force.DeepCloner;

namespace Celeste64.SpeedrunUtils;

public static class SpeedrunUtilsMod
{
    public static void Load()
    {
        DeepCloner.SetKnownTypesProcessor(type =>
        {
            if (
                // Celeste64 Resources
                type.IsAssignableTo(typeof(Model)) ||
                
                // Foster Graphics Resources
                type.IsAssignableTo(typeof(Material)) ||
                type.IsAssignableTo(typeof(Texture)) ||
                type.IsAssignableTo(typeof(Mesh)) ||
                type.IsAssignableTo(typeof(Batcher)) ||
                type.IsAssignableTo(typeof(Target))
            ) return true;
            
            return null;
        });
    }
    
    
    public static void Unload()
    {
        
    }
    
    private const int MaxSaveStateSlots = 10;
    private static int currentSlot = 0;
    private static bool loadQueued = false;
    private static readonly SaveState?[] currentStates = new SaveState[MaxSaveStateSlots];
    public static void Update()
    {
        if (SpeedrunUtilsControls.SaveState.Pressed)
        {
            currentStates[currentSlot]?.Clear();
            currentStates[currentSlot] = SaveState.Create();
        }
        if (SpeedrunUtilsControls.LoadState.Pressed || loadQueued)
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
            }
        }
        if (SpeedrunUtilsControls.ClearState.Pressed)
        {
            currentStates[currentSlot]?.Clear();
            currentStates[currentSlot] = null;
        }
        
        // Switch save state slot
        if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.D1)) currentSlot = 0;
        else if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.D2)) currentSlot = 1;
        else if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.D3)) currentSlot = 2;
        else if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.D4)) currentSlot = 3;
        else if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.D5)) currentSlot = 4;
        else if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.D6)) currentSlot = 5;
        else if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.D7)) currentSlot = 6;
        else if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.D8)) currentSlot = 7;
        else if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.D9)) currentSlot = 8;
        else if (Input.Keyboard.Down(Keys.LeftControl) && Input.Keyboard.Down(Keys.D0)) currentSlot = 9;
    }
}