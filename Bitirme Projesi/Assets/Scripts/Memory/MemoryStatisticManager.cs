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
            Destroy(this);
        }
    }

    public void EvaluateValues(float minigameScore, int correctlyAnswered, float globalTime)
    {
        this.correctlyAnswered = correctlyAnswered;
        this.globalTime = globalTime;
    }
}
