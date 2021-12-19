using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public List<GameObject> fruits;
    public List<GameObject> currentFruits;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("spawnObject", 500);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // timer = Random.Range(1, 2);
        // currentTime -= Time.fixedDeltaTime;
        if (currentFruits.Count >= 500)
        {
            CancelInvoke();
        }
        else
        {
            Invoke("spawnObject", 1);
        }

        for (int i = 0; i < currentFruits.Count; i++)
        {
            float newScale = Random.Range(0.5f, 1.5f);
            currentFruits[i].transform.localScale = Vector2.Lerp(currentFruits[i].transform.localScale, new Vector2(newScale, newScale), newScale * 0.2f);
        }
    }

    Vector2 getSpawnPoint()
    {
        GameObject background = GameObject.FindGameObjectWithTag("Background");
        SpriteRenderer bgSize = background.GetComponent<SpriteRenderer>();

        float maxX = bgSize.size.x / 2 - 1;
        float minX = -maxX;
        float maxY = bgSize.size.y / 2 - 1;
        float minY = -maxY;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        return new Vector2(x, y);
    }

    void spawnObject()
    {
        GameObject objectToSpawn = fruits[Random.Range(0, fruits.Count)];
        GameObject fruit = Instantiate(objectToSpawn, getSpawnPoint(), Quaternion.identity);
        currentFruits.Add(fruit);
    }

    public void spawnObjectAt(Vector2 position)
    {
        GameObject objectToSpawn = fruits[Random.Range(0, fruits.Count)];
        GameObject fruit = Instantiate(objectToSpawn, position, Quaternion.identity);
        currentFruits.Add(fruit);
    }

}
