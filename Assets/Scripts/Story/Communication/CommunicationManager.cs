using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationManager : MonoBehaviour
{
    CommunicationMassage currentCommunication;
    // Start is called before the first frame update
    void Start()
    {
        
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
