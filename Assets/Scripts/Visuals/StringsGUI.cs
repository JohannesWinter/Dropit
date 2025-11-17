using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringsGUI : MonoBehaviour
{
    public GameObject[] strings;
    public float[] speeds;
    public Vector3[] directions;
    public bool[] move;
    bool[] moving;
    // Start is called before the first frame update
    void Start()
    {
        move = new bool[strings.Length];
        moving = new bool[strings.Length];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < move.Length; i++)
        {
            if (moving[i] == false)
            {
                if (UnityEngine.Random.Range(0, (int)(100/Time.unscaledDeltaTime)) == 0)
                {
                    move[i] = true;
                }
            }

            if (moving[i] == true)
            {
                moving[i] = false;
                StartCoroutine(moveString(strings[i], directions[i], speeds[i], i));
            }
        }
    }



    IEnumerator moveString(GameObject toMove, Vector3 direction, float speed, int index)
    {
        moving[index] = true;
        Vector3 oldPosition = toMove.transform.position;
        Vector3 toMovePosition = oldPosition + direction;
        Stack positions = new Stack();
        for (int i = 0; i < 20; i++)
        {
            positions.Push(toMove.transform.position);
            Vector3 currentPosition = toMove.transform.position;
            Vector3 restVector = toMovePosition - currentPosition;
            toMove.transform.Translate(restVector * speed);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        while (positions.Count > 0)
        {
            toMove.transform.position = (Vector3) positions.Pop();
            yield return new WaitForSecondsRealtime(0.035f);
        }
        toMove.transform.position = oldPosition;
        moving[index] = false;
    }
}
