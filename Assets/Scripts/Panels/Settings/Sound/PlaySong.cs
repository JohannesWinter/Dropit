using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySong : MonoBehaviour
{
    public PlayMusic initiator;

    public AudioSource audiosource;
    AudioSource lastFrameSource;
    
    public float pitch = 1.0f;
    public float volume = 1.0f;

    public float currentVolumeState;
    public float fadeInTime = 1;
    public float fadeOutTime = 1;

    float waitBeforeStart;
    void Start()
    {
        audiosource= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        waitBeforeStart -= Time.unscaledDeltaTime;
        if (initiator.playing == this)
        {
            if (currentVolumeState < 1)
            {
                if (waitBeforeStart <= 0)
                {
                    currentVolumeState += Time.unscaledDeltaTime / fadeInTime;
                }
            }
            else
            {
                currentVolumeState = 1;
            }
            audiosource.volume = volume * currentVolumeState;
            audiosource.pitch = pitch;

            if (audiosource.isPlaying == false && lastFrameSource != this && waitBeforeStart <= 0)
            {
                audiosource.Play();
            }
            else if (lastFrameSource == this)
            {
                initiator.playing = null;
            }
        }
        else
        {
            if (currentVolumeState > 0)
            {
                currentVolumeState -= Time.unscaledDeltaTime / fadeOutTime;
            }
            else
            {
                if(audiosource != null)
                {
                    audiosource.Stop();
                    currentVolumeState = 0;
                    fadeInTime = 1;
                    fadeOutTime = 1;
                    bool destroy = true;
                    for (int i = 0; i < initiator.playSongs.Length; i++)
                    {
                        if (initiator.playSongs[i] == this)
                        {
                            destroy = false;
                            break;
                        }
                    }
                    if (destroy)
                        Destroy(this.gameObject);
                }
            }
            if (audiosource != null)
            {
                audiosource.volume = volume * currentVolumeState;
                audiosource.pitch = pitch;
            }
        }
    }

    public void ResetLastFrameSource()
    {
        lastFrameSource = null;
    }
    public void WaitBeforeStart(float timeInSec)
    {
        waitBeforeStart = timeInSec;
    }
}
