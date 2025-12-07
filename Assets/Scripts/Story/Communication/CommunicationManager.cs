using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationManager : MonoBehaviour
{
    CommunicationMassage currentCommunication;
    public GameObject[] persons;
    public AudioSource[] lines_person1;
    public AudioSource[] lines_person2;

    public AudioSource[][] allLines;
    // Start is called before the first frame update
    void Start()
    {
        allLines = new AudioSource[1][];
        allLines[0] = lines_person1;
        allLines[1] = lines_person2;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        

        return null;
    }
}
