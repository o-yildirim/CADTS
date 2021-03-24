using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MathStatisticManager : MonoBehaviour
{
    public static MathStatisticManager instance;
    private MathStatistic statistic;
    public float minigameScore;
    public int wrongAttempts;

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

    public void EvaluateValues(float score, int wrongAttempts)
    {
        //detaylandırılabilir
        this.wrongAttempts = wrongAttempts;
        minigameScore = (float)(score - (3 * wrongAttempts));
    }

    public void InitializeStatisticObject()
    {
        statistic = new MathStatistic(
                                      DatabaseHandler.loggedInUser,
                                      "Math",
                                      "BalloonGame",
                                       minigameScore,
                                       wrongAttempts
                                      );

    }
    public void InsertToDatabase()
    {
        DatabaseHandler.InsertStatistic(statistic);
    }
}
