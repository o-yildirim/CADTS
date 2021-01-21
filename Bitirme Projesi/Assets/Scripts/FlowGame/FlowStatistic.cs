using System;
using UnityEngine;

public class FlowStatistic : Statistic
{
    public int cycleCount;
    public int costOfThePathByPipes;
    public int wrongAttempts;
    public FlowStatistic(User owner, string minigameCategory, string minigameName, float minigameScore, int cycleCount, int costOfThePathByPipes, int wrongAttempts)
    {

        SetOwner(owner);
        SetCategory(minigameCategory);
        SetMinigameName(minigameName); 
        SetDatePerformed(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));  
        this.minigameScore = minigameScore;
        //----------------------------------------------------


        this.cycleCount = cycleCount;
        this.costOfThePathByPipes = costOfThePathByPipes;
        this.wrongAttempts = wrongAttempts;
       
    }
}
