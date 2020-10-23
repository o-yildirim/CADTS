using System;

[Serializable]
public class Statistic
{
    protected User ownerOfTheStatistics;
    protected string minigameCategory;
    protected string minigameName;
    public int minigameScore;
   // public DateTime datePerformed;

   

    //Minigame ranking falan eklenebilir sıralaması oyuncunun vs.

    public string GetCategory()
    {
        return this.minigameCategory;
    }
    public string GetMinigameName()
    {
        return this.minigameName;
    }
    /*
    public int GetScore()
    {
        return this.minigameScore;
    }*/
    public User GetOwner()
    {
        return this.ownerOfTheStatistics;
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

}

