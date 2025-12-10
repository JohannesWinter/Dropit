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
        if (trackSound != null)
        {
            UpdateAlleNoiseLines(noiseLines, noiseAmplitudes);
        }
    }

    public float GetCurrentPlaySoundVolume(PlaySound trackSound)
    {
        AudioSource audioSource = trackSound.GetComponent<AudioSource>();
        if (audioSource == null || audioSource.clip == null)
        {
            return 0;
        }
        if (audioSource.time < audioSource.clip.length)
        {
            return GetSamplePeak(audioSource);
        }
        return 0;
    }

    float GetSamplePeak(AudioSource audioSource)
    {
        float[] clipSampleData = new float[128];
        audioSource.clip.GetData(clipSampleData, audioSource.timeSamples);
        float clipLoudness = 0f;
        foreach (var sample in clipSampleData)
        {
            clipLoudness += Mathf.Abs(sample);
        }
        clipLoudness /= clipSampleData.Length;
        return clipLoudness;
    }

    void UpdateAlleNoiseLines(GameObject[] noiseLines, float[] noiseAmplitudes)
    {
        for (int i = 0; i < noiseLines.Length; i++)
        {
            UpdateNoiseLine(noiseLines[i], noiseAmplitudes[i]);
        }
    }

    void UpdateNoiseLine(GameObject noiseLine, float amplitude)
    {
        Image lineImage = noiseLine.GetComponent<Image>();
        float noiseVolume = GetCurrentPlaySoundVolume(this.trackSound);
        noiseLine.GetComponent<RectTransform>().localScale = new Vector3(1, noiseVolume * amplitude, 1);
    }
}
