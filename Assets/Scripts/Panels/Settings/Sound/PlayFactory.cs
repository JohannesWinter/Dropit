using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayFactory : MonoBehaviour
{
    public PlaySound sound;

    public AudioSource _dropper1;
    public AudioSource _dropper2;
    public AudioSource _dropper3;
    public AudioSource _dropper4;
    public AudioSource _dropper5;
    public AudioSource _dropper6;
    public AudioSource _dropper7;
    public AudioSource _dropper8;
    public AudioSource _dropper9;
    public AudioSource _dropper10;

    public AudioSource _sell;
    public AudioSource _missionAdd;
    public AudioSource _upgrade;

    public AudioSource _step_1;
    public AudioSource _laugth_1;
    public AudioSource _laugth_2;
    public AudioSource _laugth_3;
    public AudioSource _attack_1;

    public AudioSource _destroy;
    public AudioSource _destroyFarAway;

    bool playSell;
    float playSellTime;

    bool playUpgrade;
    float playUpgradeTime;

    bool playMissionAdd;
    float playMissionAddTime;

    bool playdropper1;
    bool playdropper2;
    bool playdropper3;
    bool playdropper4;
    bool playdropper5;
    bool playdropper6;
    bool playdropper7;
    bool playdropper8;
    bool playdropper9;
    bool playdropper10;

    float dullFactorySounds = 1;

    float playdropper1Time;
    float playdropper2Time;
    float playdropper3Time;
    float playdropper4Time;
    float playdropper5Time;
    float playdropper6Time;
    float playdropper7Time;
    float playdropper8Time;
    float playdropper9Time;
    float playdropper10Time;
    // Start is called before the first frame update
    void Start()
    {
        playSell = true;
        playUpgrade = true;
        playSellTime = Time.time;
        playUpgradeTime = Time.time;

        playdropper1 = true;
        playdropper2 = true;
        playdropper3 = true;
        playdropper4 = true;
        playdropper5 = true;
        playdropper6 = true;
        playdropper7 = true;
        playdropper8 = true;
        playdropper9 = true;
        playdropper10 = true;

        playdropper1Time = Time.time;
        playdropper2Time = Time.time;
        playdropper3Time = Time.time;
        playdropper4Time = Time.time;
        playdropper5Time = Time.time;
        playdropper6Time = Time.time;
        playdropper7Time = Time.time;
        playdropper8Time = Time.time;
        playdropper9Time = Time.time;
        playdropper10Time = Time.time;
    }
    private void Awake()
    {
        _dropper1.volume = 0f;
        _dropper2.volume = 0f;
        _dropper3.volume = 0f;
        _dropper4.volume = 0f;
        _dropper5.volume = 0f;
        _dropper6.volume = 0f;
        _dropper7.volume = 0f;
        _dropper8.volume = 0f;
        _dropper9.volume = 0f;
        _dropper10.volume = 0f;
        _sell.volume = 0f;
        _upgrade.volume = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Manager.m.qTEBrokenLights || Manager.m.qTEUltimateWipeout)
        {
            dullFactorySounds = 0.1f;
        }
        else
        {
            dullFactorySounds = 1;
        }
        if (playSellTime + 0.1f < Time.time)
        {
            playSell = true;
        }
        if (playMissionAddTime + 0.1f < Time.time)
        {
            playMissionAdd = true;
        }
        if (playUpgradeTime + 0.1f < Time.time)
        {
            playUpgrade = true;
        }
        if (playdropper1Time + 0.1f < Time.time) { playdropper1 = true;}
        if (playdropper2Time + 0.1f < Time.time) { playdropper2 = true;}
        if (playdropper3Time + 0.1f < Time.time) { playdropper3 = true;}
        if (playdropper4Time + 0.1f < Time.time) { playdropper4 = true; }
        if (playdropper5Time + 0.1f < Time.time) { playdropper5 = true; }
        if (playdropper6Time + 0.1f < Time.time) { playdropper6 = true; }
        if (playdropper7Time + 0.1f < Time.time) { playdropper7 = true; }
        if (playdropper8Time + 0.1f < Time.time) { playdropper8 = true; }
        if (playdropper9Time + 0.1f < Time.time) { playdropper9 = true; }
        if (playdropper10Time + 0.1f < Time.time) { playdropper10 = true; }

        if (playdropper1 || playdropper2 || playdropper3 || playdropper4 || playdropper5 || playdropper6 || playdropper7 || playdropper8 || playdropper9 || playdropper10)
        {
            //Get rid of warnings that playdropper would not be used
        }
    }

    public void dropper1(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playdropper1 == true)
            {
                playdropper1 = false;
                playdropper1Time = Time.time;
                _dropper1.volume = Manager.m.factoryVolume.publicVolume * 1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _dropper1;
                s.soundtype = SoundType.Factory;
            }
        }
    }
    public void dropper2(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playdropper2 == true)
            {
                playdropper2 = false;
                playdropper2Time = Time.time;
                _dropper2.volume = Manager.m.factoryVolume.publicVolume * 1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _dropper2;
                s.soundtype = SoundType.Factory;
            }
        }
    }
    public void dropper3(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playdropper3 == true)
            {
                playdropper3 = false;
                playdropper3Time = Time.time;
                _dropper3.volume = Manager.m.factoryVolume.publicVolume * 1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _dropper3;
                s.soundtype = SoundType.Factory;
            }
        }
    }
    public void dropper4(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playdropper4 == true)
            {
                playdropper4 = false;
                playdropper4Time = Time.time;
                _dropper4.volume = Manager.m.factoryVolume.publicVolume * 1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _dropper4;
                s.pitch = 1.6f;
                s.soundtype = SoundType.Factory;
            }
        }
    }
    public void dropper5(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playdropper5 == true)
            {
                playdropper5 = false;
                playdropper5Time = Time.time;
                _dropper5.volume = Manager.m.factoryVolume.publicVolume * 1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _dropper5;
                s.soundtype = SoundType.Factory;
            }
        }
    }
    public void dropper6(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playdropper6 == true)
            {
                playdropper6 = false;
                playdropper6Time = Time.time;
                _dropper6.volume = Manager.m.factoryVolume.publicVolume * 1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _dropper6;
                s.soundtype = SoundType.Factory;
            }
        }
    }
    public void dropper7(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playdropper7 == true)
            {
                playdropper7 = false;
                playdropper7Time = Time.time;
                _dropper7.volume = Manager.m.factoryVolume.publicVolume * 1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _dropper7;
                s.soundtype = SoundType.Factory;
            }
        }
    }
    public void dropper8(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playdropper8 == true)
            {
                playdropper8 = false;
                playdropper8Time = Time.time;
                _dropper8.volume = Manager.m.factoryVolume.publicVolume * 1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _dropper8;
                s.soundtype = SoundType.Factory;
            }
        }
    }
    public void dropper9(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playdropper9 == true)
            {
                playdropper9 = false;
                playdropper9Time = Time.time;
                _dropper9.volume = Manager.m.factoryVolume.publicVolume * 1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _dropper9;
                s.soundtype = SoundType.Factory;
            }
        }
    }
    public void dropper10(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playdropper10 == true)
            {
                playdropper10 = false;
                playdropper10Time = Time.time;
                _dropper10.volume = Manager.m.factoryVolume.publicVolume * 1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _dropper10;
                s.soundtype = SoundType.Factory;
            }
        }
    }
    public void sell(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playSell == true)
            {
                playSell = false;
                playSellTime = Time.time;
                _sell.volume = Manager.m.factoryVolume.publicVolume * 0.5f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _sell;
            }
        }
    }
    public void missionAdd(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playMissionAdd == true)
            {
                playMissionAdd = false;
                playMissionAddTime = Time.time;
                _missionAdd.volume = Manager.m.factoryVolume.publicVolume * 0.5f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _missionAdd;
            }
        }
    }
    public void upgrade(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            if (playUpgrade == true)
            {
                playUpgrade = false;
                playUpgradeTime = Time.time;
                _upgrade.volume = Manager.m.factoryVolume.publicVolume * 0.1f * dullFactorySounds;
                PlaySound s = Instantiate(sound, transform.position, transform.rotation);
                s.transform.parent = Manager.m.soundFolder.transform;
                s.audiosource = _upgrade;
                s.pitch = Random.Range(0.8f, 1.2f);
            }
        }
    }
    public void step(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            PlaySound s = Instantiate(sound, transform.position, transform.rotation);
            _step_1.volume = Manager.m.factoryVolume.publicVolume * 0.5f;
            s.transform.parent = Manager.m.soundFolder.transform;
            s.audiosource = _step_1;
            s.pitch = Random.Range(0.8f, 1.2f);
            s.soundtype = SoundType.Factory;
        }
    }
    public void laught(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            PlaySound s = Instantiate(sound, transform.position, transform.rotation);
            _laugth_1.volume = Manager.m.factoryVolume.publicVolume * 0.5f;
            _laugth_2.volume = Manager.m.factoryVolume.publicVolume * 0.5f;
            _laugth_3.volume = Manager.m.factoryVolume.publicVolume * 0.5f;
            s.transform.parent = Manager.m.soundFolder.transform;
            switch (Random.Range(0, 3))
            {
                case 0:
                    {
                        s.audiosource = _laugth_1;
                        break;
                    }
                case 1:
                    {
                        s.audiosource = _laugth_2;
                        break;
                    }
                case 2:
                    {
                        s.audiosource = _laugth_3;
                        break;
                    }
            }
            s.pitch = Random.Range(0.7f, 1.5f);
            s.soundtype = SoundType.Factory;
        }
    }
    public void attack(Camera c)
    {
        if (c == Manager.m.lastDropperCamera)
        {
            PlaySound s = Instantiate(sound, transform.position, transform.rotation);
            _attack_1.volume = Manager.m.factoryVolume.publicVolume * 0.5f;
            s.transform.parent = Manager.m.soundFolder.transform;
            s.audiosource = _attack_1;
            s.pitch = Random.Range(0.8f, 1.2f);
            s.soundtype = SoundType.Factory;
        }
    }
    public void destroy(Camera c)
    {
        if (c == Manager.m.lastDropperCamera && Manager.m.inShopDropper == false && Manager.m.inShopMachine == false)
        {
            PlaySound s = Instantiate(sound, transform.position, transform.rotation);
            _destroy.volume = Manager.m.factoryVolume.publicVolume * 0.5f;
            s.transform.parent = Manager.m.soundFolder.transform;
            s.audiosource = _destroy;
            s.pitch = Random.Range(0.9f, 1.1f);
            s.soundtype = SoundType.Factory;
        }
        else
        {
            PlaySound s = Instantiate(sound, transform.position, transform.rotation);
            _destroyFarAway.volume = Manager.m.factoryVolume.publicVolume * 0.5f;
            s.transform.parent = Manager.m.soundFolder.transform;
            s.audiosource = _destroyFarAway;
            s.pitch = Random.Range(0.8f, 1.1f);
            s.soundtype = SoundType.Factory;
        }
    }
}
