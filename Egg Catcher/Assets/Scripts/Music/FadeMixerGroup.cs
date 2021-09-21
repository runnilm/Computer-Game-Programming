using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FadeMixerGroup : MonoBehaviour {
    public AudioMixer audioMixer;

    void Start() {
        DontDestroyOnLoad(transform.gameObject);
        StartCoroutine(StartFade(audioMixer, "theme", 5.0f, 1f));
    }

    public static IEnumerator StartFade(AudioMixer audioMixer, string mixerParam, float duration, float targetVolume) {
        float currentTime = 0;
        float currentVol = 0.0f;
        audioMixer.GetFloat(mixerParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, currentTime / duration, targetValue);
            audioMixer.SetFloat(mixerParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
}
