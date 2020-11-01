using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{

    public GameObject quitCanvas;


    public static SettingsManager instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
        
    }
    public void QuitRequest()
    {
        Time.timeScale = 0f;
        quitCanvas.SetActive(true);

    }
    public void RejectQuit()
    {
        quitCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public static void QuitApplication()
    {
        Application.Quit();
    }
}
