using System;

[Serializable]
public class Statistic
{
    protected User ownerOfTheStatistics;
    protected string minigameCategory;
    protected string minigameName;
    protected string datePerformed;
    public float minigameScore;
 

   

    //Minigame ranking falan eklenebilir sıralaması oyuncunun vs.

    public string GetCategory()
    {
        return minigameCategory;
    }
    public string GetMinigameName()
    {
        return minigameName;
    }
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
    public void SetOwner(User owner)
    {
        this.ownerOfTheStatistics = owner;
    }

    public void SetDatePerformed(string date)
    {
        this.datePerformed = date;
    }

}

