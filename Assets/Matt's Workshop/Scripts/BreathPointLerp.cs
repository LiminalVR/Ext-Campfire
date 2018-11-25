using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathPointLerp : MonoBehaviour {

    public BreathTime experienceTimelapse;

    public GameObject marker;
    public Transform P1, P2;
    public float transferTime;

	// Use this for initialization
	void Start () {
        ResetPosition ();
        transferTime = experienceTimelapse.experienceTime();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ResetPosition ()
    {
        marker.transform.position = P1.position;
    }
}
