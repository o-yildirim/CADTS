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
    float spawnDelay = 0.5f;

    void Start()
    {
        StartCoroutine(Spawn());
        //InvokeRepeating("spawning", 0, rate);
    }

    void Update()
    { 
        speedUp();
       /* if (finished())
            CancelInvoke();*/
    }

    public bool finished()
    {
        if (MathGameController.instance.health <= 0)
            return true;

        else
            return false;
    }

    public void speedUp()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }

        else
        {
            Debug.Log("Time has run out!");
            timeRemaining = 5;
        }
    }
    
    IEnumerator Spawn()
    {
        if (finished())
            yield break;
        else
        {
            SpawnBalloon();
            rate = rate - 0.5f;
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
            yield return null;
        } 
    }
    
    void SpawnBalloon()
    {
        //Instantiate(balloon, new Vector2(Random.Range(transform.position.x + rangeMax, transform.position.x + rangeMin), transform.position.y), Quaternion.identity);
        GameObject obj = Instantiate(balloon, new Vector2(Random.Range(gameObject.transform.position.x - rangeMax, gameObject.transform.position.x - rangeMin), gameObject.transform.position.y), Quaternion.identity);
        rb = obj.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, velocity);
        velocity *= 1.01f;
        spawnDelay *= 0.999f;
    }

    void spawning()
    {
        Instantiate(balloon, new Vector2(Random.Range(transform.position.x + rangeMax, transform.position.x + rangeMin), transform.position.y), Quaternion.identity);
    }
}
