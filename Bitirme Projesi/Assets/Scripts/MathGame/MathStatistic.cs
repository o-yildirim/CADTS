using System;
using UnityEngine;

public class MathStatistic : Statistic
{
    public MathStatistic(User owner, string minigameCategory, string minigameName, float minigameScore)
    {

        SetOwner(owner);
        SetCategory(minigameCategory);
        SetMinigameName(minigameName);
        SetDatePerformed(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        this.minigameScore = minigameScore;
    }
}
