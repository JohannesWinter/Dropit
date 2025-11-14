using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public int upgradeLevel;
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.GetComponent<Ore>() == true)
        //{
        //    if (gameObject.transform.parent.parent)
        //    {
        //        if (other.gameObject.GetComponent<Ore>().upgradeLevel < upgradeLevel && gameObject.transform.parent.GetComponentInParent<RepairDropper>().working == true)
        //        {
        //            Manager.m.factorySpeaker.upgrade(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);
        //            other.gameObject.GetComponent<Ore>().upgradeLevel += 1;
        //        }
        //    }
        //    else
        //    {
        //        if (other.gameObject.GetComponent<Ore>().upgradeLevel < upgradeLevel && gameObject.GetComponentInParent<RepairDropper>().working == true)
        //        {
        //            Manager.m.factorySpeaker.upgrade(this.gameObject.GetComponentInParent<RepairDropper>().nextCam);
        //            other.gameObject.GetComponent<Ore>().upgradeLevel += 1;
        //        }
        //    }
        //}
    }
    private void Start()
    {
        if (gameObject.transform.parent.parent)
        {
            upgradeLevel = gameObject.transform.parent.gameObject.GetComponentInParent<RepairDropper>().upgradeLevelMax;
        }
        else
        {
            upgradeLevel = gameObject.transform.GetComponentInParent<RepairDropper>().upgradeLevelMax;
        }
    }
}
