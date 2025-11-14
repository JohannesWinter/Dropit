using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource audiosource;
    public float pitch = 1.0f;
    bool played = false;
    bool stopped = false;
    public SoundType soundtype;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (soundtype == SoundType.Factory)
        {
            if (Manager.m.paused)
            {
                this.gameObject.GetComponent<AudioSource>().Pause();
                stopped = true;
            }
            else if (played == true)
            {
                if (this.gameObject.GetComponent<AudioSource>().isPlaying == false)
                {
                    this.gameObject.GetComponent<AudioSource>().UnPause();
                    stopped = false;
                }
            }
        }
        if (this.gameObject.GetComponent<AudioSource>().isPlaying == false && played == true && stopped == false)
        {
            Destroy(this.gameObject);
        }
        if (played == false)
        {
            this.gameObject.GetComponent<AudioSource>().clip = audiosource.clip;
            this.gameObject.GetComponent<AudioSource>().volume = audiosource.volume;
            this.gameObject.GetComponent<AudioSource>().maxDistance = audiosource.maxDistance;
            this.gameObject.GetComponent<AudioSource>().pitch = pitch;
            audiosource = this.gameObject.GetComponent<AudioSource>();
            audiosource.Play();
            played = true;
        }
    }
}
public enum SoundType
{
    Null,
    Factory,
    Effect,
    Music,
}
