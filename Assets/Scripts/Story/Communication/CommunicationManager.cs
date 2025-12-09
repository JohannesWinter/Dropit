using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationManager : MonoBehaviour
{
    public GameObject communicationMassageResource;
    public PlaySound soundResource;
    CommunicationMassage currentCommunication;
    public GameObject[] personProfiles;
    public AudioSource[] lines_person1;
    public AudioSource[] lines_person2;

    public AudioSource[][] allVoiceLines;

    //Test

    public bool testMessage;
    public int testPersonID;
    public int testMassageID;

    void Start()
    {
        allVoiceLines = new AudioSource[2][];
        allVoiceLines[0] = lines_person1;
        allVoiceLines[1] = lines_person2;
    }

    void Update()
    {
        if (currentCommunication != null && currentCommunication.isFinished == true)
        {
            Destroy(currentCommunication.gameObject);
            currentCommunication = null;
        }

        if (testMessage)
        {
            testMessage = false;
            AddCommunicationMassage(testPersonID, testMassageID, true);
        }
    }

    public Coroutine AddCommunicationMassage(int personID, int massageID, bool waitForMassageEnd)
    {
        return StartCoroutine(AddCommunicationMassageRoutine(personID, massageID, waitForMassageEnd));
    }
    IEnumerator AddCommunicationMassageRoutine(int personID, int massageID, bool waitForMassageEnd)
    {
        if (personID < 0)
        {
            print("Error - person <" + personID + "> does not exist");
            yield break;
        }
        while(currentCommunication != null)
        {
            if (waitForMassageEnd == false)
            {
                yield break;
            }
            yield return null;
        }
        currentCommunication = InitializeMassage(personID, massageID);
    }

    CommunicationMassage InitializeMassage(int personID, int massageID)
    {
        GameObject newMassageObject = Instantiate(communicationMassageResource);
        CommunicationMassage newMassage = newMassageObject.GetComponent<CommunicationMassage>();
        newMassage.gameObject.transform.SetParent(this.gameObject.transform);
        newMassage.personID = personID;
        newMassage.messageID = massageID;
        newMassage.personProfile = personProfiles[personID];
        PlaySound voice = Instantiate(soundResource);
        voice.gameObject.transform.parent = newMassage.gameObject.transform;
        voice.audiosource = allVoiceLines[personID][massageID];
        newMassage.messageSound = voice;
        return newMassage;
    }
}
