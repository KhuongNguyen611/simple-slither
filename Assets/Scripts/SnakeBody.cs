using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{

    public int myOrder;
    private Transform head;

    private List<Transform> bodyParts;
    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        bodyParts = head.GetComponent<SnakeMove>().bodyParts;
        for (int i = 0; i < bodyParts.Count; i++)
        {
            if (gameObject == bodyParts[i].gameObject)
            {
                myOrder = i;
            }
        }
    }

    Vector2 currentVelocity;
    float currentVelocityAngle;
    // Update is called once per frame
    public float overtime;
    float angle;
    void Update()
    {
        float rotateSpeed = head.GetComponent<SnakeMove>().rotateSpeed;
        float smoothTime = head.GetComponent<SnakeMove>().smoothTime;

        if (myOrder == 0)
        {

            Vector2 direction = head.position - transform.position;
            float targetAngle = Vector2.SignedAngle(Vector2.up, direction);
            angle = Mathf.SmoothDampAngle(angle, targetAngle, ref currentVelocityAngle, smoothTime, rotateSpeed);

            transform.eulerAngles = new Vector3(0, 0, angle);
            // Vector3 targetRotation = new Vector3(0, 0, angle);
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), rotateSpeed * Time.deltaTime);
            transform.position = Vector2.SmoothDamp(transform.position, head.position, ref currentVelocity, overtime);
        }
        else
        {
            Vector2 direction = bodyParts[myOrder - 1].position - transform.position;
            float targetAngle = Vector2.SignedAngle(Vector2.up, direction);
            angle = Mathf.SmoothDampAngle(angle, targetAngle, ref currentVelocityAngle, smoothTime, rotateSpeed);

            transform.eulerAngles = new Vector3(0, 0, angle);
            // Vector3 targetRotation = new Vector3(0, 0, angle);
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), rotateSpeed * Time.deltaTime);
            transform.position = Vector2.SmoothDamp(transform.position, bodyParts[myOrder - 1].position, ref currentVelocity, overtime);
        }
    }
}
