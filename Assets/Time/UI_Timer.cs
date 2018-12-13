using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Timer : MonoBehaviour {

    public float timer;
    public float timeToStop;

    public GameObject[] UIElementToDelete;



    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeToStop)
        {
            foreach (GameObject _gameObject in UIElementToDelete)
            {
                _gameObject.SetActive(false);
            }
        }
    }
}