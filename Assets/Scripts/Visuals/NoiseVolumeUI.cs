using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseVolumeUI : MonoBehaviour
{
    public PlaySound trackSound;
    public Transform noiseLinesFolder;
    GameObject[] noiseLines;
    void Start()
    {
        noiseLines = new GameObject[noiseLinesFolder.childCount];
        for (int i = 0; i < noiseLinesFolder.childCount; i++)
        {
            noiseLines[i] = noiseLinesFolder.GetChild(i).gameObject;
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
}
