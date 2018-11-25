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

    private float totalExperienceTime;
    private float startTime, endTime;

	// Use this for initialization
	void Start () {
        stageTimes = GetStageTimesVector();
        totalExperienceTime = GetTotalExperienceTime();

        startTime = Time.time;
        endTime = startTime + totalExperienceTime;

        Debug.Log("Start Time " + startTime);
        Debug.Log("End Time " + endTime);
        Debug.Log("Stage Times: " + stageTimes);
        Debug.Log("Total Experience Time: " + totalExperienceTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // GetTotalTime - returns total time experience is expected to take
    private float GetTotalExperienceTime()
    {
        return stageTimes.x + stageTimes.y + stageTimes.z;
    }
    // GetStageTimeVector - places stage times in vector following Stage enum
    private Vector3 GetStageTimesVector()
    {
        return new Vector3(GetTotalStageTime((int)Stage.ONE), GetTotalStageTime((int)Stage.TWO), GetTotalStageTime((int)Stage.THREE));
    }
    // GetStageTime - returns span of time taken per stage
    private float GetTotalStageTime(int stage)
    {
        return 
            (breathTimes[(int)stage].x + 
             breathTimes[(int)stage].y + 
             breathTimes[(int)stage].z) 
           * stageRepetitions[(int)stage];
    }

    // utility functions
    public float experienceTime ()
    {
        return totalExperienceTime;
    }
}
