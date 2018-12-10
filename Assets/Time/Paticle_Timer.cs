using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paticle_Timer : MonoBehaviour {

	public float timer;
	public float timeToStartP;

	public GameObject[] allParticles;



	void Update () 
	{
		timer += Time.deltaTime;

		if (timer > timeToStartP) 
		{
			print ("OK");
			foreach (GameObject _gameObject in allParticles) 
			{
				_gameObject.SetActive (true);
				print ("OK");
			}
		}
	}
}
