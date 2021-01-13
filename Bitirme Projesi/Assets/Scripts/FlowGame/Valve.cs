using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve : MonoBehaviour
{
    public float rotationSpeed = 3f;

    public IEnumerator Rotate(float duration)
    {
        float time = 0f;
        while(time <= duration)
        {
            transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
