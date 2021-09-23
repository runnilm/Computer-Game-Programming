using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// this allows me to fade in the music (make it start quietly and get up to a good volume slowly)
// using audiomixer
public class FadeMixerGroup : MonoBehaviour {
    public AudioMixer audioMixer;

    void Start() {
        // dontdestroyonload is useful to keep the music playing seamlessly from start screen into the main game scene
        DontDestroyOnLoad(transform.gameObject);
        // begins the fade in of the music, with a 5 second duration and 1 target volume
        StartCoroutine(StartFade(audioMixer, "theme", 5.0f, 1f));
    }

    public static IEnumerator StartFade(AudioMixer audioMixer, string mixerParam, float duration, float targetVolume) {
        // takes current time and current volume as 0 (its actually higher but it doesn't really matter)
        float currentTime = 0;
        float currentVol = 0.0f;
        // gets "exposed parameter" (not really sure why its called that) mixerParam value
        audioMixer.GetFloat(mixerParam, out currentVol);
        // 10^(currentVol/20) = currentVol
        currentVol = Mathf.Pow(10, currentVol / 20);
        // update target value within clamped range
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        // ensure the effect happens over "duration" seconds
        while (currentTime < duration) {
            // update current time into duration
            currentTime += Time.deltaTime;
            // find new volume using Lerp (i really like this function, i have found some cool uses for it)
            // Lerp allows me to slowly interpolate between two values
            // current time / duration is essentially how much weve progressed through the duration
            float newVol = Mathf.Lerp(currentVol, currentTime / duration, targetValue);
            // sets the exposed parameter, a logarithmic conversion allows for the volume to be slowly faded at a consistent rate,
            // as opposed to the rate of change decreasing over time
            audioMixer.SetFloat(mixerParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
}
