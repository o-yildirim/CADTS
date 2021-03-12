using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryStatisticManager : MonoBehaviour
{
    public static MemoryStatisticManager instance;
    private MemoryStatistic statistic;
    public float minigameScore;
    public float globalTime;
    public int correctlyAnswered;

    public void Awake()
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

    public void EvaluateValues(float minigameScore, int correctlyAnswered, float globalTime)
    {
        this.correctlyAnswered = correctlyAnswered;
        this.globalTime = globalTime;
    }

    public void InitializeStatisticObject()
    {
        statistic = new MemoryStatistic(
                                      DatabaseHandler.loggedInUser,
                                      "Memory",
                                      "SimonSays",
                                       minigameScore,
                                       correctlyAnswered,
                                       globalTime
                                      );
    }

    public void InsertToDatabase()
    {
        DatabaseHandler.InsertStatistic(statistic);
    }
}
