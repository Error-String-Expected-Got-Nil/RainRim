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
    public Pawn Anchor;
    
    public void Setup(FleckCreationData creationData)
    {
        // Rimworld considers 0 degrees to be north and goes clockwise from there instead of counterclockwise,
        // so we must subtract the given angle from 90 to get an angle that works for trigonometry.
        var angleRad = (90 - creationData.velocityAngle) * Mathf.Deg2Rad;
        var spawnPos = Vector3.zero;
        if (creationData.link is { Linked: true, Target: { HasThing: true, Thing: Pawn lizard } })
        {
            SourceMoodHandler = lizard.GetComp<ThingComp_LizardMoodHandler>();
            spawnPos = EffectUtils.GetHeadOffset(lizard);
            Anchor = lizard;
        }
        
        Def = creationData.def;
        Life = 1f;
        // For authenticity, this is basically how Rain World determines lizard bubble lifetime.
        Lifetime = Def.solidTime + Def.fadeOutTime * Random.value * Random.value * Random.value * Random.value;
        TurnStopwatch = 0f;
        Position = spawnPos;
        Velocity = new Vector3(Mathf.Cos(angleRad), 0f, Mathf.Sin(angleRad)) * creationData.velocitySpeed;
        ScaleFactor = 0.5f;
        Scale = new Vector3(creationData.scale, creationData.scale, creationData.scale);
        SetupTick = Find.TickManager.TicksGame;
        SpawnPosition = creationData.spawnPosition;

        // Start flecks a bit farther out so they aren't all on top of each other
        Position += Velocity * 0.2f;
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
            pos = (Anchor.DrawPosHeld ?? Anchor.DrawPos) + Position,
            rotation = 0f,
            scale = Scale * ScaleFactor,
            ageSecs = AgeSecs,
            id = id
        }, batch);
    }
}