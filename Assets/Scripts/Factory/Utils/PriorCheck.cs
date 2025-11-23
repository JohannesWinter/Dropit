using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorCheck : MonoBehaviour
{
    public BoxCollider wall;
    int enableWallFor = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enableWallFor <= 0)
        {
            wall.enabled = false;
            enableWallFor = 0;
        }
        else
        {
            enableWallFor--;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "FactoryObject")
        {
            if (other.gameObject.GetComponent<RepairDropper>().conveyorBeltSpeed != 0)
            {
                if (other.gameObject.GetComponent<RepairDropper>().conveyorBeltType == "Right")
                {
                    if (Manager.standardRotation(other.gameObject.transform.rotation.eulerAngles.y) != Manager.standardRotation(this.gameObject.transform.eulerAngles.y + 90))
                    {
                        print("WallRight");
                        wall.enabled = true;
                        enableWallFor = 2;
                    }
                }
                else if (other.gameObject.GetComponent<RepairDropper>().conveyorBeltType == "Left")
                {
                    if (Manager.standardRotation(other.gameObject.transform.rotation.eulerAngles.y) != Manager.standardRotation(this.gameObject.transform.eulerAngles.y - 90))
                    {
                        print("WallLeft");
                        wall.enabled = true;
                        enableWallFor = 2;
                    }
                }
                else if (other.gameObject.GetComponent<RepairDropper>().conveyorBeltType == "Fuse")
                {
                    if (Manager.standardRotation(other.gameObject.transform.rotation.eulerAngles.y) == Manager.standardRotation(this.gameObject.transform.rotation.eulerAngles.y + 180))
                    {
                        print("WallFuse");
                        wall.enabled = true;
                        enableWallFor = 2;
                    }
                }
                else if (other.gameObject.GetComponent<RepairDropper>().conveyorBeltType == "Split" || other.gameObject.GetComponent<RepairDropper>().conveyorBeltType == "Straight")
                {
                    if (Manager.standardRotation(other.gameObject.transform.rotation.eulerAngles.y) != Manager.standardRotation(this.gameObject.transform.rotation.eulerAngles.y))
                    {
                        print("WallStraight: " + Manager.standardRotation(other.gameObject.transform.rotation.eulerAngles.y) + " - " + Manager.standardRotation(this.gameObject.transform.rotation.eulerAngles.y));
                        wall.enabled = true;
                        enableWallFor = 2;
                    }
                }
            }
            else if (other.gameObject.GetComponent<RepairDropper>().furnaceMultiplier != 0 && other.gameObject.GetComponent<RepairDropper>().machineNumber != 2)
            {
                if (Manager.standardRotation(other.gameObject.transform.rotation.eulerAngles.y) != Manager.standardRotation(this.gameObject.transform.rotation.eulerAngles.y + 180))
                {
                    print("WallFurnace");
                    wall.enabled = true;
                    enableWallFor = 2;
                }
            }
            else
            {
                //NoWall
            }
        }
    }
}
