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
    public Text scoreTxt;
    bool finished = false;
    private int score = 0;

    void Start()
    {
        answer.Select();
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
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) {
            TapYourSubmitFunction();
        }
        answer.Select();
        answer.ActivateInputField();
    }

    public void TapYourSubmitFunction()
    {
        if (answer.text == BalloonController.instance.answer.ToString())
        {
            score += 10;
            Destroy(BalloonController.instance.gameObject);
        }
        answer.text = "";
    }

    public void checkHealth()
    {
        if (health <= 0) //Sorunlu
        {
            //health = 5;
            StatisticPanelManager.instance.gameCanvas.SetActive(false);
            //Debug.Log(score);
            //scoreTxt.text = "Your score: " + score.ToString();
            StatisticPanelManager.instance.statisticPanel.SetActive(true);
        }
    }  
}
