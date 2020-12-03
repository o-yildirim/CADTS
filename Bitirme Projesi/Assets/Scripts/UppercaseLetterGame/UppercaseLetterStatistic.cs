using System;
public class UppercaseLetterStatistic : Statistic
{
    public int questionsAsked;
    public int correctAnswers;
    public int wrongAnswers;
    public float averageReactionTime;

    public UppercaseLetterStatistic(User owner,string minigameCategory,string minigameName,float minigameScore,int questionsAsked,int correctAnswers,float averageReactionTime,int wrongAnswers)
    {
        //----------------Super class attributeları---------
        SetOwner(owner);
        SetCategory(minigameCategory);
        SetMinigameName(minigameName);
        //SetDatePerformed(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
        SetDatePerformed(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //SetScore(minigameScore);

        this.minigameScore = minigameScore;

        //----------------------------------------------------


        this.averageReactionTime = averageReactionTime;
        this.questionsAsked = questionsAsked;
        this.correctAnswers = correctAnswers;
        this.wrongAnswers = wrongAnswers;
    }
}
