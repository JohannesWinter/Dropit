using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationMassage : MonoBehaviour
{
    public int personID;
    public int messageID;
    public PlaySound messageSound;
    public GameObject personProfile;
    public bool isFinished;

    public GameObject overlay;
    public RectTransform personProfilePosition;
    void Start()
    {
        personProfile.GetComponent<RectTransform>().localPosition = personProfilePosition.localPosition;
        personProfile.SetActive(true);
        overlay.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
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
