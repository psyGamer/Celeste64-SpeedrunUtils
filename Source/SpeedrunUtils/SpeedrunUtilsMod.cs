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
    
    private static SaveState? currentState;
    
    public static void Unload()
    {
        
    }
    
    private static bool loadQueued = false;
    public static void Update()
    {
        if (SpeedrunUtilsControls.SaveState.Pressed)
        {
            currentState = SaveState.Create();
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
                currentState?.Load();
                loadQueued = false;
            }
        }
    }
}