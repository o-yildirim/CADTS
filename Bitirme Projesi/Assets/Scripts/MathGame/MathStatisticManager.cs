using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MathStatisticManager : MonoBehaviour
{
    public static MathStatisticManager instance;
    private MathStatistic statistic;
    public float minigameScore;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void EvaluateValues(float score)
    {
        //detaylandırılabilir
        minigameScore = score;
    }

    public void InitializeStatisticObject()
    {
        statistic = new MathStatistic(
                                      DatabaseHandler.loggedInUser,
                                      "Math",
                                      "BalloonGame",
                                       minigameScore 
                                      );

    }
    public void InsertToDatabase()
    {
        DatabaseHandler.InsertStatistic(statistic);
    }
}
