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

    public void loadSceneCall(int buildIndex)
    {
        StartCoroutine(loadScene(buildIndex));
    }

    public void loadMinigame()
    {
        GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
        Minigame minigame = selectedGameObject.GetComponent<Minigame>();
        if (minigame != null)
        {
            int buildIndex = minigame.sceneIndex;
            loadSceneCall(buildIndex);
        }
    }
}