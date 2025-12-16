using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommunicationMassage : MonoBehaviour
{
    public int personID;
    public int messageID;
    public PlaySound messageSound;
    public GameObject personProfile;
    public TextMeshProUGUI personName;
    public bool isFinished;

    public GameObject overlay;
    public RectTransform personProfilePosition;
    public NoiseVolumeUI noiseVolumeUI;
    public float amplitudeMultiplier;
    public float amplitudeRandomizer;

    // Update is called once per frame

    private void Start()
    {
        personProfile.GetComponent<RectTransform>().localPosition = personProfilePosition.localPosition;
        personProfile.SetActive(true);
        this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        personName.text = personProfile.gameObject.name; // gameobject has to have name of person
        noiseVolumeUI.trackSound = messageSound;
        noiseVolumeUI.amplitudeMultiplier = amplitudeMultiplier;
        noiseVolumeUI.amplitudeRandomizer = amplitudeRandomizer;
        overlay.SetActive(true);
    }
    void Update()
    {
        float guiScale = Manager.m.graphicManager.gUIScaleFactor;
        overlay.GetComponent<RectTransform>().localScale = new Vector3(guiScale, guiScale, guiScale);
        if (messageSound == null)
        {
            isFinished = true;
            overlay.SetActive(false);
        }
        else
        {
            messageSound.GetComponent<AudioSource>().volume = Manager.m.voiceVolume.publicVolume;
        }
    }
}
