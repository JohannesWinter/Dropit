using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseVolumeUI : MonoBehaviour
{
    public PlaySound trackSound;

    void Start()
    {
        
    }
    void Update()
    {
        
    }


    float GetSamplePeak(AudioClip clip, float time)
    {
        int index = (int)(time * clip.frequency);
        float[] sample = new float[1];
        clip.GetData(sample, index);
        return Mathf.Abs(sample[0]);
    }
}
