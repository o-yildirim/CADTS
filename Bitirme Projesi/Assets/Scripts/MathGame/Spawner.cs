using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject baloon;

    void Start()
    {
        InvokeRepeating("spawning", 0, 4);
    }

    void Update()
    {
        if (MathGameController.instance.health <= 0)
        {
            CancelInvoke();
        }
    }

    void spawning()
    {
        Instantiate(baloon, new Vector2(Random.Range(transform.position.x-7, transform.position.x-2), transform.position.y), Quaternion.identity);
    }
}
