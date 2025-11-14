using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public GameObject notificationObject;
    public Transform notificationFolder;
    public List<Notification> notificationQueue = new List<Notification>();
    public List<Vector3> startPositions;
    public List<Vector3> endPositions;

    public bool test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = notificationQueue.Count - 1; i >= 0; i--)
        {
            if (startPositions.Count > i)
            {
                if (notificationQueue[i].state == 0)
                {
                    notificationQueue[i].state = 1;
                    notificationQueue[i].enable = true;
                    notificationQueue[i].Slide(to: endPositions[i], 3f, from: startPositions[i]);
                }
                if (notificationQueue[i].state == 1)
                {
                    notificationQueue[i].enable = true;
                    notificationQueue[i].Slide(to: endPositions[i], 3f);
                }
                if (notificationQueue[i].elapsed > notificationQueue[i].lifetime && notificationQueue[i].state == 1)
                {
                    notificationQueue[i].Slide(to:startPositions[i], 4f);
                    notificationQueue[i].state = 2;
                }
                if (notificationQueue[i].state == 2 && Vector3.Distance(notificationQueue[i].gameObject.transform.localPosition, notificationQueue[i].slideTarget) < 50)
                {
                    notificationQueue[i].state = 3;
                    Destroy(notificationQueue[i].gameObject, 3f);
                    notificationQueue.RemoveAt(i);
                }

            }
        }
        if (test)
        {
            test = false;
            AddNotification("LoremIpsum", null);
        }
    }

    public void AddNotification(string description, Texture image)
    {
        Notification notification = Instantiate(notificationObject).GetComponent<Notification>();
        notification.gameObject.transform.SetParent(notificationFolder);
        notification.enable = false;
        notification.state = 0;
        notification.notificationText.text = description;
        notification.notificationImage.texture = image;
        notification.lifetime = 5f;
        notificationQueue.Add(notification);
    }
}

