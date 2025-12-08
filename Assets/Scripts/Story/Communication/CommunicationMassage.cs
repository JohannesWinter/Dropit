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

    // Update is called once per frame

    private void Start()
    {
        personProfile.GetComponent<RectTransform>().localPosition = personProfilePosition.localPosition;
        personProfile.SetActive(true);
        this.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        overlay.SetActive(true);
    }
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
