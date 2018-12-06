using System.Collections;
using System.Collections.Generic;
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
    

    public float stage1, stage2, stage3, experienceTime;

    [Header("Script Initialization")]
    [SerializeField]
    controlType isControlling;
    public GameObject controlledComponent;

    [Header("Campfire Properties")]
    public Vector3 minScale;
    public Vector3 maxScale;

    [Header("Lantern Properties")]
    public float minIntensity;
    public float maxIntensity;

    [Header("TEST INFO")]
    [SerializeField]
    private float holdTimeSentinel;
    [SerializeField]
    private int currentStage;

    ////////////////////////////////////////////////
    // Monobehaviour Methods

    void Start () {
        if (_BreathTime == null)            _BreathTime = GameObject.FindObjectOfType<BreathTime>();
        if (controlledComponent == null)    controlledComponent = this.gameObject;

        currentStage = 0;
        step = breathe.Inhale;
        stage1 = stage2 = stage3 = experienceTime = holdTimeSentinel = 0f; 
	}	
	void Update () {
        //LogData();

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
    }

    void TrackBreath_Fire()
    {
        switch(step)
        {
            case (breathe.Inhale):
                controlledComponent.transform.localScale = Vector3.MoveTowards(controlledComponent.transform.localScale, maxScale, Time.deltaTime / _BreathTime.GetInhaleTime(currentStage));
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
                controlledComponent.transform.localScale = Vector3.MoveTowards(controlledComponent.transform.localScale, minScale, Time.deltaTime / _BreathTime.GetExhaleTime(currentStage));
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
                break;

            case (breathe.Hold):
                break;

            case (breathe.Exhale):
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
        Debug.Log(
            "ExpTime: " + experienceTime + 
            " CurrS: " + currentStage +
            " S1: " + stage1 +
            " S2: " + stage2 +
            " S3: " + stage3);
    }
}
