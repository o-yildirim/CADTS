using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public string withUppercase;
    public string withoutUppercase;

    public Text leftText;
    public Text rightText;

    public Text timeText;

    public Text tutorialText;

    public GameObject gamePanel;
    

    public int score;
    public int questionsAsked;
    public int questionAnswered;
    public int correctAnswered;

    public int minLetters = 1;
    public int maxLetters = 7; //Random fonksiyonu 7 yi exclude ediyor yani 6 karakterli olacak en uzun.

    public bool rightIsCorrect;
    public bool gamePaused = true;

    private float limitTime;
    //private float timePassedSinceLastAnswer;
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


  
    void Start()
    {
        resetAttributes();
        //newQuestion();
        StartCoroutine(tutorial());
    }

    private void Update()
    {
        if (gamePaused) return;

        this.limitTime -= Time.deltaTime;
        this.timeText.text = ((int) this.limitTime).ToString();


        if(limitTime <= 0f)
        {
            finishGame();
           

        }


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            checkLeft();//Soldaki doğru cevap mı diye kontrol et
            newQuestion();
            //timePassedSinceLastAnswer = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            checkRight();//Sağdaki doğru cevap mı diye kontrol et
            newQuestion();
            //this.timePassedSinceLastAnswer = 0f;
        }

        //this.timePassedSinceLastAnswer += Time.deltaTime;

    }

    public void newQuestion()
    {
        int length = (int)Random.Range(minLetters, maxLetters);

        this.withoutUppercase = StringManager.instance.generateString(length);
        this.withUppercase = StringManager.instance.generateString(length);

        this.withUppercase = StringManager.instance.assignRandomUppercase(withUppercase);

        int randomTextIndex = (int)Random.Range(0, 2);

        if (randomTextIndex == 0)
        {
            this.leftText.text = withUppercase;
            this.rightText.text = withoutUppercase;
            this.rightIsCorrect = false;//O zaman doğru cevap sol olacak
        }
        else
        {
            this.rightText.text = withUppercase;
            this.leftText.text = withoutUppercase;
            this.rightIsCorrect = true;//Doğru cevap sağ olacak
        }

        this.questionsAsked++;


    }

    public void resetAttributes()
    {
        this.score = 0;
        this.questionAnswered = 0;
        this.correctAnswered = 0;
        this.questionsAsked = 0;
        this.limitTime = 60f;
    }

    public void checkLeft()
    {
        if (!rightIsCorrect)
        {
            this.score += 100;
        
            this.correctAnswered++;
            Debug.Log(score);
        }
        else
        {
            //Dııt sesi belki yanlış bildiği için
        }
        this.questionAnswered++;
    }

    public void checkRight()
    {
        if (rightIsCorrect)
        {
            this.score += 100;
            this.correctAnswered++;
            Debug.Log(score);
        }
        else
        {
            //Dııt sesi belki yanlış bildiği için
        }
        this.questionAnswered++;
    }

    public  IEnumerator tutorial()
    {
       
        this.withUppercase = "A";
        this.withoutUppercase = "a";

        this.rightText.text = withoutUppercase;
        this.leftText.text = withUppercase;

        while (!Input.GetKeyDown(KeyCode.LeftArrow))
        {
            yield return null;
        }

        this.tutorialText.text = "Harika!";
        yield return new WaitForSeconds(2f);

        this.tutorialText.text = "Unutmayın ki cevapların doğruluğu kadar soruları hızlı cevaplamak da önemlidir.";
        yield return new WaitForSeconds(3f);

        this.tutorialText.text = "Oyunu başlatmak için herhangi bir tuşa basın.";

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        tutorialText.enabled = false;
        timeText.enabled = true;
        newQuestion();
        gamePaused = false;

    }

    public void finishGame()
    {
        this.gamePaused = true;
        this.gamePanel.SetActive(false);
        StatisticManager.instance.showStatistics();
    }

}

