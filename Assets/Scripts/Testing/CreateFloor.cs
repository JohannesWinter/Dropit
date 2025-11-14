using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFloor : MonoBehaviour
{
    public GameObject[] floorObjects;
    public float density;
    public GameObject floorObjectFolder;
    public Vector3 startFloor;
    public Vector3 endFloor;
    public float minSize;
    public float maxSize;

    float floorSize;
    int objectQuantity;

    // Start is called before the first frame update
    void Start()
    {
        floorSize = Mathf.Abs((startFloor.x - endFloor.x) * (startFloor.z - endFloor.z));
        objectQuantity = Mathf.RoundToInt(floorSize * density);

        for (int i = 0; i < objectQuantity; i++)
        {
            GameObject floorObject = Instantiate(floorObjects[Random.Range(0, floorObjects.Length - 1)]);
            float xPos = startFloor.x + Random.Range(0, endFloor.x - startFloor.x);
            float yPos = startFloor.y + Random.Range(0, (endFloor.y - startFloor.y) * 1000) / 1000;
            float zPos = startFloor.z + Random.Range(0, endFloor.z - startFloor.z);

            float size = minSize + Random.Range(0, (maxSize - minSize) * 10) / 10;

            floorObject.transform.position = new Vector3(xPos, yPos, zPos);
            floorObject.transform.localScale = new Vector3(size, floorObject.transform.localScale.y, size);
            floorObject.SetActive(true);
            floorObject.transform.SetParent(floorObjectFolder.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
