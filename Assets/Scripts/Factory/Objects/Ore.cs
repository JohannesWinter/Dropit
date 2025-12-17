using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Subsystems;

public class Ore : MonoBehaviour
{
    static int currentID = 1;
    int oreID;
    public int oreNumber;
    public double value;
    public double baseValue;
    public double exp;
    public int upgradeLevel;
    public double upgradeMultiplyer;
    //public double marketMultiplier;
    Light halo;
    double currenttime;
    double currenttime2;
    public bool isDestroyed;
    public bool moveableByBelt;
    public bool isFacade;
    float antiLagMult;
    float loadInTime;
    List<Upgrade> upgraded = new List<Upgrade>();
    public List<int> visitedBelts;
    Rigidbody rb;
    public bool doDropAnimation;
    public bool inDropAnimation;
    public Vector2 dropAnimationDirection;
    public GameObject oreMesh;
    // Start is called before the first frame update
    private void Awake()
    {
        visitedBelts = new List<int>();
        baseValue = value;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        loadInTime = Time.time + Time.deltaTime * 10;
        moveableByBelt = true;  
        upgradeMultiplyer = 0;

        halo = gameObject.GetComponent<Light>();
        antiLagMult = 1;
        oreID = getOreID();
    }
    // Update is called once per frame
    void Update()
    {
        if (Manager.m.paused == false)
        {
            if (doDropAnimation && inDropAnimation == false)
            {
                doDropAnimation = false;
                StartCoroutine(DropAnimation());
            }
            if (Manager.m.frameCounter % 10 == 0)
            {
                if (isFacade == true)
                {
                    value = 0;
                }
                upgradeMultiplyer = 1 + 0.5 * upgradeLevel;
                if (moveableByBelt == true)
                {
                    currenttime2 = Time.time;
                }
                if (currenttime2 + 5 < Time.time)
                {
                    Destroy(this, 10f);
                    isDestroyed = true;
                    gameObject.SetActive(false);
                }
                try
                {
                    if (upgradeLevel == 0 || Manager.m.graphicManager.enableHaloState == GraphicQuality.Off)
                    {
                        halo.enabled = false;
                    }
                    else
                    {
                        GraphicQuality haloQuality = Manager.m.graphicManager.enableHaloState;

                        if (haloQuality == GraphicQuality.All)
                        {
                            if (upgradeLevel == 1)
                            {
                                halo.color = new Color(0, 100, 0);
                                halo.range = 2f;
                                halo.enabled = true;
                                halo.intensity = 0.3f;
                            }
                            else if (upgradeLevel == 2)
                            {
                                halo.color = new Color(0, 0, 120);
                                halo.range = 2.5f;
                                halo.enabled = true;
                                halo.intensity = 0.3f;
                            }
                            else if (upgradeLevel == 3)
                            {
                                halo.color = new Color(140, 0, 200);
                                halo.range = 3f;
                                halo.enabled = true;
                                halo.intensity = 0.3f;
                            }
                        }
                        else if (haloQuality == GraphicQuality.Some)
                        {
                            if (upgradeLevel == 1)
                            {
                                halo.color = new Color(0, 100, 0);
                                halo.range = 1.5f;
                                halo.enabled = true;
                                halo.intensity = 0.3f;
                            }
                            else if (upgradeLevel == 2)
                            {
                                halo.color = new Color(0, 0, 120);
                                halo.range = 1.5f;
                                halo.enabled = true;
                                halo.intensity = 0.3f;
                            }
                            else if (upgradeLevel == 3)
                            {
                                halo.color = new Color(140, 0, 200);
                                halo.range = 1.5f;
                                halo.enabled = true;
                                halo.intensity = 0.3f;
                            }
                        }
                    }
                }
                catch { }
            }
            if (isDestroyed == true)
            {
                gameObject.tag = "Destroyed";
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Upgrade>() && Time.time > loadInTime)
        {
            Upgrade upgradeComponent = other.gameObject.GetComponent<Upgrade>();
            if (upgradeComponent.upgradeLevel > upgradeLevel && upgradeComponent.gameObject.GetComponentInParent<RepairDropper>().working == true && upgraded.IndexOf(upgradeComponent) == -1)
            {
                upgradeLevel++;
                Manager.m.factorySpeaker.upgrade(upgradeComponent.gameObject.GetComponentInParent<RepairDropper>().nextCam);
                upgraded.Add(upgradeComponent);
            }
        }
        if (other.gameObject.tag == "BeltSide")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "ConveyorBelt" || other.gameObject.tag == "ConveyorBeltRight" || other.gameObject.tag == "ConveyorBeltLeft" || other.gameObject.tag == "ConveyorBeltFuse" || other.gameObject.tag == "ConveyorBeltSplit")
        {
            if (visitedBelts.Contains(other.gameObject.GetComponentInParent<RepairDropper>().id) == false)
            {
                if (Manager.m.qTEBelts > 0) //normally 0
                {
                    value += baseValue * Manager.m.qTEBelts; //absolute increase
                }
                else if (Manager.m.qTEBelts < 0)
                {
                    value += value * Manager.m.qTEBelts; //reladive decrease
                }
                if (value < 0)
                {
                    Destroy(this, 10f);
                    isDestroyed = true;
                    gameObject.SetActive(false);
                }
                visitedBelts.Add(other.gameObject.GetComponentInParent<RepairDropper>().id);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Manager.m.paused == false)
        {
            if (Manager.m.frameCounter % antiLagMult == 0)
            {
                try
                {
                    if (Time.time > currenttime && moveableByBelt == true && Time.time > loadInTime)
                    {
                        if (other.gameObject.tag == "ConveyorBelt" || other.gameObject.tag == "ConveyorBeltRight" || other.gameObject.tag == "ConveyorBeltLeft" || other.gameObject.tag == "ConveyorBeltFuse" || other.gameObject.tag == "ConveyorBeltSplit")
                        {
                            RepairDropper repairDropperComponent = other.GetComponentInParent<RepairDropper>();
                            switch (other.gameObject.tag)
                            {
                                case "ConveyorBelt":
                                    {
                                        Vector3 forward = new Vector3(0, 0, repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts);
                                        forward = other.transform.TransformDirection(forward);
                                        forward = gameObject.transform.InverseTransformDirection(forward);
                                        gameObject.transform.Translate(forward * Time.deltaTime * antiLagMult);
                                        currenttime = Time.time + 0.05 * antiLagMult;
                                        break;
                                    }
                                case "ConveyorBeltLeft":
                                    {
                                        Vector3 forward = new Vector3(0, 0, repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts);
                                        forward = other.transform.TransformDirection(forward);
                                        forward = gameObject.transform.InverseTransformDirection(forward);
                                        gameObject.transform.Translate(forward * Time.deltaTime * antiLagMult);
                                        currenttime = Time.time + 0.05 * antiLagMult;
                                        switch (other.gameObject.transform.rotation.eulerAngles.y)
                                        {
                                            case 0:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).x;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).x;
                                                    if (orePosition - conveyorBeltPosition < 2.5)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (orePosition - conveyorBeltPosition > 3)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts * 0.5f, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }
                                            case 90:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).z;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).z;
                                                    if (conveyorBeltPosition - orePosition < 2.5)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition > 3)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts * 0.5f, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }
                                            case 180:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).x;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).x;
                                                    if (conveyorBeltPosition - orePosition < 2.5)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition > 3)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts * 0.5f, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }
                                            case 270:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).z;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).z;
                                                    if (conveyorBeltPosition - orePosition > -2.5)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition < -3)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts * 0.5f, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }

                                        }
                                        break;
                                    }
                                case "ConveyorBeltRight":
                                    {
                                        Vector3 forward = new Vector3(0, 0, repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts);
                                        forward = other.transform.TransformDirection(forward);
                                        forward = gameObject.transform.InverseTransformDirection(forward);
                                        gameObject.transform.Translate(forward * Time.deltaTime * antiLagMult);
                                        currenttime = Time.time + 0.05 * antiLagMult;

                                        switch (other.gameObject.transform.rotation.eulerAngles.y)
                                        {
                                            case 0:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).x;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).x;
                                                    if (conveyorBeltPosition - orePosition < 2.5)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition > 3)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts * 0.5f, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }
                                            case 90:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).z;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).z;
                                                    if (conveyorBeltPosition - orePosition > -2.5)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition < -3)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts * 0.5f, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }
                                            case 180:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).x;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).x;
                                                    if (conveyorBeltPosition - orePosition > -2.5)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition < -3)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts * 0.5f, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }
                                            case 270:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).z;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).z;
                                                    if (conveyorBeltPosition - orePosition < 2.5)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition > 3)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts * 0.5f, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }

                                        }
                                        break;
                                    }
                                case "ConveyorBeltFuse":
                                    {
                                        Vector3 forward = new Vector3(0, 0, repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts);
                                        forward = other.transform.TransformDirection(forward);
                                        forward = gameObject.transform.InverseTransformDirection(forward);
                                        gameObject.transform.Translate(forward * Time.deltaTime * antiLagMult);
                                        currenttime = Time.time + 0.05 * antiLagMult;

                                        switch (other.gameObject.transform.rotation.eulerAngles.y)
                                        {
                                            case 0:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).x;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).x;
                                                    if (conveyorBeltPosition - orePosition < -0.5)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition > 0.5)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }
                                            case 90:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).z;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).z;
                                                    if (conveyorBeltPosition - orePosition < -0.5)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition > 0.5)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }
                                            case 180:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).x;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).x;
                                                    if (conveyorBeltPosition - orePosition < -0.5)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition > 0.5)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }
                                            case 270:
                                                {
                                                    double conveyorBeltPosition = other.transform.TransformPoint(Vector3.zero).z;
                                                    double orePosition = gameObject.transform.TransformPoint(Vector3.zero).z;
                                                    if (conveyorBeltPosition - orePosition < -0.5)
                                                    {
                                                        Vector3 sideward = new Vector3(-repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    if (conveyorBeltPosition - orePosition > 0.5)
                                                    {
                                                        Vector3 sideward = new Vector3(repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts, 0, 0);
                                                        sideward = other.transform.TransformDirection(sideward);
                                                        sideward = gameObject.transform.InverseTransformDirection(sideward);
                                                        gameObject.transform.Translate(sideward * Time.deltaTime * antiLagMult);
                                                    }
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                case "ConveyorBeltSplit":
                                    {
                                        Vector3 forward = new Vector3(0, 0, repairDropperComponent.conveyorBeltSpeed * Manager.m.qTEBrokenBelts);
                                        forward = other.transform.TransformDirection(forward);
                                        forward = gameObject.transform.InverseTransformDirection(forward);
                                        gameObject.transform.Translate(forward * Time.deltaTime * antiLagMult);
                                        currenttime = Time.time + 0.05 * antiLagMult;
                                        break;
                                    }
                            }
                        }
                    }
                }
                catch { }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "EntityCollider")
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), collision.collider, true);
        }
    }

    IEnumerator DropAnimation()
    {
        inDropAnimation = true;
        switch (this.oreNumber)
        {
            case 0:
                {
                    rb.isKinematic = true;
                    gameObject.transform.Translate(dropAnimationDirection.x * 6.5f, 0, dropAnimationDirection.y * 6.5f, Space.World);
                    oreMesh.transform.Translate(dropAnimationDirection.x * -6.5f, 0, dropAnimationDirection.y * -6.5f, Space.World);
                    float travelSpeed = 8f;
                    Vector3 oldPosition;
                    Vector3 newPosition;
                    do
                    {
                        oldPosition = oreMesh.transform.position;
                        oreMesh.transform.Translate((gameObject.transform.position - oreMesh.transform.position).normalized * Time.deltaTime * travelSpeed, Space.World);
                        newPosition = oreMesh.transform.position;
                        yield return null;
                    }
                    while (Vector3.Distance(oldPosition, gameObject.transform.position) > Vector3.Distance(newPosition, gameObject.transform.position));
                    oreMesh.transform.position = gameObject.transform.position;
                    rb.isKinematic = false;
                    break;
                }
            case 1:
                {
                    break;
                }
            case 2:
                {
                    oreMesh.transform.position = gameObject.transform.position;
                    rb.AddForce(UnityEngine.Random.Range(2f, 3f) * new Vector3(dropAnimationDirection.x, 0, dropAnimationDirection.y), ForceMode.Impulse);
                    break;
                }
            case 3:
                {
                    rb.isKinematic = true;
                    gameObject.transform.Translate(dropAnimationDirection.x * 9f, 0, dropAnimationDirection.y * 9f, Space.World);
                    oreMesh.transform.Translate(dropAnimationDirection.x * -9f, 0, dropAnimationDirection.y * -9f, Space.World);
                    float travelSpeed = 9f;
                    Vector3 oldPosition;
                    Vector3 newPosition;
                    do
                    {
                        oldPosition = oreMesh.transform.position;
                        oreMesh.transform.Translate((gameObject.transform.position - oreMesh.transform.position).normalized * Time.deltaTime * travelSpeed, Space.World);
                        newPosition = oreMesh.transform.position;
                        yield return null;
                    }
                    while (Vector3.Distance(oldPosition, gameObject.transform.position) > Vector3.Distance(newPosition, gameObject.transform.position));
                    oreMesh.transform.position = gameObject.transform.position;
                    rb.isKinematic = false;
                    break;
                }
            case 4:
                {
                    break;
                }
            case 5:
                {
                    break;
                }
            case 6:
                {
                    break;
                }
            case 7:
                {
                    break;
                }
            case 8:
                {
                    rb.isKinematic = true;
                    gameObject.transform.Translate(dropAnimationDirection.x * 5f, 0, dropAnimationDirection.y * 5f, Space.World);
                    oreMesh.transform.Translate(dropAnimationDirection.x * -5f, 0, dropAnimationDirection.y * -5f, Space.World);
                    float travelSpeed = 9f;
                    Vector3 oldPosition;
                    Vector3 newPosition;
                    do
                    {
                        oldPosition = oreMesh.transform.position;
                        oreMesh.transform.Translate((gameObject.transform.position - oreMesh.transform.position).normalized * Time.deltaTime * travelSpeed, Space.World);
                        newPosition = oreMesh.transform.position;
                        yield return null;
                    }
                    while (Vector3.Distance(oldPosition, gameObject.transform.position) > Vector3.Distance(newPosition, gameObject.transform.position));
                    oreMesh.transform.position = gameObject.transform.position;
                    rb.isKinematic = false;
                    break;
                }
            case 9:
                {
                    rb.isKinematic = false;
                    oreMesh.transform.position = gameObject.transform.position;
                    gameObject.transform.Translate(dropAnimationDirection.x * -1, 0, dropAnimationDirection.y * -1, Space.World);
                    rb.AddForce(new Vector3(dropAnimationDirection.x * 3, 26, dropAnimationDirection.y * 3), ForceMode.Impulse);
                    rb.AddTorque(new Vector3(dropAnimationDirection.y * 200, 0, dropAnimationDirection.x * -200), ForceMode.Force);
                    break;
                }
            default:
                {
                    Debug.Log("Unknown Ore Number: " + oreNumber);
                    break;
                }
        }
        yield return null;
        inDropAnimation = false;
    }

    private int getOreID()
    {
        int id = currentID;
        currentID++;
        return id;
    }
}
