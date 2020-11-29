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

    public void Start()
    {
        quitCanvas = gameObject.transform.GetChild(0).gameObject;        
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

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void LogOut()
    {
        DatabaseHandler.loggedInUser = null;
        //TOKENI YOKET
        SceneManagement.instance.loadLoginScreen();
    }
}
