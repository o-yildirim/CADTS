using System;

[Serializable]
public class Statistic
{
    protected User ownerOfTheStatistics;
    protected string minigameCategory;
    protected string minigameName;
    protected string datePerformed;
    public int minigameScore;
 

   

    //Minigame ranking falan eklenebilir sıralaması oyuncunun vs.

    public string GetCategory()
    {
        return minigameCategory;
    }
    public string GetMinigameName()
    {
        return minigameName;
    }
    /*
    public int GetScore()
    {
        return this.minigameScore;
    }*/
    public User GetOwner()
    {
        return ownerOfTheStatistics;
    }

    public string GetDate()
    {
        return datePerformed;
    }


    public void SetCategory(string category)
    {
        this.minigameCategory = category;
    }
    public void SetMinigameName(string minigameName)
    {
        this.minigameName = minigameName;
    }
   /* public void SetScore(int score)
    {
        this.minigameScore = score;
    }*/
    public void SetOwner(User owner)
    {
        this.ownerOfTheStatistics = owner;
    }

    public void SetDatePerformed(string date)
    {
        this.datePerformed = date;
    }

}

