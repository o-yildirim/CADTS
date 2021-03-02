using System;
using UnityEngine;

public class MemoryStatistic : Statistic
{
    int correctlyAnswered;
    float globalTime;
    public MemoryStatistic(User owner, string minigameCategory, string minigameName, float minigameScore, int correctlyAnswered, float globalTime)
    {

        SetOwner(owner);
        SetCategory(minigameCategory);
        SetMinigameName(minigameName);
        SetDatePerformed(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        this.minigameScore = minigameScore;
        this.correctlyAnswered = correctlyAnswered;
        this.globalTime = globalTime;
    }
}
