using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTextureRandomizer : MonoBehaviour
{
        public int seed;
        public GameObject[] factoryWalls;
        // Start is called before the first frame update
        void Start()
        {
            System.Random rnd = new System.Random(seed);

            for (int i = 0; i < factoryWalls.Length; i++)
            {
                int randomIntRange = rnd.Next(50, 400);
                Material mat = factoryWalls[i].GetComponent<Renderer>().material;
                mat.SetFloat("_NormalScale", (randomIntRange / 100f));
            }
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}
