using System;
public class UppercaseLetterStatistic : Statistic
{
    public int questionsAsked;
    public int correctAnswers;
    public int wrongAnswers;

    public UppercaseLetterStatistic(User owner,string minigameCategory,string minigameName,int minigameScore,int questionsAsked,int correctAnswers,int wrongAnswers)
    {
        //----------------Super class attributeları---------
        SetOwner(owner);
        SetCategory(minigameCategory);
        SetMinigameName(minigameName);

        //SetScore(minigameScore);

        this.datePerformed = DateTime.Now.ToString();
        this.minigameScore = minigameScore;

        //----------------------------------------------------



        this.questionsAsked = questionsAsked;
        this.correctAnswers = correctAnswers;
        this.wrongAnswers = wrongAnswers;
    }
}
