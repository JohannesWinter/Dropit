using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DirectionButton : MonoBehaviour
{
    public GameObject Arrows;
    public GameObject StraightArrow;
    public GameObject RightArrow;
    public GameObject LeftArrow;
    public GameObject FuseArrow;
    public GameObject SplitArrow;
    float editRotation;
    // Start is called before the first frame update
    void Start()
    {
        RightArrow.SetActive(false);
        LeftArrow.SetActive(false);
        FuseArrow.SetActive(false);
        SplitArrow.SetActive(false);
        Manager.m.objectType = "Straight";
    }

    // Update is called once per frame
    void Update()
    {
        Arrows.transform.rotation = Quaternion.Euler(0, Manager.m.dropperRotation + 180 + editRotation, 0);
    }

    private void OnMouseEnter()
    {
        StraightArrow.transform.Translate(0, -0.2f, 0);
        RightArrow.transform.Translate(0, -0.2f, 0);
        LeftArrow.transform.Translate(0, -0.2f, 0);
        FuseArrow.transform.Translate(0, -0.2f, 0);
        SplitArrow.transform.Translate(0, -0.2f, 0);
    }

    private void OnMouseExit()
    {
        StraightArrow.transform.Translate(0, 0.2f, 0);
        RightArrow.transform.Translate(0, 0.2f, 0);
        LeftArrow.transform.Translate(0, 0.2f, 0);
        FuseArrow.transform.Translate(0, 0.2f, 0);
        SplitArrow.transform.Translate(0, 0.2f, 0);
    }

    private void OnMouseDown()
    {
        if (StraightArrow.activeSelf == true)
        {
            StraightArrow.SetActive(false);
            RightArrow.SetActive(true);
            LeftArrow.SetActive(false);
            FuseArrow.SetActive(false);
            SplitArrow.SetActive(false);
            Manager.m.objectType = "Right";
            editRotation = -90;
        }
        else if (RightArrow.activeSelf == true)
        {
            StraightArrow.SetActive(false);
            RightArrow.SetActive(false);
            LeftArrow.SetActive(true);
            FuseArrow.SetActive(false);
            SplitArrow.SetActive(false);
            Manager.m.objectType = "Left";
            editRotation = 90;
        }
        else if (LeftArrow.activeSelf == true)
        {
            StraightArrow.SetActive(false);
            RightArrow.SetActive(false);
            LeftArrow.SetActive(false);
            FuseArrow.SetActive(true);
            SplitArrow.SetActive(false);
            Manager.m.objectType = "Fuse";
            editRotation = 0;
        }
        else if (FuseArrow.activeSelf == true)
        {
            StraightArrow.SetActive(false);
            RightArrow.SetActive(false);
            LeftArrow.SetActive(false);
            FuseArrow.SetActive(false);
            SplitArrow.SetActive(true);
            Manager.m.objectType = "Split";
            editRotation = 0;
        }
        else
        {
            StraightArrow.SetActive(true);
            RightArrow.SetActive(false);
            LeftArrow.SetActive(false);
            FuseArrow.SetActive(false);
            SplitArrow.SetActive(false);
            Manager.m.objectType = "Straight";
            editRotation = 0;
        }

    }
}
