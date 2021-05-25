using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowStatisticManager : MonoBehaviour
{

    public static FlowStatisticManager instance;
    private FlowStatistic statistic;

    public Dictionary<Tile, int> tilesRotationCounts;

    public float time;
    public int cycles;
    public int wrongAttempts;
    public int[] pathCostByPipes;
    public int allLevelsTotalCostByPipes;
    public float score;


    public float timeMultiplier = 2500f;
    public float cycleMultiplier = 5f;
    public float wrongAttemptMultiplier = 15f;
    public float pathCostByPipesMultiplier = 1f;

    public GameObject statisticCanvas;
    public Text cycleText;
    public Text pathCostText;
    public Text scoreText;
    public Text wrongAttemptsText;

    public bool timerOn = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }


    }
    void Start()
    {
        tilesRotationCounts = new Dictionary<Tile, int>();
        statisticCanvas.GetComponentInChildren<Button>().onClick.AddListener(SceneManagement.instance.loadMainMenu);
        pathCostByPipes = new int[ProblemSolvingGameManager.instance.levelCount + 1];
    }

    private void Update()
    {
        if (timerOn)
        {
            time += Time.deltaTime;
        }
    }

    public void IncrementRotation(Tile tile)
    {

        if (tilesRotationCounts.ContainsKey(tile))
        {
            tilesRotationCounts[tile]++;
        }
        else
        {
            tilesRotationCounts.Add(tile, 1);
        }

        Debug.Log(tilesRotationCounts[tile]);

    }

    public void IncrementWrongAttempt()
    {
        wrongAttempts++;
    }

    public void EvaluateValues()
    {

        foreach (int rotationCount in tilesRotationCounts.Values)
        {
            cycles += rotationCount / 4;
        }

        for (int i = 0; i < pathCostByPipes.Length; i++)
        {
            Debug.Log("Level " + i + " yol uzunlugu: " + pathCostByPipes[i]);
            allLevelsTotalCostByPipes += pathCostByPipes[i];
        }

        score = ((1f / time) * timeMultiplier) - (wrongAttempts * wrongAttemptMultiplier) - (cycles * cycleMultiplier) - (allLevelsTotalCostByPipes * pathCostByPipesMultiplier);

        if (score < 0f)
        {
            score = 0f;
        }
    }
    public void InitializeStatisticObject()
    {
        statistic = new FlowStatistic(
                                      DatabaseHandler.loggedInUser,
                                      "ProblemSolving",
                                      "FlowGame",
                                       score,
                                       cycles,
                                       allLevelsTotalCostByPipes,
                                       wrongAttempts
                                      );

    }
    public void InsertToDatabase()
    {
        DatabaseHandler.InsertStatistic(statistic);
    }

    public void DisplayStatisticPanel()
    {
        cycleText.text = "Devir sayısı: " + cycles;
        wrongAttemptsText.text = "Yanlış deneme: " + wrongAttempts;
        pathCostText.text = "Bulunan yolların toplam uzunluğu: " + allLevelsTotalCostByPipes;
        scoreText.text = "Skor: " + score;

        statisticCanvas.SetActive(true);

    }

    public void ResetAttributes()
    {
        tilesRotationCounts.Clear();
        cycles = 0;
        instance.wrongAttempts = 0;
        for (int i = 0; i < pathCostByPipes.Length; i++)
        {
            pathCostByPipes[i] = 0;
        }
        allLevelsTotalCostByPipes = 0;
    }

}
