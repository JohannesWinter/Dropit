//using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Analytics;

public class SetObject : MonoBehaviour
{
    GameObject ConveyorBelt1;
    GameObject ConveyorBelt1Blueprint;
    GameObject ConveyorBelt1Right;
    GameObject ConveyorBelt1RightBlueprint;
    GameObject ConveyorBelt1Left;
    GameObject ConveyorBelt1LeftBlueprint;
    GameObject ConveyorBelt1Fuse;
    GameObject ConveyorBelt1FuseBlueprint;
    GameObject ConveyorBelt1Split;
    GameObject ConveyorBelt1SplitBlueprint;
    GameObject ConveyorBelt2;
    GameObject ConveyorBelt2Blueprint;
    GameObject ConveyorBelt2Right;
    GameObject ConveyorBelt2RightBlueprint;
    GameObject ConveyorBelt2Left;
    GameObject ConveyorBelt2LeftBlueprint;
    GameObject ConveyorBelt2Fuse;
    GameObject ConveyorBelt2FuseBlueprint;
    GameObject ConveyorBelt2Split;
    GameObject ConveyorBelt2SplitBlueprint;
    GameObject ConveyorBelt3;
    GameObject ConveyorBelt3Blueprint;
    GameObject ConveyorBelt3Right;
    GameObject ConveyorBelt3RightBlueprint;
    GameObject ConveyorBelt3Left;
    GameObject ConveyorBelt3LeftBlueprint;
    GameObject ConveyorBelt3Fuse;
    GameObject ConveyorBelt3FuseBlueprint;
    GameObject ConveyorBelt3Split;
    GameObject ConveyorBelt3SplitBlueprint;

    GameObject Upgrader1;
    GameObject Upgrader1Blueprint;
    GameObject Upgrader2;
    GameObject Upgrader2Blueprint;
    GameObject Upgrader3;
    GameObject Upgrader3Blueprint;

    GameObject Furnace1;
    GameObject Furnace1Blueprint;
    GameObject Furnace2;
    GameObject Furnace2Blueprint;
    GameObject Furnace3;
    GameObject Furnace3Blueprint;

    GameObject Dropper1;
    GameObject Dropper1Blueprint;
    GameObject Dropper2;
    GameObject Dropper2Blueprint;
    GameObject Dropper3;
    GameObject Dropper3Blueprint;
    GameObject Dropper4;
    GameObject Dropper4Blueprint;
    GameObject Dropper5;
    GameObject Dropper5Blueprint;
    GameObject Dropper6;
    GameObject Dropper6Blueprint;
    GameObject Dropper7;
    GameObject Dropper7Blueprint;
    GameObject Dropper8;
    GameObject Dropper8Blueprint;
    GameObject Dropper9;
    GameObject Dropper9Blueprint;
    GameObject Dropper10;
    GameObject Dropper10Blueprint;

    GameObject i;
    //List<GameObject> bluePrintList;
    GameObject j;
    bool OnField = false;
    double mouseCheck = 0;
    double mouseCheckDelayed = 0;
    //bool EmptyField = true;
    public bool allowPlace = false;
    //public float Rotation = 0;
    public Quaternion QRotation;
    Collider OtherObject;
    bool triggered;

