using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPrefab : MonoBehaviour {

    public float minTime;
    public float maxTime;

    public float timeUntilPlayAgain;

    public List<AudioClip> audioTracks = new List<AudioClip>();

    public int CurrentTrackResult;

    public AudioClip CurrentTrack;
    public AudioSource audioOutput;

	// Use this for initialization
	void Start ()
    {
        audioOutput = GetComponent<AudioSource>();

        timeUntilPlayAgain = Random.Range(minTime, maxTime);

    }
	
	// Update is called once per frame
	void Update ()
    {
        timeUntilPlayAgain -= Time.deltaTime;
        if (timeUntilPlayAgain < 0)
        {
            CurrentTrackResult = Random.Range(0, audioTracks.Count);

            CurrentTrack = audioTracks[CurrentTrackResult];
            audioOutput.clip = CurrentTrack;
            audioOutput.Play();
            timeUntilPlayAgain = Random.Range(minTime, maxTime);
        }
	}
}
