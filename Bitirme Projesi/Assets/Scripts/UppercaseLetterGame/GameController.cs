using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    public Button skipTutorialButton;

    public string withUppercase;
    public string withoutUppercase;

    public Text leftText;
    public Text rightText;

    public Text timeText;

    public Text tutorialText;

    public GameObject gamePanel;

    public Image marker;
    public Sprite check;
    public Sprite cross;


    public float score = 0f;
    public int questionsAsked;
    public int questionAnswered;
    public int correctAnswered;

    //  public float questionAskedTime;
    //  public float questionAnsweredTime;
    //  public float reactionTimeAverage;

    public float reactionTimeAverage;
    public float reactionTimeCounter = 0;

    public int wrongStreakLimit = 5;
    private int wrongStreak = 0;

    public int correctStreakBonusReach = 5;
    private int correctStreak = 0;

    public float givenTime;
    public float markerDisplayDuration = 0.25f;
    public float markerActiveTime = 0f;
    public float bonusTime = 2f;

    public int minLetters = 1;
    public int maxLetters = 7; //Random fonksiyonu 7 yi exclude ediyor yani 6 karakterli olacak en uzun.

    public bool rightIsCorrect;
    public bool gamePaused = true;

    private float limitTime;
    private float timePassedSinceLastAnswer;
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
        marker.enabled = false;
        StartCoroutine(tutorial());
    }

    private void Update()
    {
        if (gamePaused) return;

        reactionTimeCounter += Time.deltaTime;

        this.limitTime -= Time.deltaTime;
        this.timeText.text = ((int) this.limitTime).ToString();

        if(limitTime <= 0f)
        {
            finishGame();
        }

        markerActiveTime += Time.deltaTime;
        if(marker.isActiveAndEnabled && markerActiveTime > markerDisplayDuration)
        {
            marker.enabled = false;
        }



        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // questionAnsweredTime = Time.time;
            // reactionTimeAverage += questionAskedTime - questionAnsweredTime;
            

            checkLeft();//Soldaki doğru cevap mı diye kontrol et
            newQuestion();
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
   
           // questionAskedTime = Time.time;
           // reactionTimeAverage += questionAskedTime - questionAnsweredTime;

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
        //questionAskedTime = Time.time;


    }

    public void resetAttributes()
    {
        this.score = 0;
        this.questionAnswered = 0;
        this.correctAnswered = 0;
        this.questionsAsked = 0;
        this.limitTime = this.givenTime;
    }

    public void checkLeft()
    {
        if (!rightIsCorrect)
        {
            reactionTimeAverage += reactionTimeCounter;
            reactionTimeCounter = 0f;

            this.correctAnswered++;       
            displayMarker(check);

           
            wrongStreak = 0;
            correctStreak++;
            if(correctStreak == correctStreakBonusReach)
            {
                limitTime += bonusTime;
                correctStreak = 0;
            }

        }
        else
        {
            //StartCoroutine(displayMarker(markerDisplayDuration, cross));
            displayMarker(cross);
            correctStreak = 0;
            wrongStreak++;
            if(wrongStreak == wrongStreakLimit)
            {
                //QUESTION ANSWERED I ARTTIRACAK MI
                finishGame();
            }
        }
        this.questionAnswered++;
    }

    public void checkRight()
    {
        if (rightIsCorrect)
        {
            reactionTimeAverage += reactionTimeCounter;
            reactionTimeCounter = 0f;

            this.correctAnswered++;
            // Debug.Log(score);
            //StartCoroutine(displayMarker(markerDisplayDuration, check));
            displayMarker(check);

            wrongStreak = 0;
            correctStreak++;
            if (correctStreak == correctStreakBonusReach)
            {
                limitTime += bonusTime;
                correctStreak = 0;
            }
        }
        else
        {
            //StartCoroutine(displayMarker(markerDisplayDuration, cross));
            displayMarker(cross);
            correctStreak = 0;
            wrongStreak++;
            if (wrongStreak == wrongStreakLimit)
            {
                //QUESTION ANSWERED I ARTTIRACAK MI
                finishGame();
            }
        }
        this.questionAnswered++;
    }

    public  IEnumerator tutorial()
    {
        skipTutorialButton.enabled = true;
       
        this.withUppercase = "A";
        this.withoutUppercase = "a";

        this.rightText.text = withoutUppercase;
        this.leftText.text = withUppercase;

        while (!Input.GetKeyDown(KeyCode.LeftArrow))
        {
            yield return null;
        }
        displayMarker(check);
        this.tutorialText.text = "Harika!";
        yield return new WaitForSeconds(2f);
        marker.enabled = false;

        this.tutorialText.text = "Unutmayın ki cevapların doğruluğu kadar soruları hızlı cevaplamak da önemlidir.";
        yield return new WaitForSeconds(5f);

        this.tutorialText.text = "Arka arkaya yapılan her 5 doğruda sürenize 2 saniye eklenmektedir.";
        yield return new WaitForSeconds(5f);

        this.tutorialText.text = "Eğer arka arkaya 5 yanlış yaparsanız oyun doğrudan sonlanacaktır.";
        yield return new WaitForSeconds(5f);

        this.tutorialText.text = "Oyunu başlatmak için herhangi bir tuşa basın.";

        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        skipTutorialButton.gameObject.SetActive(false);
        startGame();

    }
    public void startGame()
    {
        if (!gamePaused)
        {
            return;
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
        if (reactionTimeAverage != 0)
        {
            reactionTimeAverage = reactionTimeAverage / questionAnswered;
        }
        UppercaseLetterStatisticManager.instance.InitializeStatistics();
        UppercaseLetterStatisticManager.instance.ShowStatistics();
        UppercaseLetterStatisticManager.instance.InsertStatistics();
    }

    public void displayMarker(Sprite markerToDisplay)
    {
        marker.sprite = markerToDisplay;
        marker.enabled = true;
        markerActiveTime = 0f;
    }

    public void skipTutorial()
    {
        skipTutorialButton.gameObject.SetActive(false);
        StopCoroutine(tutorial());
        startGame();

    }

}

