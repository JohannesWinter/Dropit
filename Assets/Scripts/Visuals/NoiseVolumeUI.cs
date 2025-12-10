using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseVolumeUI : MonoBehaviour
{
    public PlaySound trackSound;
    public Transform noiseLinesFolder;
    GameObject[] noiseLines;
    public float[] noiseAmplitudes;
    void Start()
    {
        noiseLines = new GameObject[noiseLinesFolder.childCount];
        for (int i = 0; i < noiseLinesFolder.childCount; i++)
        {
            noiseLines[i] = noiseLinesFolder.GetChild(i).gameObject;
        }
        noiseAmplitudes = new float[noiseLines.Length];
        int x = noiseAmplitudes.Length / 2;
        for (int i = noiseAmplitudes.Length / 2; i >= 0; i--)
        {
            noiseAmplitudes[i] = ((float)i / 1.2f) / (noiseAmplitudes.Length / 2) + 0.17f;
            noiseAmplitudes[x] = ((float)i / 1.2f) / (noiseAmplitudes.Length / 2) + 0.17f;
            x++;
        }
    }
    void Update()
    {
        
    }

    public float GetCurrentPlaySoundVolume(PlaySound trackSound)
    {
        float time = trackSound.GetComponent<AudioSource>().time;
        AudioClip clip = trackSound.GetComponent<AudioSource>().clip;

        return GetSamplePeak(clip, time);
    }

    float GetSamplePeak(AudioClip clip, float time)
    {
        int index = (int)(time * clip.frequency);
        float[] sample = new float[1];
        clip.GetData(sample, index);
        return Mathf.Abs(sample[0]);
    }

    void UpdateNoiseLine(GameObject noiseLine, float amplitude)
    {
        Image lineImage = noiseLine.GetComponent<Image>();
        float noiseVolume = GetCurrentPlaySoundVolume(this.trackSound);
        noiseLine.GetComponent<RectTransform>().localScale = new Vector3(1, noiseVolume * amplitude, 1);
    }
}
