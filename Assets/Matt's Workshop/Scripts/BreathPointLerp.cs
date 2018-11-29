using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

[System.Serializable]
[PreferBinarySerialization]
public class MarkerData
{
    public GameObject token, p1, p2;
}
public class BreathPointLerp : MonoBehaviour {

    // Garbage data
    public KeyCode resetKey;
    public int currentStage = 0;
    public string breathStage;

    // Actual stuff /// MEGA YIKE ///

    public BreathTime timerRef;

    public MarkerData total_Marker, stage1_Marker, stage2_Marker, stage3_Marker, breath_Marker;

    public float temp;

	// Use this for initialization
	void Start () {
        FullReset();
        breathStage = "IN";
    }
	
	// Update is called once per frame
	void Update () {
        TestFunc();
        if(total_Marker.token.transform.position != total_Marker.p2.transform.position)
            TrackBreath();

        temp += Time.deltaTime;

        total_Marker.token.transform.position  = Vector3.MoveTowards(total_Marker.token.transform.position,  total_Marker.p2.transform.position,  Time.deltaTime / timerRef.GetTotalExperienceTime());
        print(Time.deltaTime / timerRef.GetTotalExperienceTime());

        // begin stage 1
        stage1_Marker.token.transform.position = Vector3.MoveTowards(stage1_Marker.token.transform.position, stage1_Marker.p2.transform.position, Time.deltaTime / timerRef.GetTotalStageTime(0));

        // begin stage 2 when S1 marker is at end point
        if (stage1_Marker.token.transform.position == stage1_Marker.p2.transform.position)
        {
            currentStage = 1;
            stage2_Marker.token.transform.position = Vector3.MoveTowards(stage2_Marker.token.transform.position, stage2_Marker.p2.transform.position, Time.deltaTime / timerRef.GetTotalStageTime(1));
        }

        // begin stage 2 when S1 marker is at end point
        if (stage2_Marker.token.transform.position == stage2_Marker.p2.transform.position)
        {
            currentStage = 2;
            stage3_Marker.token.transform.position = Vector3.MoveTowards(stage3_Marker.token.transform.position, stage3_Marker.p2.transform.position, Time.deltaTime / timerRef.GetTotalStageTime(2));
        }
    }


    void TrackBreath()
    {
        if (breathStage == "IN")
        {
            breath_Marker.token.transform.position = Vector3.MoveTowards(breath_Marker.token.transform.position, breath_Marker.p1.transform.position, Time.deltaTime / timerRef.GetInhaleTime(currentStage));
            if (breath_Marker.token.transform.position == breath_Marker.p1.transform.position)
                breathStage = "HOLD";
        }
        if (breathStage == "HOLD")
        {
            breath_Marker.token.transform.position = Vector3.MoveTowards(breath_Marker.token.transform.position, breath_Marker.p2.transform.position, Time.deltaTime / timerRef.GetHoldTime(currentStage));
            if (breath_Marker.token.transform.position == breath_Marker.p2.transform.position)
                breathStage = "OUT";
        }
        if (breathStage == "OUT")
        {
            breath_Marker.token.transform.localPosition = Vector3.MoveTowards(breath_Marker.token.transform.localPosition, Vector3.zero, Time.deltaTime / timerRef.GetExhaleTime(currentStage));
            if (breath_Marker.token.transform.localPosition == Vector3.zero)
                breathStage = "IN";
        }
    }
    public void FullReset()
    {
        ResetPosition(total_Marker);
        ResetPosition(stage1_Marker);
        ResetPosition(stage2_Marker);
        ResetPosition(stage3_Marker);
        breath_Marker.token.transform.localPosition = Vector3.zero;
    }
    void ResetPosition (MarkerData marker)
    {
        marker.token.transform.position = marker.p1.transform.position;
    }
    void TestFunc()
    {
        if(Input.GetKeyDown(resetKey))
        {
            FullReset();
        }
    }
}
