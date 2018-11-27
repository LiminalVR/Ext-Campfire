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

    public BreathTime timerRef;

    public MarkerData total_Marker, stage1_Marker, stage2_Marker, stage3_Marker, breath_Marker;

	// Use this for initialization
	void Start () {
        FullReset();
    }
	
	// Update is called once per frame
	void Update () {
        total_Marker.token.transform.position = Vector3.MoveTowards(total_Marker.token.transform.position, total_Marker.p2.transform.position, /*Time.deltaTime / */timerRef.ExperienceTime());
        stage1_Marker.token.transform.position = Vector3.MoveTowards(stage1_Marker.token.transform.position, stage1_Marker.p2.transform.position, Time.deltaTime / timerRef.GetTotalStageTime(0));
        stage2_Marker.token.transform.position = Vector3.MoveTowards(stage2_Marker.token.transform.position, stage2_Marker.p2.transform.position, Time.deltaTime / timerRef.GetTotalStageTime(1));
        stage3_Marker.token.transform.position = Vector3.MoveTowards(stage3_Marker.token.transform.position, stage3_Marker.p2.transform.position, Time.deltaTime / timerRef.GetTotalStageTime(2));
    }


    void TrackTotal()
    {

    }


    public void FullReset()
    {
        ResetPosition(total_Marker);
        ResetPosition(stage1_Marker);
        ResetPosition(stage2_Marker);
        ResetPosition(stage3_Marker);
        ResetPosition(breath_Marker);
    }

    void ResetPosition (MarkerData marker)
    {
        marker.token.transform.position = marker.p1.transform.position;
    }

    void TestFunc()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            FullReset();
        }
    }
}
