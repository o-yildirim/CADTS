using System;
public class UppercaseLetterStatistic : Statistic
{
    public int questionsAsked;
    public int correctAnswers;
    public float averageReactionTime;

    public UppercaseLetterStatistic(User owner,string minigameCategory,string minigameName,float minigameScore,int questionsAsked,int correctAnswers,float averageReactionTime)
    {
        //----------------Super class attributes---------
        SetOwner(owner);
        SetCategory(minigameCategory);
        SetMinigameName(minigameName);      
        SetDatePerformed(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        this.minigameScore = minigameScore;

        //----------------------------------------------------


        this.averageReactionTime = averageReactionTime;
        this.questionsAsked = questionsAsked;
        this.correctAnswers = correctAnswers;
 
    }
}
