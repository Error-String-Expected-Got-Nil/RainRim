using RainRim.CreatureCosmetics;
using RainRim.Utils;
using UnityEngine;
using Verse;

namespace RainRim.Effects;

public struct FleckLizardBubble : IFleck
{
    // Every this many seconds, the fleck's direction will change
    public const float TurnInterval = 1f / 40f;
    
    public FleckDef Def;
    public float Life;
    public float Lifetime;
    public float TurnStopwatch;
    public float AgeSecs;
    public float ScaleFactor;
    public Vector3 Position;
    public Vector3 Velocity;
    public Vector3 Scale;
    public int SetupTick;
    public Vector3 SpawnPosition;
    public ThingComp_LizardMoodHandler SourceMoodHandler;
    
    public void Setup(FleckCreationData creationData)
    {
        var angleRad = creationData.velocityAngle * Mathf.Deg2Rad;
        var spawnPos = creationData.spawnPosition;
        var extraVelocity = Vector3.zero;
        if (creationData.link is { Linked: true, Target: { HasThing: true, Thing: Pawn lizard } })
        {
            SourceMoodHandler = lizard.GetComp<ThingComp_LizardMoodHandler>();
            spawnPos = creationData.spawnPosition + EffectUtils.GetHeadOffset(lizard);
            extraVelocity = lizard.Drawer.tweener.LastTickTweenedVelocity;
        }
        
        Def = creationData.def;
        Life = 1f;
        // For authenticity, this is basically how Rain World determines lizard bubble lifetime.
        Lifetime = 0.5f + 5f * Random.value * Random.value * Random.value * Random.value;
        TurnStopwatch = 0f;
        Position = spawnPos;
        Velocity = new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad)) * creationData.velocitySpeed 
                   + extraVelocity;
        ScaleFactor = 0.5f;
        Scale = new Vector3(creationData.scale, creationData.scale, creationData.scale);
        SetupTick = Find.TickManager.TicksGame;
        SpawnPosition = creationData.spawnPosition;
    }

    public bool TimeInterval(float deltaTime, Map map)
    {
        AgeSecs += deltaTime;
        TurnStopwatch += deltaTime;
        // Rain World randomly changes the angle every tick, but it has 40 ticks per second, while Rimworld has 60,
        // so we use this stopwatch instead.
        if (TurnStopwatch > TurnInterval)
        {
            TurnStopwatch -= TurnInterval;
            // This is how Rain World does it.
            var angle = 2f * Mathf.PI * Random.value;
            Velocity = Vector3.Slerp(Velocity, new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)), 0.2f);
        }

        Position += Velocity * deltaTime;
        Life -= 1f / Lifetime * deltaTime;
        ScaleFactor = (Mathf.Sin(Mathf.PI * Life) + Life) * 0.5f;
        
        return Life <= 0f; // true means fleck should be destroyed
    }

    public void Draw(DrawBatch batch)
    {
        var id = SetupTick + SpawnPosition.GetHashCode();
        ((Graphic_Fleck)Def.GetGraphicData(id).Graphic).DrawFleck(new FleckDrawData
        {
            alpha = 1f,
            color = SourceMoodHandler?.LastHeadColor ?? Color.black,
            drawLayer = 0,
            pos = Position,
            rotation = 0f,
            scale = Scale * ScaleFactor,
            ageSecs = AgeSecs,
            id = id
        }, batch);
    }
}