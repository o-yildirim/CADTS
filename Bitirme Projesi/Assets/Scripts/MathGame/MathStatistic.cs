using System;
using UnityEngine;

public class MathStatistic : Statistic
{
    public int wrongAttempts;
    public MathStatistic(User owner, string minigameCategory, string minigameName, float minigameScore, int wrongAttempts)
    {

        SetOwner(owner);
        SetCategory(minigameCategory);
        SetMinigameName(minigameName);
        SetDatePerformed(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        this.minigameScore = minigameScore;
        this.wrongAttempts = wrongAttempts;
    }
}
