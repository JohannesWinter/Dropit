using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationManager : MonoBehaviour
{
    public CommunicationMassage communicationMassageResource;
    public PlaySound soundResource;
    CommunicationMassage currentCommunication;
    public GameObject[] personProfiles;
    public AudioSource[] lines_person1;
    public AudioSource[] lines_person2;

    public AudioSource[][] allVoiceLines;
    // Start is called before the first frame update
    void Start()
    {
        allVoiceLines = new AudioSource[1][];
        allVoiceLines[0] = lines_person1;
        allVoiceLines[1] = lines_person2;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: update all Voice line volumes when chaned
        if (currentCommunication != null && currentCommunication.isFinished == true)
        {
            Destroy(currentCommunication);
            currentCommunication = null;
        }
    }

    public IEnumerator AddCommunicationMassage(int personID, int massageID)
    {
        if (personID < 0)
        {
            print("Error - person <" + personID + "> does not exist");
            yield break;
        }
        while(currentCommunication != null)
        {
            yield return null;
        }
        currentCommunication = InitializeMassage(personID, massageID);
    }

    CommunicationMassage InitializeMassage(int personID, int massageID)
    {
        CommunicationMassage newMassage = Instantiate(communicationMassageResource);
        newMassage.gameObject.transform.parent = this.gameObject.transform;
        newMassage.personID = personID;
        newMassage.messageID = massageID;
        newMassage.personProfile = personProfiles[personID];
        PlaySound voice = Instantiate(soundResource);
        voice.gameObject.transform.parent = newMassage.gameObject.transform;
        voice.audiosource = allVoiceLines[personID][massageID];
        return newMassage;
    }
}
