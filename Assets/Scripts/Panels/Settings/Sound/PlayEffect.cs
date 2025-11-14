using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class PlayEffect: MonoBehaviour
{
    public PlaySound sound;

    public AudioSource _click;
    public AudioSource _swipe;
    public AudioSource _sell;
    public AudioSource _placeMiner;
    public AudioSource _repair1;
    public AudioSource _repair2;
    public AudioSource _repair3;
    public AudioSource _error;
    public AudioSource _accept;
    public AudioSource _install;
    public AudioSource _coins;
    public AudioSource _beep;
    public AudioSource _removeScrap;
    public AudioSource _slam;
    public AudioSource _squeakWheel;
    public AudioSource _clickLight;
    public AudioSource _humming;
    public AudioSource _barrel1;
    public AudioSource _barrel2;
    public AudioSource _railing;

    float errorTimer;
    private void Awake()
    {
        _click.volume = 0f;
        _swipe.volume = 0f;
        _placeMiner.volume = 0f;
        _repair1.volume = 0f;
        _repair2.volume = 0f;
        _repair3.volume = 0f;
        _error.volume = 0f;
        _accept.volume = 0f;
        _install.volume = 0f;
        _coins.volume = 0f;
        _sell.volume = 0f;
        _beep.volume = 0f;
        _slam.volume = 0f;
        _squeakWheel.volume = 0f;
        _clickLight.volume = 0f;
        _humming.volume = 0f;
        _barrel1.volume = 0f;
        _barrel2.volume = 0f;
        _railing.volume = 0f;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public PlaySound click()
    {
        _click.volume = Manager.m.effectsVolume.publicVolume;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _click;
        return s;
    }
    public PlaySound swipe()
    {
        _swipe.volume = Manager.m.effectsVolume.publicVolume;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _swipe;
        return s;
    }
    public PlaySound sell()
    {
        _sell.volume = Manager.m.effectsVolume.publicVolume;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _sell;
        return s;
    }
    public PlaySound placeMiner()
    {
        _placeMiner.volume = Manager.m.effectsVolume.publicVolume * 0.1f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _placeMiner;
        return s;
    }
    public PlaySound repair()
    {
        int random = UnityEngine.Random.Range(1, 4);
        if (random == 1)
        {
            _repair1.volume =  Manager.m.effectsVolume.publicVolume * 0.4f;
            PlaySound s = Instantiate(sound, transform.position, transform.rotation);
            s.transform.parent = Manager.m.soundFolder.transform;
            s.audiosource = _repair1;
            return s;
        }
        if (random == 2)
        {
            _repair2.volume = Manager.m.effectsVolume.publicVolume * 0.4f;
            PlaySound s = Instantiate(sound, transform.position, transform.rotation);
            s.transform.parent = Manager.m.soundFolder.transform;
            s.audiosource = _repair2;
            return s;
        }
        if (random == 3)
        {
            _repair3.volume = Manager.m.effectsVolume.publicVolume * 0.5f;
            PlaySound s = Instantiate(sound, transform.position, transform.rotation);
            s.transform.parent = Manager.m.soundFolder.transform;
            s.audiosource = _repair3;
            return s;
        }
        return null;
    }
    public PlaySound error()
    {
        if (errorTimer < Time.unscaledTime)
        {
            errorTimer = Time.unscaledTime + 0.2f;
            _error.volume = Manager.m.effectsVolume.publicVolume;
            PlaySound s = Instantiate(sound, transform.position, transform.rotation);
            s.transform.parent = Manager.m.soundFolder.transform;
            s.audiosource = _error;
            return s;
        }
        return null;
    }
    public PlaySound accept()
    {
        _accept.volume = Manager.m.effectsVolume.publicVolume * 0.6f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _accept;
        return s;
    }
    public PlaySound install()
    {
        _install.volume = Manager.m.effectsVolume.publicVolume;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _install;
        return s;
    }
    public PlaySound coins()
    {
        _coins.volume = Manager.m.effectsVolume.publicVolume;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _coins;
        return s;
    }
    public PlaySound beep()
    {
        _beep.volume = Manager.m.effectsVolume.publicVolume * 0.8f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _beep;
        return s;
    }
    public PlaySound removeScrap()
    {
        _removeScrap.volume = Manager.m.effectsVolume.publicVolume * 0.8f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _removeScrap;
        return s;
    }

    public PlaySound changePlaySoundParameters(Func<PlaySound> playSoundFunction, float pitch)
    {
        PlaySound ps = playSoundFunction();
        if (pitch != 0)
        {
            ps.pitch = pitch;
        }
        return ps;
    }
    public PlaySound slam()
    {
        _slam.volume = Manager.m.effectsVolume.publicVolume * 0.6f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _slam;
        return s;
    }
    public PlaySound squeakWheel()
    {
        _squeakWheel.volume = Manager.m.effectsVolume.publicVolume * 0.1f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _squeakWheel;
        return s;
    }
    public PlaySound clickLight()
    {
        _clickLight.volume = Manager.m.effectsVolume.publicVolume * 0.3f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _clickLight;
        return s;
    }
    public PlaySound humming()
    {
        _humming.volume = Manager.m.effectsVolume.publicVolume * 1f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _humming;
        return s;
    }
    public PlaySound barrel1()
    {
        _barrel1.volume = Manager.m.effectsVolume.publicVolume * 0.3f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _barrel1;
        return s;
    }
    public PlaySound barrel2()
    {
        _barrel2.volume = Manager.m.effectsVolume.publicVolume * 0.3f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _barrel2;
        return s;
    }
    public PlaySound railing()
    {
        _railing.volume = Manager.m.effectsVolume.publicVolume * 0.8f;
        PlaySound s = Instantiate(sound, transform.position, transform.rotation);
        s.transform.parent = Manager.m.soundFolder.transform;
        s.audiosource = _railing;
        return s;
    }
}
