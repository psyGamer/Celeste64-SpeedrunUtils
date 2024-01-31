using Force.DeepCloner;
using Force.DeepCloner.Helpers;

namespace Celeste64.SpeedrunUtils;

public record SaveState
{
    public required World World;
    public required Save.LevelRecord Record;
    
    // Objects which can't be disposed, since they are save-stated
    internal static readonly HashSet<object> PreventDispose = new();
    
    public static SaveState Create()
    {
        if (Game.Scene is not World world)
            throw new Exception("Current scene is not World");
        
        var state = new SaveState()
        {
            World = world.DeepClone(),
            Record = Save.CurrentRecord.DeepClone(),
        };
        if (state.World.postTarget != null)
            PreventDispose.Add(state.World.postTarget);
        return state;
    }
    
    public void Load()
    {
        // Save.Instance.EraseRecord(Record.ID);
        // Save.Instance.Records.Add(Record);
        Game.Instance.Goto(new Transition()
        {
            Mode = Transition.Modes.Replace,
            Scene = () => World.DeepClone(), // We need to deep-clone again, to allow loading the state multiple times
            ToPause = true,
            ToBlack = new AngledWipe(),
            PerformAssetReload = true
        });
    }
    
    public void Clear()
    {
        // Dispose resources of the World
        if (World.postTarget != null && PreventDispose.Contains(World.postTarget))
        {
            PreventDispose.Remove(World.postTarget);
            if (Game.Scene is not World world) return;
            if (World.postTarget == world.postTarget) return; // It's still currently in use

            World.postTarget.Dispose();
            World.postTarget = null;
        }
    }
}