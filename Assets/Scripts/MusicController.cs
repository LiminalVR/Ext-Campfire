using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource AudioSource;
    public float StartTime;
    public float FadeInTime;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(StartTime);

        var elapsedTime = 0f;
        var targetVolume = AudioSource.volume;
        AudioSource.volume = 0f;

        AudioSource.Play();

        while (elapsedTime< FadeInTime)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            AudioSource.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / FadeInTime);
        }

        AudioSource.volume = targetVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
