using System;
using System.Collections;
using System.Collections.Generic;
using Liminal.Core.Fader;
using Liminal.SDK.Core;
using UnityEngine;

public class FireSystem : MonoBehaviour {

    ////////////////////////////////////////////////
    // Enum control definitions

    enum controlType
    {
        Campfire,
        Lantern
    }
    enum breathe
    {
        Inhale,
        Hold, 
        Exhale
    }

    ////////////////////////////////////////////////
    // Variable definitions
    [SerializeField]
    private BreathTime _BreathTime;
    private breathe step;    

    private float stage1, stage2, stage3, experienceTime;
    private float scaleVal;
    private Light lanternRef;

    [Header("Script Initialization")]
    [SerializeField]
    controlType isControlling;
    public GameObject controlledComponent;

    [Header("Campfire Properties")]
    public Vector3 minScale;
    public Vector3 maxScale;
    private Vector3 adjScale;

    [Header("Lantern Properties")]
    public float minIntensity;
    public float maxIntensity;
    private float adjIntensity;

    [Header("TEST INFO")]
    private float holdTimeSentinel;
    private int currentStage;

    private bool hasEnded = false;

    ////////////////////////////////////////////////
    // Monobehaviour Methods

    void Start () {
        if (_BreathTime == null)            _BreathTime = GameObject.FindObjectOfType<BreathTime>();
        if (controlledComponent == null)    controlledComponent = this.gameObject;

        currentStage = 0;
        step = breathe.Inhale;
        scaleVal = stage1 = stage2 = stage3 = experienceTime = holdTimeSentinel = 0f;
        if (isControlling == controlType.Lantern)
        {
            adjIntensity = maxIntensity - minIntensity;
            lanternRef = controlledComponent.GetComponent<Light>();
        }
        else
        {
            adjScale = maxScale - minScale;
            lanternRef = null;
        }
	}	
	void Update () {
        if(experienceTime != 1f)
        {
            ThisIsWhereTheFunBegins();
        }
        experienceTime = Mathf.MoveTowards(experienceTime, 1f, Time.deltaTime / _BreathTime.GetTotalExperienceTime() );
        TrackStages();
	}

    ////////////////////////////////////////////////
    // Custom Built Methods

    //----------------------
    // Public


    //----------------------
    // Private
    void ThisIsWhereTheFunBegins()
    {
        if (isControlling == controlType.Campfire)
        {
            TrackBreath_Fire();
        }
        else if (isControlling == controlType.Lantern)
        {
            TrackBreath_Lamp();
        }
        else
        {
            Debug.Log("ERROR:[FireSystem.cs] : Missing controlType argument");
        }
    }

    void TrackStages()
    {
        if(stage1 != 1f)
        {
            currentStage = 0;
            stage1 = Mathf.MoveTowards(stage1, 1f, Time.deltaTime / _BreathTime.GetTotalStageTime(currentStage));
        }
        if(stage1 == 1f && stage2 != 1f)
        {
            currentStage = 1;
            stage2 = Mathf.MoveTowards(stage2, 1f, Time.deltaTime / _BreathTime.GetTotalStageTime(currentStage));
        }
        if(stage2 == 1f && stage3 != 1f)
        {
            currentStage = 2;
            stage3 = Mathf.MoveTowards(stage3, 1f, Time.deltaTime / _BreathTime.GetTotalStageTime(currentStage));
        }
        else if (Math.Abs(stage2 - 1f) < 0.01f && Math.Abs(stage3 - 1f) < 0.01f && !hasEnded)
        {
            hasEnded = true;
            StartCoroutine(EndRoutine(2f));
        }
    }

    public void End(float fadeTime)
    {
        StartCoroutine(EndRoutine(fadeTime));
    }

    private IEnumerator EndRoutine(float fadeTime)
    {
        print("aaaa");
        var elapsedTime = 0f;
        var startingVolume = AudioListener.volume;

        ScreenFader.Instance.FadeToBlack(fadeTime);

        while (elapsedTime < fadeTime)
        {
            AudioListener.volume = Mathf.Lerp(startingVolume, 0f, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return ScreenFader.Instance.WaitUntilFadeComplete();

        ExperienceApp.End();
    }

    void TrackBreath_Fire()
    {
        switch(step)
        {
            case (breathe.Inhale):
                scaleVal = Mathf.MoveTowards(scaleVal, 1f, Time.deltaTime / _BreathTime.GetInhaleTime(currentStage));
                controlledComponent.transform.localScale = minScale + (adjScale * scaleVal);

                if(controlledComponent.transform.localScale == maxScale)
                {
                    holdTimeSentinel = Time.timeSinceLevelLoad;
                    step = breathe.Hold;
                    Debug.Log("Switching to [Hold]");
                }
                break;

            case (breathe.Hold):
                if(Time.timeSinceLevelLoad >= holdTimeSentinel + _BreathTime.GetHoldTime(currentStage))
                {
                    //holdTimeSentinel = 0f;
                    step = breathe.Exhale;
                    Debug.Log("Switching to [Exhale]");
                }
                break;

            case (breathe.Exhale):
                scaleVal = Mathf.MoveTowards(scaleVal, 0f, Time.deltaTime / _BreathTime.GetExhaleTime(currentStage));
                controlledComponent.transform.localScale = minScale + (adjScale * scaleVal);

                if (controlledComponent.transform.localScale == minScale)
                {
                    step = breathe.Inhale;
                    Debug.Log("Switching to [Inhale]");
                }
                break;

            default:
                Debug.Log("ERROR:[FireSystem.cs] : Breath Tracking system error");
                break;
        }
    }
    void TrackBreath_Lamp()
    {
        switch (step)
        {
            case (breathe.Inhale):
                scaleVal = Mathf.MoveTowards(scaleVal, 1f, Time.deltaTime / _BreathTime.GetInhaleTime(currentStage));
                lanternRef.intensity = minIntensity + (adjIntensity * scaleVal);

                if (lanternRef.intensity == maxIntensity)
                {
                    holdTimeSentinel = Time.timeSinceLevelLoad;
                    step = breathe.Hold;
                    Debug.Log("Switching to [Hold]");
                }
                break;

            case (breathe.Hold):
                if (Time.timeSinceLevelLoad >= holdTimeSentinel + _BreathTime.GetHoldTime(currentStage))
                {
                    //holdTimeSentinel = 0f;
                    step = breathe.Exhale;
                    Debug.Log("Switching to [Exhale]");
                }
                break;

            case (breathe.Exhale):
                scaleVal = Mathf.MoveTowards(scaleVal, 0f, Time.deltaTime / _BreathTime.GetExhaleTime(currentStage));
                lanternRef.intensity = minIntensity + (adjIntensity * scaleVal);

                if (lanternRef.intensity == minIntensity)
                {
                    step = breathe.Inhale;
                    Debug.Log("Switching to [Inhale]");
                }
                break;

            default:
                Debug.Log("ERROR:[FireSystem.cs] : Breath Tracking system error");
                break;
        }
    }


    //~~~~~~~~~~~~~~~~~~~~~~
    // TEST CODE

    void LogData()
    {
        //Debug.Log(
        //    "ExpTime: " + experienceTime + 
        //    " CurrS: " + currentStage +
        //    " S1: " + stage1 +
        //    " S2: " + stage2 +
        //    " S3: " + stage3);

        Debug.Log(
            "Inhale: " + _BreathTime.GetInhaleTime(currentStage) +
            " Hold: " + _BreathTime.GetHoldTime(currentStage) +
            " Exhale: " + _BreathTime.GetExhaleTime(currentStage));
    }
}
