using Force.DeepCloner;
using Force.DeepCloner.Helpers;

namespace Celeste64.SpeedrunUtils;

public record SaveState
{
    public required World World;
    public required Save.LevelRecord Record;
    
    public static SaveState Create()
    {
        if (Game.Scene is not World world)
            throw new Exception("Current scene is not World");
        
        return new SaveState()
        {
            World = world.DeepClone(),
            Record = Save.CurrentRecord.DeepClone(),
        };
    }
    
    public void Load()
    {
        // Don't save-state time
        var prevTime = Save.CurrentRecord.Time;
        Record.DeepCloneTo(Save.CurrentRecord);
        Save.Instance.LevelID = Record.ID;
        Save.CurrentRecord.Time = prevTime;
        
        // Disabled currently, since the World is invisible on the 2nd load 
        const bool FastLoadAnimation = false;
        if (FastLoadAnimation) 
        {
            // Adapted from Game::Update's transition handling
            if (Game.Instance.scenes.TryPeek(out var lastScene))
            {
                lastScene.Exited();
                lastScene.Disposed();
            }
            
            if (Game.Instance.scenes.Count > 0)
                Game.Instance.scenes.Pop();
            Game.Instance.scenes.Push(World);
            
            if (Game.Instance.scenes.TryPeek(out var nextScene))
            {
                nextScene.Entered();
                nextScene.Update();
            }
            
            {
                var last = Game.Instance.Music.IsPlaying && lastScene != null ? lastScene.Music : string.Empty;
                var next = nextScene?.Music ?? string.Empty;
                if (next != last)
                {
                    Game.Instance.Music.Stop();
                    Game.Instance.Music = Audio.Play(next);
                    if (Game.Instance.Music)
                        Game.Instance.Music.SetCallback(Game.Instance.audioEventCallback);
                }
            }
            
            {
                var last = Game.Instance.Ambience.IsPlaying && lastScene != null ? lastScene.Ambience : string.Empty;
                var next = nextScene?.Ambience ?? string.Empty;
                if (next != last)
                {
                    Game.Instance.Ambience.Stop();
                    Game.Instance.Ambience = Audio.Play(next);
                }
            }
            
            Save.Instance.SyncSettings();
        }
        else
        {
            Game.Instance.Goto(new Transition()
            {
                Mode = Transition.Modes.Replace,
                Scene = () => World.DeepClone(), // We need to deep-clone again, to allow loading the state multiple times
                ToPause = true,
                ToBlack = new AngledWipe(),
                PerformAssetReload = true
            });
        }
    }
    
    public void Clear()
    {
        World.Disposed();
    }

    public static void Configure()
    {
        DeepCloner.SetKnownTypesProcessor(type =>
        {
            if (
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
}