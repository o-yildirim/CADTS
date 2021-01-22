using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject balloon;
    [SerializeField]
    public int rangeMin=-7, rangeMax=-2;
    Rigidbody2D rb;
    float velocity = 1f;
    [SerializeField]
    float time=0f, timeLimit=2f, timeToStart=0f;

    void Update()
    {
        if (timeToStart > 0f)
        {
            timeToStart -= Time.deltaTime;
            StartCoroutine(Wait());
        }

        else
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
                timeLimit -= 0.01f;
            }
        }

        
    }

    void SpawnBalloon()
    {
        GameObject obj = Instantiate(balloon, new Vector2(Random.Range(gameObject.transform.position.x + rangeMax, gameObject.transform.position.x + rangeMin), gameObject.transform.position.y), Quaternion.identity);
        rb = obj.GetComponent<Rigidbody2D>();
        //rb.velocity = new Vector2(0f, velocity * Time.deltaTime);
        rb.velocity = new Vector2(0f, velocity);
        Debug.Log(rb.velocity.magnitude);
        velocity *= 1.01f;
        MathGameController.instance.balloons.Add(obj.GetComponent<Balloon>());
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeToStart);
    }
}
