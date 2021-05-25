using System;

[Serializable] 
public class Date
{
    public int correctAnswers;
    public int minigameScore;
    public int questionsAsked;
    public int wrongAnswers;
}
[Serializable]
public class Game
{
    public Date date;
}
[Serializable]
public class Category
{
    public Game game;
}
[Serializable]
public class UserId
{
    public Category category;
}
[Serializable]
public class UserStatistics
{
    public UserId userId;
}

