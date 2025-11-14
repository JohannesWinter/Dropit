using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieCamera : MonoBehaviour
{
    public Camera movieCam;
    public GameObject camObject;
    public Vector3 currentPosition;
    public Vector3 currentRotation;
    public Vector3 aimPosition;
    public Vector3 aimRotation;
    public Vector3 changePosition;
    public Vector3 changeRotation;
    public float speed;
    public float rotSpeed;
    public bool move = false;
    public bool applyChange = false;
    public bool setChange0 = false;
    // Start is called before the first frame update
    void Start()
    {
        currentPosition = camObject.transform.position;
        currentRotation = camObject.transform.rotation.eulerAngles;
        aimPosition = currentPosition;
        aimRotation = currentRotation;
        changePosition = new Vector3(0, 0, 0);
        changeRotation = new Vector3(0, 0, 0);
        speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        camObject.transform.position = currentPosition;
        camObject.transform.rotation = Quaternion.Euler(currentRotation);
        if ((Vector3.Magnitude(changePosition) != 0 || Vector3.Magnitude(changeRotation) != 0) && applyChange == true)
        {
            applyChange = false;
            aimPosition = currentPosition + changePosition;
            aimRotation = currentRotation + changeRotation;
            aimPosition = new Vector3(0, 0, 0);
            aimRotation = new Vector3(0, 0, 0);
        }
        float positionChangeRatio = 0;
        if (currentPosition != aimPosition && move == true)
        {
            Vector3 direction = aimPosition - currentPosition;
            Vector3 changePerFrame = direction * (1f / direction.magnitude) * Time.deltaTime * speed;
            currentPosition += changePerFrame;
            if (direction.magnitude < changePerFrame.magnitude)
            {
                currentPosition = aimPosition;
            }
            positionChangeRatio = changePerFrame.magnitude / direction.magnitude;
        }
        if (currentRotation != aimRotation && move == true)
        {
            Vector3 direction = aimRotation - currentRotation;
            Vector3 changePerFrame;

            if (rotSpeed != 0)
            {
                changePerFrame = direction * (1f / direction.magnitude) * Time.deltaTime * rotSpeed;
            }
            else
            {
                changePerFrame = direction * positionChangeRatio;
            }
            currentRotation += changePerFrame;
            if (direction.magnitude < changePerFrame.magnitude)
            {
                currentRotation = aimRotation;
            }
        }

        if (Manager.m.useMovieCam == true)
        {
            movieCam.enabled = true;
        }
        else
        {
            movieCam.enabled = false;
        }
        if (setChange0 == true)
        {
            setChange0 = false;
            aimPosition = currentPosition;
            aimRotation = currentRotation;
        }
    }
}
