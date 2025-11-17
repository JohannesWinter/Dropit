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
                move[i] = true;
            }

            if (move[i] == true)
            {
                move[i] = false;
                StartCoroutine(moveString(strings[i], directions[i], speeds[i] * UnityEngine.Random.Range(0.85f, 1.15f), i));
            }
        }
    }



    IEnumerator moveString(GameObject toMove, Vector3 direction, float speed, int index)
    {
        if (moving[index])
        {
            yield return null;
        }
        moving[index] = true;
        Vector3 oldPosition = toMove.transform.position;
        Vector3 toMovePosition = oldPosition + direction;
        Stack positions = new Stack();
        float restDistance = float.MaxValue;
        while (restDistance > 0.1)
        {
            positions.Push(toMove.transform.localPosition);
            Vector3 currentPosition = toMove.transform.position;
            Vector3 restVector = toMovePosition - currentPosition;
            restDistance = Vector3.Magnitude(restVector);
            toMove.transform.Translate(restVector * speed);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        while (positions.Count > 0)
        {
            toMove.transform.localPosition = (Vector3) positions.Pop();
            yield return new WaitForSecondsRealtime(0.035f);
        }
        moving[index] = false;
    }
}
