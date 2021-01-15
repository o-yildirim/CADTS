using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject balloon;
    [SerializeField]
    public int rangeMin=-7, rangeMax=-2;
    private float rate = 4.0f;
    private float timeRemaining = 5;
    Rigidbody2D rb;
    float velocity = 0.2f;
    float spawnDelay = 3f;
    float time=0f, timeLimit=2f;

    void Start()
    {
       
    }

    void Update()
    {

        if (MathGameController.instance.isFinished)
        {
            return;
        }

        time += Time.deltaTime;
        if (time > timeLimit)
        {
            SpawnBalloon();
            time = 0f;
        }
    }

    void SpawnBalloon()
    {
        //Instantiate(balloon, new Vector2(Random.Range(transform.position.x + rangeMax, transform.position.x + rangeMin), transform.position.y), Quaternion.identity);
        GameObject obj = Instantiate(balloon, new Vector2(Random.Range(gameObject.transform.position.x + rangeMax, gameObject.transform.position.x + rangeMin), gameObject.transform.position.y), Quaternion.identity);
        rb = obj.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, velocity);
        velocity *= 1.01f;
        spawnDelay *= 0.999f;
        MathGameController.instance.balloons.Add(obj.GetComponent<Balloon>());
    }
}
