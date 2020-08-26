using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AppController : MonoBehaviour
{
    [Header("App Settings (Seconds)")]
    public float FadeStartTime = 30;
    public float TimeElapsed = 0;

    public UnityEvent OnFadeBegin;

    private void Start()
    {
        StartCoroutine(Loop());
        StartCoroutine(DisplayTimeLapsed());
    }

    private IEnumerator Loop()
    {
        yield return new WaitForSeconds(FadeStartTime);
        OnFadeBegin?.Invoke();
    }

    private IEnumerator DisplayTimeLapsed()
    {
        var startTime = Time.time;
        while (true)
        {
            TimeElapsed = Time.time - startTime;
            yield return null;
        }
    }
}
