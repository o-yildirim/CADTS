using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;
    private void Awake()
    {
       if(instance == null)
        {
            instance = this;
        }
       else if(instance != this)
        {
            Destroy(this);
        }
    }

    public IEnumerator loadScene(int index)
    {
        Debug.Log("Korutinin içi");
        //loading screen aktifleşir
        AsyncOperation loading = SceneManager.LoadSceneAsync(index);
        while (!loading.isDone)
        {
            Debug.Log("Loading");
            yield return null;
        }
        //loading screeni kaldır
    }

    public void loadSceneCall()
    {
        Debug.Log("Metodun içi");
        StartCoroutine(loadScene(1));
    }
}
