using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class GameManager : MonoBehaviour
{

    public GameObject gameButtonPrefab;
    public Text Info;
    public GameObject DisplayText;
    public Text Timer;
    public Text GTimer;
    public GameObject LevelTimer;
    public GameObject GameOverScreen = null;
    public GameObject FinalCorrectlyAnswered;
    public Text FCA;
    public GameObject FinalScore;
    public Text FS;
    public GameObject FinalTime;
    public Text FT;

    public Button skip;
    public GameObject SkipButton;


    public float timeStart;
    public float globalTimer = 0f;

    public List<ButtonSetting> buttonSettings;
    public Transform gameFieldPanelTransform;
    List<GameObject> gameButtons;

    int bleepCount = 2;
    List<int> bleeps;
    List<int> playerBleeps;

    int currentLevel = 0;
    int correctlyAnswered = 0;
    int buttonPoints = 0;
    float score = 0f;
    System.Random rg, randomNumberGenerator;

    bool inputEnabled = false;
    bool gameOver = false;
    bool timerActive = false;
    bool globalTimerActive = false;
    bool tutorialEnabled = false;

    void Start()
    {
        GameOverScreen = GameObject.Find("GameOverScreen");
        GameOverScreen.SetActive(false);
        randomNumberGenerator = new System.Random(System.DateTime.Now.Millisecond);
        gameButtons = new List<GameObject>();
        DisplayText = GameObject.Find("DisplayText");
        Info = DisplayText.GetComponent<Text>();
        Info.text = "";
        LevelTimer = GameObject.Find("LevelTimer");
        Timer = LevelTimer.GetComponent<Text>();
        Timer.text = timeStart.ToString("F2");
        SkipButton = GameObject.Find("SkipButton");
        skip = SkipButton.GetComponent<Button>();

        CreateGameButton(0, new Vector3(-64, 64));
        CreateGameButton(1, new Vector3(64, 64));
        CreateGameButton(2, new Vector3(-64, -64));
        CreateGameButton(3, new Vector3(64, -64));

        StartCoroutine(Tutorial());
    }

    void Update()                                           //level timer ve global timer
    {
        if (timerActive)
        {
            timeStart += Time.deltaTime;
            Timer.text = timeStart.ToString("F2");
        }
        else
        {
            timeStart = 0f;
            Timer.text = timeStart.ToString("F2");
        }

        if (globalTimerActive)
        {
            globalTimer += Time.deltaTime;
        }


    }

    void CreateGameButton(int index, Vector3 position)
    {
        GameObject gameButton = Instantiate(gameButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        gameButton.transform.SetParent(gameFieldPanelTransform);
        gameButton.transform.localPosition = position;

        gameButton.GetComponent<Image>().color = buttonSettings[index].normalColor;
        gameButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            OnGameButtonClick(index);
        });

        gameButtons.Add(gameButton);
    }

    void PlayAudio(int index)
    {
        float length = 0.5f;
        float frequency = 0.001f * ((float)index + 1f);

        AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, -1f), new Keyframe(length, 0f, -1f, 0f));
        AnimationCurve frequencyCurve = new AnimationCurve(new Keyframe(0f, frequency, 0f, 0f), new Keyframe(length, frequency, 0f, 0f));

        LeanAudioOptions audioOptions = LeanAudio.options();
        audioOptions.setWaveSine();
        audioOptions.setFrequency(44100);

        AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, audioOptions);

        LeanAudio.play(audioClip, 0.5f);
    }

    void OnGameButtonClick(int index)
    {
        if (!inputEnabled)
        {
            return;
        }

        Bleep(index);

        playerBleeps.Add(index);


        if (bleeps[playerBleeps.Count - 1] != index)
        {
            GameOver();
            return;
        }

        if (tutorialEnabled == false)
        {
            correctlyAnswered++;
            buttonPoints += 10;
        }

        if (bleeps.Count == playerBleeps.Count)
        {
            if (tutorialEnabled == true)
                StartCoroutine(TutorialEnd());
            else
                StartCoroutine(SimonSays());
        }
    }

    void GameOver()
    {
        gameOver = true;
        inputEnabled = false;
        timerActive = false;
        globalTimerActive = false;
        GameOverScreen.SetActive(true);
        /* = GameObject.Find("FinalCorrectlyAnswered");                                         //Bu noktada bir bug var, henüz çözemedim!!! FinalScore ve FinalTİme gibi çalışması lazım?
        FCA = FinalCorrectlyAnswered.GetComponentInChildren<Text>();                                             
        FCA.text = "Toplam Doğru Sayısı: " + correctlyAnswered;*/                                                   
        FinalScore = GameObject.Find("FinalScore");
        FS = FinalScore.GetComponentInChildren<Text>();
        FS.text = "Toplam Skor: " + score;
        FinalTime = GameObject.Find("FinalTime");
        FT = FinalTime.GetComponentInChildren<Text>();
        FT.text = "Toplam Süre: " + globalTimer.ToString("F2");

    }

    IEnumerator Tutorial()                      //tutorial skip buttonu eklenecek, yazılar delayleriyle birlikte düzenlenecek
    {
        SkipButton.SetActive(true);
        skip.onClick.AddListener(SkipTutorial);
        inputEnabled = false;
        tutorialEnabled = true;
        rg = new System.Random("honeybee".GetHashCode());           //tutorialda her seferinde aynı ikili geliyor.
        SetBleeps();
        Info.text = "Bu oyunda amacınız, yanan düğmelerin sırasını aklınızda tutarak tekrar girmenizdir.";
        yield return new WaitForSeconds(2.5f);
        Info.text = "Düğmeler şimdi sırayla parlayacaktır, lütfen dikkatlice izleyiniz.";
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < bleeps.Count; i++)
        {
            Bleep(bleeps[i]);

            yield return new WaitForSeconds(1f);
        }
        Info.text = "Sıra sizde. Lütfen diziyi tekrar giriniz.";
        inputEnabled = true;
        while (gameOver != true)
        {
            yield return null;
        }
    }           

    IEnumerator TutorialEnd()           //yazılar delayleriyle birlikte düzenlenecek
    {
        Info.text = "Girdiniz doğru! Harika!";
        yield return new WaitForSeconds(2f);
        Info.text = "Unutmayın ki doğru bastığınız düğme sayısı ile birlikte ne kadar hızlı yanıt verdiğiniz de önemlidir.";
        yield return new WaitForSeconds(2.5f);
        Info.text = "Oyunu başlatmak için 'C' tuşuna basınız.";

        while (!Input.GetKeyDown("c"))
        {
            yield return null;
        }
        tutorialEnabled = false;
        StartCoroutine(SimonSays());
    }

    public void SkipTutorial()
    {
        StopAllCoroutines();
        tutorialEnabled = false;
        StartCoroutine(SimonSays());
    }

    IEnumerator SimonSays()             //yazılar delayleriyle birlikte düzenlenecek
    {
        SkipButton.SetActive(false);
        inputEnabled = false;
        timerActive = false;
        globalTimerActive = false;
        currentLevel++;
        var randomNumber = randomNumberGenerator;

        rg = new System.Random(randomNumber.GetHashCode());

        SetBleeps();
        Info.text = "Seviye başlıyor. Lütfen diziyi dikkatlice takip ediniz.";
        yield return new WaitForSeconds(2.5f);

        for (int i = 0; i < bleeps.Count; i++)
        {
            Bleep(bleeps[i]);

            yield return new WaitForSeconds(1f);
        }

        Info.text = "Sıra sizde. Lütfen diziyi tekrar giriniz.";
        inputEnabled = true;
        timerActive = true;
        globalTimerActive = true;
        yield return null;
        Scoring();
    }


    void Bleep(int index)
    {
        LeanTween.value(gameButtons[index], buttonSettings[index].normalColor, buttonSettings[index].highlightColor, 0.25f).setOnUpdate((Color color) =>
        {
            gameButtons[index].GetComponent<Image>().color = color;
        });

        LeanTween.value(gameButtons[index], buttonSettings[index].highlightColor, buttonSettings[index].normalColor, 0.25f)
            .setDelay(0.5f)
            .setOnUpdate((Color color) =>
            {
                gameButtons[index].GetComponent<Image>().color = color;
            });

        PlayAudio(index);
    }

    void SetBleeps()
    {
        bleeps = new List<int>();
        playerBleeps = new List<int>();

        for (int i = 0; i < bleepCount; i++)
        {
            bleeps.Add(rg.Next(0, gameButtons.Count));
        }

        bleepCount++;
    }

    void Scoring()
    {
        score += (buttonPoints / timeStart) * (currentLevel * 0.01f);
    }

}
