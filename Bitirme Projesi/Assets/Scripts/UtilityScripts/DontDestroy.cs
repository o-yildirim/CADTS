
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DontDestroy : MonoBehaviour
{

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(this.gameObject.tag);

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

    }

}