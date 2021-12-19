using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGrow : MonoBehaviour
{
    public SnakeTail snakeTail;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform.tag == "Food")
        {
            GameObject background = GameObject.FindGameObjectWithTag("Background");
            List<GameObject> fruits = background.GetComponent<RandomSpawner>().currentFruits;
            fruits.Remove(other.gameObject);
            Destroy(other.gameObject);
            Debug.Log(fruits.Count);
            snakeTail.AddTail();
        }
    }
}
