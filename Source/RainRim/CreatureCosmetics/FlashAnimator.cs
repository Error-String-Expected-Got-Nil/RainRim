using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RainRim.CreatureCosmetics;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

public class FlashAnimator : IExposable
{
    public List<FlashAnimationStage> Stages;
    public float IntensityFactor;
    public bool Finished { get; private set; }
    
    private int _stageIndex;
    private int _stageStopwatch;
    private int _periodStopwatch;
    private bool _fallingPeriod = true;

    public FlashAnimator() {}
    
    public FlashAnimator(List<FlashAnimationStage> stages, float intensityFactor = 1f)
    {
        Stages = stages;
        IntensityFactor = intensityFactor;

        _periodStopwatch = -stages[0].phase % stages[0].duration;
        if (stages[0].phase > 0) _fallingPeriod = false;
    }

    public float PeekTick()
    {
        var intensity = Peek();
        Tick();
        return intensity;
    }

    public void Tick()
    {
        var stage = Stages[_stageIndex];
        
        _periodStopwatch++;
        if (_periodStopwatch > stage.period)
        {
            _periodStopwatch = 0;
            _fallingPeriod = !_fallingPeriod;
        }
        
        _stageStopwatch++;
        if (_stageStopwatch > stage.duration)
        {
            _stageStopwatch = 0;
            _stageIndex++;
            if (_stageIndex == Stages.Count)
            {
                Finished = true;
            }
            else
            {
                stage = Stages[_stageIndex];
                _fallingPeriod = true;
                _periodStopwatch = -stage.phase % stage.duration;
                if (stage.phase > 0) _fallingPeriod = false;
            }
        }
    }

    public float Peek()
    {
        if (Finished) return 0f;

        var stage = Stages[_stageIndex];
        var stageProgress = (float)_stageStopwatch / stage.duration;
        var periodProgress = (float)_periodStopwatch / stage.period;

        var maxIntensity = stage.maxIntensity * Mathf.Lerp(1f, stage.fadeFactor, stageProgress);
        var minIntensity = stage.minIntensity;

        float curIntensity;

        if (_fallingPeriod)
            curIntensity = stage.smooth 
                ? Mathf.SmoothStep(maxIntensity, minIntensity, periodProgress) 
                : maxIntensity;
        else 
            curIntensity = stage.smooth 
                ? Mathf.SmoothStep(minIntensity, maxIntensity, periodProgress) 
                : minIntensity;
        
        return curIntensity * IntensityFactor;
    }

    public void ExposeData()
    {
        Scribe_Collections.Look(ref Stages, nameof(Stages), LookMode.Deep);
        Scribe_Values.Look(ref IntensityFactor, nameof(IntensityFactor), 1f);
     
        var finished = Finished;
        Scribe_Values.Look(ref finished, nameof(Finished));
        if (Scribe.mode == LoadSaveMode.LoadingVars) Finished = finished;
        
        Scribe_Values.Look(ref _stageIndex, nameof(_stageIndex));
        Scribe_Values.Look(ref _stageStopwatch, nameof(_stageStopwatch));
        Scribe_Values.Look(ref _periodStopwatch, nameof(_periodStopwatch));
        Scribe_Values.Look(ref _fallingPeriod, nameof(_fallingPeriod), true);
    }
}

public class FlashAnimationDef : Def
{
    public int TotalDuration => stages.Sum(stage => stage.duration);
    
    public List<FlashAnimationStage> stages;

    public FlashAnimator GetAnimator(float intensityFactor = 1f) => new FlashAnimator(stages, intensityFactor);
    
    public override IEnumerable<string> ConfigErrors()
    {
        foreach (var error in base.ConfigErrors()) yield return error;

        for (var i = 0; i < stages.Count; i++)
        {
            var stage = stages[i];

            if (stage.duration <= 0)
                yield return "Stage " + i + " has duration <= 0";
        }
    }
}

public class FlashAnimationStage : IExposable
{
    public float maxIntensity = 1f;
    public float minIntensity = 1f;
    // This lerps from 1 -> set value over the course of the stage, and is a factor on max intensity
    // Ex.: At 0.5, max intensity will be half of its base value by the end of the stage
    public float fadeFactor = 1f; 
    public int duration = 60; // Total ticks the stage should last for
    public int period = 60; // Duration on and off when flickering. Set >= duration for no flicker.
    public int phase; // Subtracted from period stopwatch at start of stage
    public bool smooth = true; // False makes flickers have hard edges 

    public void ExposeData()
    {
        Scribe_Values.Look(ref maxIntensity, nameof(maxIntensity), 1f);
        Scribe_Values.Look(ref minIntensity, nameof(minIntensity), 1f);
        Scribe_Values.Look(ref fadeFactor, nameof(fadeFactor), 1f);
        Scribe_Values.Look(ref duration, nameof(duration), 60);
        Scribe_Values.Look(ref period, nameof(period), duration);
        Scribe_Values.Look(ref phase, nameof(phase));
        Scribe_Values.Look(ref smooth, nameof(smooth), true);
    }
}