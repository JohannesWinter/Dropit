using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditHistoryManager : MonoBehaviour
{
    int currentPosition = -1;

    public List<EditEvent> editHistory = new List<EditEvent>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetEditHistory()
    {
        editHistory.Clear();
        currentPosition = -1;
    }

    bool LoadEvent(EditEvent toLoadEvent, bool undo)
    {
        if (currentPosition < 0)
        {
            return false;
        }
        if (toLoadEvent == null)
        {
            return false;
        }
        var allFactoryObjects = GameObject.FindGameObjectsWithTag("FactoryObject");
        if (undo)
            Manager.m.money -= toLoadEvent.moneyChange;
        else
            Manager.m.money += toLoadEvent.moneyChange;
        switch(undo ? toLoadEvent.type : toLoadEvent.type == EditEventType.Sold ? EditEventType.Bought : EditEventType.Sold) 
        {
            case EditEventType.Sold:
                {
                    GameObject recreated = (GameObject)Instantiate(Resources.Load(toLoadEvent.name));

                    recreated.transform.position = toLoadEvent.position;
                    recreated.transform.rotation = Quaternion.Euler(toLoadEvent.rotation);
                    recreated.transform.localScale = toLoadEvent.scale;

                    RepairDropper rd = recreated.GetComponent<RepairDropper>();
                    rd.durability = toLoadEvent.durability;
                    rd.id = toLoadEvent.id;
                    rd.isScrap = toLoadEvent.isScrap;
                    if (rd.isScrap)
                        recreated.transform.parent = Manager.m.scrapFolder.transform;
                    else
                        recreated.transform.parent = Manager.m.machineFolder.transform;

                    Drop drop = recreated.GetComponentInChildren<Drop>();
                    if (drop)
                    {
                        drop.stopTimeReset = true;
                        drop.currenttime = toLoadEvent.dropTimer;
                    }
                    break;
                }
            case EditEventType.Bought:
                {
                    for(int i = 0; i < allFactoryObjects.Length; i++)
                    {
                        if (allFactoryObjects[i].GetComponent<RepairDropper>() && allFactoryObjects[i].GetComponent<RepairDropper>().id == toLoadEvent.id)
                        {
                            allFactoryObjects[i].GetComponent<RepairDropper>().sold = true;
                            Destroy(allFactoryObjects[i], Time.deltaTime);
                            break;
                        }
                    }
                    break;
                }
        }
        Physics.SyncTransforms();
        return true;
    }

    public bool Undo(bool perform)
    {
        if (currentPosition < 0) return false;
        if (perform == false) return true;
        LoadEvent(editHistory[currentPosition], true);
        currentPosition--;
        return true;
    }
    public bool Redo(bool perform)
    {
        if (currentPosition + 1 >= editHistory.Count) return false;
        if (perform == false) return true;

        currentPosition++;
        LoadEvent(editHistory[currentPosition], false);
        return true;
    }


    public void AddEditEvent(RepairDropper factoryObject, EditEventType type, double moneyChange)
    {
        EditEvent editEvent = new EditEvent();
        editEvent.type = type;

        editEvent.name = factoryObject.name.Replace("(Clone)", "");
        editEvent.id = factoryObject.id;
        editEvent.durability = factoryObject.durability;
        editEvent.dropTimer = (factoryObject.dropperNumber != 0 && factoryObject.gameObject.GetComponentInChildren<Drop>())
                                ? factoryObject.gameObject.GetComponentInChildren<Drop>().currenttime
                                : 0f;
        editEvent.isScrap = factoryObject.isScrap;
        editEvent.inputOres = factoryObject.inputOres;
        editEvent.position = factoryObject.transform.position;
        editEvent.rotation = factoryObject.transform.rotation.eulerAngles;
        editEvent.scale = factoryObject.transform.localScale;

        editEvent.moneyChange = moneyChange;

        currentPosition++;
        if (currentPosition >= editHistory.Count) editHistory.Add(editEvent);
        else
        {
            editHistory.RemoveRange(currentPosition, editHistory.Count - currentPosition);
            editHistory.Add(editEvent);
        }
    }

    
}

public class EditEvent
{
    public EditEventType type;

    public string name;
    public int id;
    public float durability;
    public float dropTimer;
    public bool isScrap;
    public float[] inputOres;

    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public double moneyChange;
}
public enum EditEventType
{
    Sold,
    Bought,
}
