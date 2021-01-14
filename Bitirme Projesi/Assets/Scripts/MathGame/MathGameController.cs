using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MathGameController : MonoBehaviour
{
    public static MathGameController instance;
    [SerializeField]
    public int health = 5;
    [SerializeField]
    public InputField answer;
    bool finished = false;

    void Start()
    {
        
    }

    void Awake()
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

    void Update()
    {
        checkHealth();
        if (Input.GetKeyUp(KeyCode.Return)) {
            TapYourSubmitFunction();
        }
    }

    public void TapYourSubmitFunction()
    {
        if (answer.text == BaloonController.instance.answer.ToString())
        {
            Destroy(BaloonController.instance.gameObject);
        }
        answer.text = "";
    }

    public void checkHealth()
    {
        if (health <= 0)
        {
            StatisticPanelManager.instance.isActive = true;
        }
    }  
}
