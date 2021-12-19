using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : MonoBehaviour
{
    public Transform snakeTail;
    public float circleDiameter;
    // Start is called before the first frame update

    private List<Transform> bodyParts = new List<Transform>();
    private List<Vector2> positions = new List<Vector2>();

    void Start()
    {
        positions.Add(snakeTail.position);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = ((Vector2)snakeTail.position - positions[0]).magnitude;

        if (distance > circleDiameter)
        {
            Vector2 direction = ((Vector2)snakeTail.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * circleDiameter);
            positions.RemoveAt(positions.Count - 1);

            distance -= circleDiameter;
        }

        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].position = Vector2.Lerp(positions[i + 1], positions[i], distance / circleDiameter);
        }


    }
    public void AddTail()
    {
        Transform tail = Instantiate(snakeTail, positions[positions.Count - 1], Quaternion.identity, transform);
        bodyParts.Add(tail);
        positions.Add(tail.position);
    }
}
