using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayMusic : MonoBehaviour
{
    public PlaySong playing;

    public AudioSource[] allMusic;
    public PlaySong[] playSongs;
    public PlaySong pS;
    public Transform pSFolder;

    public float[] allMusicMaxVolume;
    public float stoppedTime = 0;

    public string musicType = "normal";

    public bool enableMusic = true;

    public Button skipSong;
    void Start()
    {
        playSongs = new PlaySong[allMusic.Length];
        stoppedTime = Time.unscaledTime;
        skipSong.onClick.AddListener(SkipCurrentSong);
    }

    // Update is called once per frame
    void Update()
    {

        if (playing == null && stoppedTime <= Time.unscaledTime)
        {
            playing = getMusicOfType(musicType);
        }
        if (enableMusic == true)
        {
            for (int i = 0; i < playSongs.Length; i++)
            {
                if (playSongs[i] != null)
                    playSongs[i].volume = Manager.m.musicVolume.publicVolume * allMusicMaxVolume[i];
            }
        }
        else
        {
            for (int i = 0; i < playSongs.Length; i++)
            {
                if (playSongs[i] != null)
                {
                    playSongs[i].volume = 0;
                }
            }
        }

        if (GameInputManager.GetKeyDown(Manager.m.ActionKey("SkipMusic")))
        {
            SkipCurrentSong();
        }
    }

    public void ChangeMusic(float fadeInTime, float fadeOutTime, string musicType, float stopTimeFor)
    {
        if (playing != null)
        {
            playing.fadeOutTime = fadeOutTime;
        }
        playing = getMusicOfType(musicType);
        this.musicType = musicType;
        playing.fadeInTime = fadeInTime;
        playing.WaitBeforeStart(stopTimeFor + fadeOutTime);
    }

    public void StopMusic(float time)
    {
        playing = null;
        stoppedTime = Time.unscaledTime + time;
    }

    void SkipCurrentSong()
    {
        if (musicType == "normal")
        {
            StopMusic(0.5f);
        }
    }

    public PlaySong getMusicOfType(string type)
    {
        PlaySong newPlaySong = Instantiate(pS);
        newPlaySong.gameObject.transform.SetParent(pSFolder);
        int position = 0;
        if (type == "normal")
        {
            position = Random.Range(0, 11);
        }
        else if (type == "scary1")
        {
            position = Random.Range(12, 14);
        }
        else if (type == "scary2")
        {
            position = Random.Range(14, 16);
        }
        else if (type == "credits")
        {
            position = Random.Range(11,12);
        }
        addAudioSource(newPlaySong.gameObject, position);
        playSongs[position] = newPlaySong;
        playSongs[position].initiator = this;
        playSongs[position].enabled = true;
        return newPlaySong;
    }

    public AudioSource addAudioSource(GameObject gameObject, int position)
    {
        AudioSource source = allMusic[position];
        AudioSource newSource = gameObject.AddComponent<AudioSource>();

        newSource.clip = source.clip;
        newSource.outputAudioMixerGroup = source.outputAudioMixerGroup;
        newSource.volume = source.volume;
        newSource.pitch = source.pitch;
        newSource.loop = false;
        newSource.spatialBlend = source.spatialBlend;
        newSource.playOnAwake = source.playOnAwake;
        newSource.priority = source.priority;
        newSource.dopplerLevel = source.dopplerLevel;
        newSource.rolloffMode = source.rolloffMode;
        newSource.minDistance = source.minDistance;
        newSource.maxDistance = source.maxDistance;

        return newSource;
    }
}
