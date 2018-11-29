using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathTime : MonoBehaviour {


    enum Stage
    {
        ONE = 0,
        TWO = 1, 
        THREE = 2
    }

    [Tooltip("Breathing stage timing.\n(x = Inhale, y = Pause, z = Exhale)")]
    public List<Vector3> breathTimes;

    [Tooltip("How many full breath cycles are made per stage.\n(x = Stage 1, y = Stage 2, z = Stage 3)")]
    public Vector3 stageRepetitions;

    [Tooltip("SET BY ENGINE\nTotal time each stage is active for.\n(x = Stage 1, y = Stage 2, z = Stage 3)")]
    [SerializeField]
    private Vector3 stageTimes;

    [SerializeField]
    private float totalExperienceTime;
    [SerializeField]
    private float startTime, endTime;

	// Use this for initialization
	void Start () {
        stageTimes = GetStageTimesVector();
        totalExperienceTime = GetTotalExperienceTime();

        startTime = Time.time;
        endTime = startTime + totalExperienceTime;

        //Debug.Log("Start Time " + startTime);
        //Debug.Log("End Time " + endTime);
        //Debug.Log("Stage Times: " + stageTimes);
        //Debug.Log("Total Experience Time: " + totalExperienceTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // GetTotalTime - returns total time experience is expected to take
    public float GetTotalExperienceTime()
    {
        return stageTimes.x + stageTimes.y + stageTimes.z;
    }
    // GetStageTimeVector - places stage times in vector following Stage enum
    private Vector3 GetStageTimesVector()
    {
        return new Vector3(GetTotalStageTime((int)Stage.ONE), GetTotalStageTime((int)Stage.TWO), GetTotalStageTime((int)Stage.THREE));
    }
    // GetStageTime - returns span of time taken per stage
    public float GetTotalStageTime(int stage)
    {
        Debug.Log("rep1: " + stageRepetitions[0]);
        Debug.Log("rep2: " + stageRepetitions[1]);
        Debug.Log("rep3: " + stageRepetitions[2]);

        return 
            (breathTimes[(int)stage].x + 
             breathTimes[(int)stage].y + 
             breathTimes[(int)stage].z) 
           * stageRepetitions[stage];
    }
    // utility functions
    public float ExperienceTime ()
    {
        return totalExperienceTime;
    }

    public float GetInhaleTime(int stage)
    {
        Debug.Log("Inhale: " + breathTimes[stage].z);
        return breathTimes[stage].x;
    }
    public float GetHoldTime(int stage)
    {
        Debug.Log("Hold: " + breathTimes[stage].z);
        return breathTimes[stage].y;
    }
    public float GetExhaleTime(int stage)
    {
        Debug.Log("Exhale: " + breathTimes[stage].z);
        return breathTimes[stage].z;
    }
}