    void Start()
    {
        //bluePrintList = new List<GameObject>();
        ConveyorBelt1 = Manager.m.ConveyorBelt1;
        ConveyorBelt1Blueprint = Manager.m.ConveyorBelt1Blueprint;
        ConveyorBelt1Left = Manager.m.ConveyorBelt1Left;
        ConveyorBelt1LeftBlueprint = Manager.m.ConveyorBelt1LeftBlueprint;
        ConveyorBelt1Right = Manager.m.ConveyorBelt1Right;
        ConveyorBelt1RightBlueprint = Manager.m.ConveyorBelt1RightBlueprint;
        ConveyorBelt1Fuse = Manager.m.ConveyorBelt1Fuse;
        ConveyorBelt1FuseBlueprint = Manager.m.ConveyorBelt1FuseBlueprint;
        ConveyorBelt1Split = Manager.m.ConveyorBelt1Split;
        ConveyorBelt1SplitBlueprint = Manager.m.ConveyorBelt1SplitBlueprint;

        ConveyorBelt2 = Manager.m.ConveyorBelt2;
        ConveyorBelt2Blueprint = Manager.m.ConveyorBelt2Blueprint;
        ConveyorBelt2Left = Manager.m.ConveyorBelt2Left;
        ConveyorBelt2LeftBlueprint = Manager.m.ConveyorBelt2LeftBlueprint;
        ConveyorBelt2Right = Manager.m.ConveyorBelt2Right;
        ConveyorBelt2RightBlueprint = Manager.m.ConveyorBelt2RightBlueprint;
        ConveyorBelt2Fuse = Manager.m.ConveyorBelt2Fuse;
        ConveyorBelt2FuseBlueprint = Manager.m.ConveyorBelt2FuseBlueprint;
        ConveyorBelt2Split = Manager.m.ConveyorBelt2Split;
        ConveyorBelt2SplitBlueprint = Manager.m.ConveyorBelt2SplitBlueprint;

        ConveyorBelt3 = Manager.m.ConveyorBelt3;
        ConveyorBelt3Blueprint = Manager.m.ConveyorBelt3Blueprint;
        ConveyorBelt3Left = Manager.m.ConveyorBelt3Left;
        ConveyorBelt3LeftBlueprint = Manager.m.ConveyorBelt3LeftBlueprint;
        ConveyorBelt3Right = Manager.m.ConveyorBelt3Right;
        ConveyorBelt3RightBlueprint = Manager.m.ConveyorBelt3RightBlueprint;
        ConveyorBelt3Fuse = Manager.m.ConveyorBelt3Fuse;
        ConveyorBelt3FuseBlueprint = Manager.m.ConveyorBelt3FuseBlueprint;
        ConveyorBelt3Split = Manager.m.ConveyorBelt3Split;
        ConveyorBelt3SplitBlueprint = Manager.m.ConveyorBelt3SplitBlueprint;

        Furnace1 = Manager.m.Furnace1;
        Furnace1Blueprint = Manager.m.Furnace1Blueprint;
        Furnace2 = Manager.m.Furnace2;
        Furnace2Blueprint = Manager.m.Furnace2Blueprint;
        Furnace3 = Manager.m.Furnace3;
        Furnace3Blueprint = Manager.m.Furnace3Blueprint;

        Upgrader1 = Manager.m.Upgrader1;
        Upgrader1Blueprint = Manager.m.Upgrader1Blueprint;
        Upgrader2 = Manager.m.Upgrader2;
        Upgrader2Blueprint = Manager.m.Upgrader2Blueprint;
        Upgrader3 = Manager.m.Upgrader3;
        Upgrader3Blueprint = Manager.m.Upgrader3Blueprint;

        Dropper1 = Manager.m.Dropper1;
        Dropper1Blueprint = Manager.m.Dropper1Blueprint;
        Dropper2 = Manager.m.Dropper2;
        Dropper2Blueprint = Manager.m.Dropper2Blueprint;
        Dropper3 = Manager.m.Dropper3;
        Dropper3Blueprint = Manager.m.Dropper3Blueprint;
        Dropper4 = Manager.m.Dropper4;
        Dropper4Blueprint = Manager.m.Dropper4Blueprint;
        Dropper5 = Manager.m.Dropper5;
        Dropper5Blueprint = Manager.m.Dropper5Blueprint;
        Dropper6 = Manager.m.Dropper6;
        Dropper6Blueprint = Manager.m.Dropper6Blueprint;
        Dropper7 = Manager.m.Dropper7;
        Dropper7Blueprint = Manager.m.Dropper7Blueprint;
        Dropper8 = Manager.m.Dropper8;
        Dropper8Blueprint = Manager.m.Dropper8Blueprint;
        Dropper9 = Manager.m.Dropper9;
        Dropper9Blueprint = Manager.m.Dropper9Blueprint;
        Dropper10 = Manager.m.Dropper10;
        Dropper10Blueprint = Manager.m.Dropper10Blueprint;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ore>() == true)
        {
            Destroy(collision.gameObject);
        }
    }
    private void OnMouseOver()
    {
        OnField = true;

        mouseCheck++;
        if(mouseCheck > 1000000000)
        {
            mouseCheck = 0;
        }
    }
    private void OnMouseEnter()
    {
        if ((Manager.m.editMode_placeDropper == true && Manager.m.inSettings == false && Manager.m.inMarket == false && Manager.m.inFactoryHalls == false && Manager.m.chain.activeSelf == false))
        {
            Physics.SyncTransforms();
            Collider[] hits = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 2, transform.rotation);
            foreach (Collider hit in hits)
            {
                if (hit.gameObject.tag == "FactoryObject" || hit.gameObject.tag == "Wall")
                {
                    return;
                }
            }
            if (i != null)
            {
                Destroy(i);
            }
            switch (Manager.m.dropperNumber)
            {
                case 1:
                    {
                        i = Instantiate(Dropper1Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 6, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation - 90, 0);
                        i.transform.localScale = new Vector3(2f, 2f, 2f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 2:
                    {
                        i = Instantiate(Dropper2Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 6, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation - 90, 0);
                        i.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 3:
                    {
                        i = Instantiate(Dropper3Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 6, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation - 180, 0);
                        i.transform.localScale = new Vector3(2f, 2f, 2f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 4:
                    {
                        i = Instantiate(Dropper4Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 8, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation - 90, 0);
                        i.transform.localScale = new Vector3(4f, 4f, 4f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 5:
                    {
                        i = Instantiate(Dropper5Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 6, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation - 90, 0);
                        i.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 6:
                    {
                        i = Instantiate(Dropper6Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 6, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation - 90, 0);
                        i.transform.localScale = new Vector3(2.7f, 2.7f, 2.7f);
                        i.transform.Translate(0, 2f, 0);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 7:
                    {
                        i = Instantiate(Dropper7Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 5.2f, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation - 180, 0);
                        i.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 8:
                    {
                        i = Instantiate(Dropper8Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 7, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation - 180, 0);
                        i.transform.localScale = new Vector3(2.1f, 2f, 2.1f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 9:
                    {
                        i = Instantiate(Dropper9Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 5.2f, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                        i.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 10:
                    {
                        i = Instantiate(Dropper10Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 5.2f, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                        i.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
            }
            //bluePrintList.Add(i);
            //i.layer = 3;
            //i.GetComponent<BoxCollider>().enabled = false;
            //i.GetComponent<Drop>().enabled = false;
            //i.GetComponent<RepairDropper>().enabled = false;

        }
        if ((Manager.m.editMode_placeMachine == true && Manager.m.inSettings == false && Manager.m.inMarket == false && Manager.m.inFactoryHalls == false && Manager.m.chain.activeSelf == false))
        {
            Physics.SyncTransforms();
            Collider[] hits = Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 2, transform.rotation);
            foreach (Collider hit in hits)
            {
                if (hit.gameObject.tag == "FactoryObject" || hit.gameObject.tag == "Wall")
                {
                    return;
                }
            }
            if (i != null)
            {
                Destroy(i);
            }
            switch (Manager.m.machineNumber)
            {
                case 1:
                    {
                        if (Manager.m.objectType == "Straight")
                        {
                            i = Instantiate(ConveyorBelt1Blueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Right")
                        {
                            i = Instantiate(ConveyorBelt1RightBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Left")
                        {
                            i = Instantiate(ConveyorBelt1LeftBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Fuse")
                        {
                            i = Instantiate(ConveyorBelt1FuseBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Split")
                        {
                            i = Instantiate(ConveyorBelt1SplitBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        break;
                    }
                case 2:
                    {
                        i = Instantiate(Furnace1Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 5.2f, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                        i.transform.localScale = new Vector3(4f, 4f, 4f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 3:
                    {
                        i = Instantiate(Upgrader1Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 3.8f, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                        i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 4:
                    {
                        if (Manager.m.objectType == "Straight")
                        {
                            i = Instantiate(ConveyorBelt2Blueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Right")
                        {
                            i = Instantiate(ConveyorBelt2RightBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Left")
                        {
                            i = Instantiate(ConveyorBelt2LeftBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Fuse")
                        {
                            i = Instantiate(ConveyorBelt2FuseBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Split")
                        {
                            i = Instantiate(ConveyorBelt2SplitBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        break;
                    }
                case 5:
                    {
                        i = Instantiate(Furnace2Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 5.2f, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                        i.transform.localScale = new Vector3(4f, 4f, 4f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 6:
                    {
                        i = Instantiate(Upgrader2Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 5.2f, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                        i.transform.localScale = new Vector3(3.7f, 3.7f, 3.7f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 7:
                    {
                        if (Manager.m.objectType == "Straight")
                        {
                            i = Instantiate(ConveyorBelt3Blueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Right")
                        {
                            i = Instantiate(ConveyorBelt3RightBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Left")
                        {
                            i = Instantiate(ConveyorBelt3LeftBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Fuse")
                        {
                            i = Instantiate(ConveyorBelt3FuseBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        if (Manager.m.objectType == "Split")
                        {
                            i = Instantiate(ConveyorBelt3SplitBlueprint, transform.position, transform.rotation);
                            i.transform.Translate(0, 3.9f, 0);
                            i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                            i.transform.localScale = new Vector3(6f, 5.2f, 6f);
                            MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                            for (int i = 0; i < Meshs.Length; i++)
                            {
                                Meshs[i].enabled = false;
                            }
                            break;
                        }
                        break;
                    }
                case 8:
                    {
                        i = Instantiate(Furnace3Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 5.8f, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                        i.transform.localScale = new Vector3(4f, 4f, 4f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 9:
                    {
                        i = Instantiate(Upgrader3Blueprint, transform.position, transform.rotation);
                        i.transform.Translate(0, 4f, 0);
                        i.transform.Rotate(0, Manager.m.dropperRotation, 0);
                        i.transform.localScale = new Vector3(4, 4f, 4f);
                        MeshRenderer[] Meshs = i.GetComponentsInChildren<MeshRenderer>();
                        for (int i = 0; i < Meshs.Length; i++)
                        {
                            Meshs[i].enabled = false;
                        }
                        break;
                    }
                case 10:
                    {
                        break;
                    }
            }
            //bluePrintList.Add(i);
        }
    }
    private void OnMouseDown()
    {

    }
    private void OnMouseExit()
    {
        //if (Manager.m.placeDropper == true || Manager.m.placeMachine == true)
        //{
        //    try
        //    {
        //        Destroy(i);
        //        //OnField = false;
        //    }
        //    catch { }
        //}
    }
    // Start is called before the first frame update

// Update is called once per frame
async void Update()
    {
        if (i != null)
        {
            if (OnField == false)
            {
                Destroy(i);
                //bluePrintList.Remove(i);
            }
        }
        if (triggered == true && !OtherObject)
        {
            allowPlace = false;
            triggered = false;
        }
        if (mouseCheckDelayed == mouseCheck && OnField == true)
        {
            Destroy(i);
            //bluePrintList.Remove(i);
            OnField = false;
            //delayCheck += 1;
        }
        if (Manager.m.frameCounter % 5 == 0)
        {

        }
        mouseCheckDelayed = mouseCheck;
        if (i != null)
        {
            if (Manager.m.editMode_placeMachine == true)
            {
                i.transform.rotation = Quaternion.Euler(0, Manager.m.dropperRotation, 0);
            }
            if (Manager.m.editMode_placeDropper == true)
            {
                i.transform.rotation = Quaternion.Euler(0, Manager.m.dropperRotation - 90, 0);
            }
        }
        if (Manager.m.editMode_placeDropper == false && Manager.m.editMode_placeMachine == false)
        {
            Destroy(i);
        }
        if ((OnField == true) && (
            GameInputManager.GetKeyDown(Manager.m.ActionKey("RotateLeft")) || 
            GameInputManager.GetKeyDown(Manager.m.ActionKey("RotateRight")) ||
            GameInputManager.GetKeyDown(Manager.m.ActionKey("Transform")) ||
            GameInputManager.GetKeyDown(Manager.m.ActionKey("SetTransform1")) ||
            GameInputManager.GetKeyDown(Manager.m.ActionKey("SetTransform2")) ||
            GameInputManager.GetKeyDown(Manager.m.ActionKey("SetTransform3")) ||
            GameInputManager.GetKeyDown(Manager.m.ActionKey("SetTransform4")) ||
            GameInputManager.GetKeyDown(Manager.m.ActionKey("SetTransform5"))))
        {
            Destroy(i);
            await Task.Delay((int)Mathf.Ceil(Time.unscaledDeltaTime * 1000 * 1f));
            OnMouseEnter();
        }
        if (Input.GetButton("ClickLeft"))
        {
            if (OnField == true && i != null)
            {
                OnField = false;
                allowPlace = i.GetComponent<RepairDropper>().canBePlaced;
                if (Manager.m.editMode_placeDropper == true && allowPlace == true)
                {
                    switch (Manager.m.dropperNumber)
                    {
                        case 1:
                            {
                                double cost = Dropper1.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMinersNumber == 1)
                                {
                                    cost = cost * Manager.m.qTECheapMiners;
                                }
                                else if (Manager.m.qTEExpensiveMinersNumber == 1)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMiners;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.placeMiner();
                                    j = Instantiate(Dropper1, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money = Manager.m.money - cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 2:
                            {
                                double cost = Dropper2.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMinersNumber == 2)
                                {
                                    cost = cost * Manager.m.qTECheapMiners;
                                }
                                else if (Manager.m.qTEExpensiveMinersNumber == 2)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMiners;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.placeMiner();
                                    j = Instantiate(Dropper2, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 3:
                            {
                                double cost = Dropper3.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMinersNumber == 3)
                                {
                                    cost = cost * Manager.m.qTECheapMiners;
                                }
                                else if (Manager.m.qTEExpensiveMinersNumber == 3)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMiners;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.placeMiner();
                                    j = Instantiate(Dropper3, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 4:
                            {
                                double cost = Dropper4.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMinersNumber == 4)
                                {
                                    cost = cost * Manager.m.qTECheapMiners;
                                }
                                else if (Manager.m.qTEExpensiveMinersNumber == 4)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMiners;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.placeMiner();
                                    j = Instantiate(Dropper4, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 5:
                            {
                                double cost = Dropper5.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMinersNumber == 5)
                                {
                                    cost = cost * Manager.m.qTECheapMiners;
                                }
                                else if (Manager.m.qTEExpensiveMinersNumber == 5)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMiners;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.placeMiner();
                                    j = Instantiate(Dropper5, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 6:
                            {
                                double cost = Dropper6.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMinersNumber == 6)
                                {
                                    cost = cost * Manager.m.qTECheapMiners;
                                }
                                else if (Manager.m.qTEExpensiveMinersNumber == 6)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMiners;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.placeMiner();
                                    j = Instantiate(Dropper6, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 7:
                            {
                                double cost = Dropper7.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMinersNumber == 7)
                                {
                                    cost = cost * Manager.m.qTECheapMiners;
                                }
                                else if (Manager.m.qTEExpensiveMinersNumber == 7)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMiners;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.placeMiner();
                                    j = Instantiate(Dropper7, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 8:
                            {
                                double cost = Dropper8.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMinersNumber == 8)
                                {
                                    cost = cost * Manager.m.qTECheapMiners;
                                }
                                else if (Manager.m.qTEExpensiveMinersNumber == 8)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMiners;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.placeMiner();
                                    j = Instantiate(Dropper8, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 9:
                            {
                                double cost = Dropper9.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMinersNumber == 9)
                                {
                                    cost = cost * Manager.m.qTECheapMiners;
                                }
                                else if (Manager.m.qTEExpensiveMinersNumber == 9)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMiners;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.placeMiner();
                                    j = Instantiate(Dropper9, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 10:
                            {
                                double cost = Dropper10.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMinersNumber == 10)
                                {
                                    cost = cost * Manager.m.qTECheapMiners;
                                }
                                else if (Manager.m.qTEExpensiveMinersNumber == 10)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMiners;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.placeMiner();
                                    j = Instantiate(Dropper10, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                    }
                }

                if (Manager.m.editMode_placeMachine == true && allowPlace == true)
                {
                    switch (Manager.m.machineNumber)
                    {
                        case 1:
                            {
                                double cost = ConveyorBelt1.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMachinesNumber == 1)
                                {
                                    cost = cost * Manager.m.qTECheapMachines;
                                }
                                else if (Manager.m.qTEExpensiveMachinesNumber == 1)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMachines;
                                }
                                if (Manager.m.objectType == "Straight")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt1, i.transform.position, i.transform.rotation);
                                        //j.GetComponent<Förderband>().Richtung = Rotation;
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Right")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt1Right, i.transform.position, i.transform.rotation);
                                        //j.GetComponent<Förderband>().Richtung = Rotation;
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Left")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt1Left, i.transform.position, i.transform.rotation);
                                        //j.GetComponent<Förderband>().Richtung = Rotation;
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Fuse")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt1Fuse, i.transform.position, i.transform.rotation);
                                        //j.GetComponent<Förderband>().Richtung = Rotation;
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Split")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt1Split, i.transform.position, i.transform.rotation);
                                        //j.GetComponent<Förderband>().Richtung = Rotation;
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                break;
                            }
                        case 2:
                            {
                                double cost = Furnace1.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMachinesNumber == 2)
                                {
                                    cost = cost * Manager.m.qTECheapMachines;
                                }
                                else if (Manager.m.qTEExpensiveMachinesNumber == 2)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMachines;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.install();
                                    j = Instantiate(Furnace1, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 3:
                            {
                                double cost = Upgrader1.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMachinesNumber == 3)
                                {
                                    cost = cost * Manager.m.qTECheapMachines;
                                }
                                else if (Manager.m.qTEExpensiveMachinesNumber == 3)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMachines;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.install();
                                    j = Instantiate(Upgrader1, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 4:
                            {
                                double cost = ConveyorBelt2.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMachinesNumber == 4)
                                {
                                    cost = cost * Manager.m.qTECheapMachines;
                                }
                                else if (Manager.m.qTEExpensiveMachinesNumber == 4)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMachines;
                                }
                                if (Manager.m.objectType == "Straight")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt2, i.transform.position, i.transform.rotation);
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Right")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt2Right, i.transform.position, i.transform.rotation);
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Left")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt2Left, i.transform.position, i.transform.rotation);
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Fuse")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt2Fuse, i.transform.position, i.transform.rotation);
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Split")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt2Split, i.transform.position, i.transform.rotation);
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                break;
                            }
                        case 5:
                            {
                                double cost = Furnace2.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMachinesNumber == 5)
                                {
                                    cost = cost * Manager.m.qTECheapMachines;
                                }
                                else if (Manager.m.qTEExpensiveMachinesNumber == 5)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMachines;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.install();
                                    j = Instantiate(Furnace2, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 6:
                            {
                                double cost = Upgrader2.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMachinesNumber == 6)
                                {
                                    cost = cost * Manager.m.qTECheapMachines;
                                }
                                else if (Manager.m.qTEExpensiveMachinesNumber == 6)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMachines;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.install();
                                    j = Instantiate(Upgrader2, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 7:
                            {
                                double cost = ConveyorBelt3.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMachinesNumber == 7)
                                {
                                    cost = cost * Manager.m.qTECheapMachines;
                                }
                                else if (Manager.m.qTEExpensiveMachinesNumber == 7)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMachines;
                                }
                                if (Manager.m.objectType == "Straight")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt3, i.transform.position, i.transform.rotation);
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Right")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt3Right, i.transform.position, i.transform.rotation);
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Left")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt3Left, i.transform.position, i.transform.rotation);
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Fuse")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt3Fuse, i.transform.position, i.transform.rotation);
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                else if (Manager.m.objectType == "Split")
                                {
                                    if (Manager.m.money >= cost)
                                    {
                                        Manager.m.effectSpeaker.install();
                                        j = Instantiate(ConveyorBelt3Split, i.transform.position, i.transform.rotation);
                                        j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                        j.transform.parent = Manager.m.machineFolder.transform;
                                        Manager.m.money -= cost;
                                        Manager.m.currentCost = cost;
                                        Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                        Destroy(i);
                                    }
                                    else
                                    {
                                        Manager.m.effectSpeaker.error();
                                    }
                                    break;
                                }
                                break;
                            }
                        case 8:
                            {
                                double cost = Furnace3.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMachinesNumber == 8)
                                {
                                    cost = cost * Manager.m.qTECheapMachines;
                                }
                                else if (Manager.m.qTEExpensiveMachinesNumber == 8)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMachines;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.install();
                                    j = Instantiate(Furnace3, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 9:
                            {
                                double cost = Upgrader3.GetComponent<RepairDropper>().cost;
                                if (Manager.m.qTECheapMachinesNumber == 9)
                                {
                                    cost = cost * Manager.m.qTECheapMachines;
                                }
                                else if (Manager.m.qTEExpensiveMachinesNumber == 9)
                                {
                                    cost = cost * Manager.m.qTEExpensiveMachines;
                                }
                                if (Manager.m.money >= cost)
                                {
                                    Manager.m.effectSpeaker.install();
                                    j = Instantiate(Upgrader3, i.transform.position, i.transform.rotation);
                                    j.transform.localScale = new Vector3(i.transform.localScale.x, i.transform.localScale.y, i.transform.localScale.z);
                                    j.transform.parent = Manager.m.machineFolder.transform;
                                    Manager.m.money -= cost;
                                    Manager.m.currentCost = cost;
                                    Manager.m.editHistoryManager.AddEditEvent(j.GetComponent<RepairDropper>(), EditEventType.Bought, -cost);
                                    Destroy(i);
                                }
                                else
                                {
                                    Manager.m.effectSpeaker.error();
                                }
                                break;
                            }
                        case 10:
                            {
                             break;
                            }   
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Blueprint")
        {
            triggered = true;
            OtherObject = other;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Blueprint")
        {
            allowPlace = other.gameObject.GetComponent<RepairDropper>().canBePlaced;
        }
    }
    private void OnTriggerExit(Collider other)
    {

    }
}