using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MathGameController : MonoBehaviour
{
    public static MathGameController instance;
    public List<Balloon> balloons;
    [SerializeField]
    public int health = 5;
    [SerializeField]
    public InputField answer;
    public Text scoreTxt;
    public bool isFinished = false;
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
        
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) {
            submitButton();
        }
        answer.Select();
        answer.ActivateInputField();
    }

    public void submitButton()
    {
        for (int i=0; i<balloons.Count; i++)
        {
            if (balloons[i].answer.ToString() == answer.text)
            {
                score += 10;
                GameObject balloonToRemove = balloons[i].gameObject;
                balloons.Remove(balloons[i]);
                Destroy(balloonToRemove);
                Debug.Log(score);
            }
        }
        
        answer.text = "";
    }

    public void checkHealth()
    {
        if (health <= 0) 
        {
            isFinished = true;
            destroyBalloons();
            StatisticPanelManager.instance.gameCanvas.SetActive(false);
            Debug.Log(score);
            scoreTxt.text += score.ToString();
            StatisticPanelManager.instance.statisticPanel.SetActive(true);
        }
    }

    public void destroyBalloons()
    {
        for (int i = 0; i < balloons.Count; i++)
        {
            Destroy(balloons[i].gameObject);
        }
    }
}
