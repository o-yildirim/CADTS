using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MathGameController : MonoBehaviour
{
    public static MathGameController instance;
    public GameObject exp;
    public List<Balloon> balloons;
    [SerializeField]
    public int health = 5;
    [SerializeField]
    public InputField answer;
    public Text scoreTxt;
    public bool isFinished = false;
    private int score = 0;
    public Text tutorialText;
    public Button skipTutorialBtn;
    public Spawner spawnerLeft;
    public Spawner spawnerRight;
    public Spawner tutorialSpawner;
    private Coroutine tutorialC = null;

    void Start()
    {
        tutorialC = StartCoroutine(tutorial());
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

        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            submitButton();
        }
        answer.Select();
        answer.ActivateInputField();
    }

    public void submitButton()
    {
        for (int i = 0; i < balloons.Count; i++)
        {
            if (balloons[i].answer.ToString() == answer.text)
            {
                score += 10;
                GameObject balloonToRemove = balloons[i].gameObject;
                balloons.Remove(balloons[i]);
                Instantiate(exp, new Vector2(balloonToRemove.transform.position.x, balloonToRemove.transform.position.y + 1), Quaternion.identity);
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
            MathStatisticManager.instance.EvaluateValues(score); //**
            MathStatisticManager.instance.InitializeStatisticObject(); //**
            MathStatisticManager.instance.InsertToDatabase(); //**
            StatisticPanelManager.instance.exitButton.onClick.AddListener(SceneManagement.instance.loadMainMenu);
        }
    }

    public void destroyBalloons()
    {
        for (int i = 0; i < balloons.Count; i++)
        {
            Destroy(balloons[i].gameObject);
        }
    }

    public IEnumerator tutorial()
    {
        skipTutorialBtn.enabled = true;
        skipTutorialBtn.onClick.AddListener(skipTutorial);
        health = int.MaxValue;
        while (!(score > 0))
        {
            yield return null;
        }

        tutorialSpawner.gameObject.SetActive(false);
        destroyBalloons();
        balloons = new List<Balloon>();
       

        tutorialText.text = "Harika!";
        Debug.Log(tutorialText.text);
        yield return new WaitForSeconds(5f);

        tutorialText.text = "Balonları yukarıdaki çubuğa temas edip patlatmadan önce doğru cevabı yazarak siz patlatmalısınız.";
        yield return new WaitForSeconds(7f);

        tutorialText.text = "Toplamda 5 temas hakkınız vardır. Haklarınız tükenince oyun sonlanacaktır.";
        yield return new WaitForSeconds(5f);

        tutorialText.text = "Zaman geçtikçe balonların hızı artacaktır. Bu yüzden işlemleri hızlı hesaplamak önemlidir.";
        yield return new WaitForSeconds(5f);

        tutorialText.text = "Oyunu başlatmak için herhangi bir tuşa basın.";

        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        startGame();
    }

    public void startGame()
    {
        tutorialText.gameObject.SetActive(false);
        skipTutorialBtn.gameObject.SetActive(false);
        spawnerLeft.gameObject.SetActive(true);
        spawnerRight.gameObject.SetActive(true);
        instance.score = 0;
        instance.health = 5;
        answer.text = "";
    }

    public void skipTutorial()
    {
        StopCoroutine(tutorialC);
        tutorialSpawner.gameObject.SetActive(false);
        if (balloons.Count>0)
        {
            destroyBalloons();
            balloons = new List<Balloon>();
        }         
        startGame();
    }
}
