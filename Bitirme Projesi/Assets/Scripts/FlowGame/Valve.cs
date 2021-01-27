using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve : MonoBehaviour
{
    private AudioSource audioSource;
    public float rotationSpeed = 3f;
    public float offset = 0.3f;
    private float rotationDuration;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rotationDuration = audioSource.clip.length;
    }
    public IEnumerator Rotate()
    {

        float time = 0f;
        audioSource.Play();
        while (time <= rotationDuration - offset)
        {
            transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(offset);
    }
}
