using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;

public class SnakeMove : MonoBehaviour
{
    public float speed = 3f;
    public float rotateSpeed = 270f;

    // public float direction;

    public GameObject sign;

    Vector3 mousePos;

    public Vector2 currentVelocity;

    public List<Transform> bodyParts = new List<Transform>();

    public float smoothTime = 0.3f;

    float minDistance = 1;

    float distanceTagName = 2;

    Color snakeColor;

    // Start is called before the first frame update
    void Start()
    {
        sign = Instantiate(sign, transform.position + transform.up * distanceTagName, Quaternion.identity);
        sign.transform.rotation = Camera.main.transform.rotation;
        sign.GetComponent<TextMesh>().text = "Slither";

        growOnThisFood.Add(2);
        var colors = new Color[]{
        Color.black, Color.blue, Color.cyan, Color.clear, Color.gray, Color.green, Color.grey, Color.magenta, Color.red, Color.white, Color.yellow
        };

        snakeColor = colors[Random.Range(0, colors.Length)];
        Transform snakeHead = transform.GetChild(0);
        snakeHead.GetComponent<Renderer>().material.color = snakeColor;
    }


    void colorMySnake()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].GetComponent<Renderer>().material.color = snakeColor;
        }
    }

    void Update()
    {
        Running();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), rotateSpeed * Time.deltaTime);

        mousePosition += (transform.position - mousePosition).normalized * minDistance;
        transform.position = Vector2.SmoothDamp(transform.position, mousePosition + transform.up * 5, ref currentVelocity, smoothTime, speed);
        sign.transform.position = Vector2.SmoothDamp(sign.transform.position, transform.position + transform.up * distanceTagName, ref currentVelocity, smoothTime, speed);
        colorMySnake();
        ApplyingStuffForBody();
    }

    public Transform snakeBody;
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.transform.tag == "Food")
        {
            GameObject background = GameObject.FindGameObjectWithTag("Background");
            List<GameObject> fruits = background.GetComponent<RandomSpawner>().currentFruits;
            fruits.Remove(other.gameObject);
            Destroy(other.gameObject);
            foodCounter++;
            if (SizeUp(foodCounter) == false)
            {
                if (bodyParts.Count == 0)
                {
                    Vector3 currentPos = transform.position;
                    Transform newBodyPart = Instantiate(snakeBody, currentPos, Quaternion.identity) as Transform;

                    // newBodyPart.localScale = currentSize;
                    // newBodyPart.GetComponent<SnakeBody>().overtime = bodyPartOverTimeFollow;

                    bodyParts.Add(newBodyPart);
                    Renderer renderer = newBodyPart.GetComponent<SpriteRenderer>();
                    renderer.sortingOrder = -bodyParts.Count;
                }
                else
                {
                    Vector3 currentPos = bodyParts[bodyParts.Count - 1].position;
                    Transform newBodyPart = Instantiate(snakeBody, currentPos, Quaternion.identity) as Transform;

                    // newBodyPart.localScale = currentSize;
                    // newBodyPart.GetComponent<SnakeBody>().overtime = bodyPartOverTimeFollow;

                    bodyParts.Add(newBodyPart);
                    Renderer renderer = newBodyPart.GetComponent<SpriteRenderer>();
                    renderer.sortingOrder = -bodyParts.Count;
                }
            }
            else
            {
                currentSize += Vector3.one * growthrate;
                bodyPartOverTimeFollow += 0.04f;
                transform.localScale = currentSize;
                foodCounter = 0;
            }

        }
    }

    private int foodCounter;
    private int currentFood;
    public List<int> growOnThisFood = new List<int>();
    private Vector3 currentSize = Vector3.one;
    public float growthrate = 0.1f;
    public float bodyPartOverTimeFollow = 0.2f;
    bool SizeUp(int x)
    {
        if (x == growOnThisFood[currentFood])
        {
            currentFood++;
            growOnThisFood.Add(Mathf.FloorToInt(growOnThisFood[growOnThisFood.Count - 1] * 1.5f));
            return true;
        }
        else { return false; }
    }


    private bool running;
    public float speedWhileRunnin = 8f;
    public float speedWhileWalking = 4f;
    public float bodyPartFollowTimeWalking = 0.2f;
    public float bodyPartFollowTimeRunning = 0.1f;
    void Running()
    {
        if (bodyParts.Count > 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                speed = speedWhileRunnin;
                running = true;
                bodyPartOverTimeFollow = bodyPartFollowTimeRunning;
            }
            if (Input.GetMouseButtonUp(0))
            {
                speed = speedWhileWalking;
                running = false;
                bodyPartOverTimeFollow = bodyPartFollowTimeWalking;
            }
        }
        else
        {
            speed = speedWhileWalking;
            running = false;
            bodyPartOverTimeFollow = bodyPartFollowTimeWalking;
        }
        if (running == true)
        {
            StartCoroutine("LoseBodyPart");
        }
        else
        {
            bodyPartOverTimeFollow = bodyPartFollowTimeWalking;
        }
        Debug.Log(growOnThisFood[currentFood]);
    }

    int totalFoodCount(List<int> grow)
    {
        int sum = 0;
        foreach (int item in grow)
        {
            sum += item;
        }
        sum = sum - grow[grow.Count - 1] + foodCounter;
        return sum;
    }
    int totalFoodCurrentFood(List<int> grow)
    {
        int sum = 0;
        foreach (int item in grow)
        {
            sum += item;
        }
        return sum;
    }

    IEnumerator LoseBodyPart()
    {
        yield return new WaitForSeconds(0.5f);
        int lastIndex = bodyParts.Count - 1;
        Transform lastBodyPart = bodyParts[lastIndex].transform;

        RandomSpawner randomSpawner = GameObject.FindGameObjectWithTag("Background").GetComponent<RandomSpawner>();
        Vector2 randomPos = Random.insideUnitCircle * 0.5f;
        randomPos += (Vector2)lastBodyPart.position;
        randomSpawner.spawnObjectAt(randomPos);

        int totalFoodEat = totalFoodCount(growOnThisFood);

        currentSize -= Vector3.one * growthrate / totalFoodEat;
        bodyPartOverTimeFollow -= 0.04f / totalFoodEat;
        transform.localScale = currentSize;

        bodyParts.RemoveAt(lastIndex);
        Destroy(lastBodyPart.gameObject);
        foodCounter--;

        if (foodCounter < 0)
        {
            growOnThisFood.RemoveAt(growOnThisFood.Count - 1);
            currentFood--;
            foodCounter = growOnThisFood[currentFood] - 1;
        }

        StopCoroutine("LoseBodyPart");
    }
    void ApplyingStuffForBody()
    {
        foreach (Transform bodyPart in bodyParts)
        {
            bodyPart.localScale = currentSize;
            bodyPart.GetComponent<SnakeBody>().overtime = bodyPartOverTimeFollow;
        }
    }
}
