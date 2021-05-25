using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator loadScene(int index)
    {  
        AsyncOperation loading = SceneManager.LoadSceneAsync(index);
        while (!loading.isDone)
        {
            yield return null;
        }
    }

    public void loadSceneCall(int buildIndex)
    {
        StartCoroutine(loadScene(buildIndex));
    }

    public void loadLoginScreen()
    {
        StartCoroutine(loadScene(0));
    }

    public void loadMainMenu()
    {
        StartCoroutine(loadScene(1));
    }
}