using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class StaisticsPanelManager : MonoBehaviour
{

    private Dictionary<string, Statistic> statisticsToAnalyze = new Dictionary<string, Statistic>();
    private GlobalStatistic globalStatistic;

    public Text percentageAmongUsers;
    public Text ownPerformanceText;

    private float userAverageOverall;

    private float userAverageLastPerformanceExcluded;
    private float lastPerformance;




    private float globalAverage;


    private int ageGapLower;
    private int ageGapUpper;

    public int[,] ageGaps = new int[,] { { 0, 14 }, { 15, 24 }, { 25, 64 } ,{ 65, 150 } };


private void OnEnable()
    {

        string email = DatabaseHandler.loggedInUser.email;
        email = email.Replace(".", ",");
        string category = "attention"; //BURALAR
        string game = "UppercaseLetterGame"; // MINIGAME SCRIPTINDEN ATTRIBUTE OLARAK ÇEKİLECEK



        InitializeUserAverageAndLastPerformance(email, category, game);
        DetermineAgeGaps();
        InitializeGlobalAverage(category, game);


    }




    public void InitializeGlobalAverage(string category, string game)
    {
        DatabaseHandler.GetGlobalStatistic(category, game, ageGapLower, ageGapUpper, globalStatistic =>
        {

            if (globalStatistic == null)
            {
                percentageAmongUsers.text = "Bu oyuna ait yaş aralığınıza (" + ageGapLower + "-" + ageGapUpper + ") ait hiç bir istatistik bulunmamaktadır.";
                return;
            }

            this.globalStatistic = globalStatistic;
            globalAverage = globalStatistic.totalScore / globalStatistic.totalGamesPlayed;
            InformUserAboutGlobal();

        }

        );
    }

    private void InformUserAboutGlobal()
    {
        float globalAverageWithoutLastIncluded = (globalStatistic.totalScore - lastPerformance) / (globalStatistic.totalGamesPlayed - 1);
        float globalLastPerformanceDifference =  (globalAverageWithoutLastIncluded-lastPerformance);
        float globalLastPerformanceChangePercentage = Math.Abs(globalLastPerformanceDifference) * 100f / globalAverageWithoutLastIncluded;

        //Debug.Log(globalStatistic.averageScore);



        if (globalLastPerformanceDifference > 0)//Kötü durum
        {
            percentageAmongUsers.text = "Son performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                      "diğer oyuncuların performanslarına göre <color=red>%" +
                                       globalLastPerformanceChangePercentage.ToString("F1") +
                                       "</color> daha kötü durumda.\n\n";
        }
        else if (globalLastPerformanceDifference < 0)// iyi durum
        {
            percentageAmongUsers.text = "Son performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                      "diğer oyuncuların performanslarına göre <color=green>%" +
                                       globalLastPerformanceChangePercentage.ToString("F1") +
                                       "</color> daha iyi durumda.\n\n";

        }
        else
        {
            percentageAmongUsers.text = "Son performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                      "diğer oyuncuların performanslarına tamamen aynı seyretmiş.\n\n";

        }


        //Genel oyuncu ortalamasını global ile yani genelle kıyaslamak

        /* float globalAverageWithoutLastIncluded = (globalStatistic.totalScore - lastPerformance) / (globalStatistic.totalGamesPlayed - 1);
         float globalLastPerformanceDifference = (globalStatistic.totalScore - globalAverageWithoutLastIncluded) / (globalStatistic.totalGamesPlayed - 1);
         float globalLastPerformanceChangePercentage = Math.Abs(globalLastPerformanceDifference) * 100f / globalStatistic.averageScore;
         */




     
        float globalOverallPerformanceDifference = globalAverage - userAverageOverall;
        float globalOverallPerformanceChangePercentage = Math.Abs(globalOverallPerformanceDifference) * 100f / globalAverage;

        if (globalOverallPerformanceDifference > 0)//Kötü durum
        {
            percentageAmongUsers.text += "Genel performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                      "diğer oyuncuların performanslarına göre <color=red>%" +
                                       globalOverallPerformanceChangePercentage.ToString("F1") +
                                       "</color> daha kötü durumda.";
        }
        else if (globalOverallPerformanceDifference < 0)// iyi durum
        {
            percentageAmongUsers.text += "Genel performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                      "diğer oyuncuların performanslarına göre <color=green>%" +
                                       globalOverallPerformanceChangePercentage.ToString("F1") +
                                       "</color> daha iyi durumda.";

        }
        else
        {
            percentageAmongUsers.text += "Genel performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                        "diğer oyuncuların performanslarına tamamen aynı seyretmiş";

        }
    }

 

    private void InitializeUserAverageAndLastPerformance(string email, string category, string game)
    {

        DatabaseHandler.GetUserStatistics(email, category, game, statistics =>
        {
            if (statistics == null)
            {
                ownPerformanceText.text = "Bu oyuna ait bir istatistiğiniz bulunmamaktadır.";
                return;
            }
            else if (statistics.Count == 1)
            {
                ownPerformanceText.text = "Son performansınızın kıyaslanabileceği başka istatistiğiniz bulunmamaktadır.";
                return;
            }

            foreach (var statistic in statistics)
            {
                //Debug.Log($"{statistic.Key} tarihindeki skor: {statistic.Value.minigameScore}");
                statisticsToAnalyze.Add(statistic.Key, statistic.Value);
                userAverageOverall += statistic.Value.minigameScore;
            }



            userAverageLastPerformanceExcluded = (userAverageOverall - statistics.Values.Last().minigameScore) / (statisticsToAnalyze.Count - 1);
            userAverageOverall = userAverageOverall / statisticsToAnalyze.Count;

            if (userAverageLastPerformanceExcluded == 0f)//Önceki istatistiklerinin ortalaması 0 ise % hesaplayamayız.
            {
                ownPerformanceText.text = "Son performansınız önceki performanslarınıza göre çok daha iyi durumda.";
                return;
            }

            lastPerformance = statistics.Values.Last().minigameScore;

            InformUserComparedToHisOwn();

        }
      );
    }

    private void InformUserComparedToHisOwn()
    {
        float difference = userAverageLastPerformanceExcluded - lastPerformance;
        float changePercentage = Math.Abs(difference) * 100f / userAverageLastPerformanceExcluded;
        if (difference > 0)//Kötü durum
        {
            ownPerformanceText.text = "Son performansınız, önceki performanslarınıza göre <color=red>%" +
                                       changePercentage.ToString("F1") +
                                       "</color> daha kötü durumda.";
        }
        else if (difference < 0)// iyi durum
        {
            ownPerformanceText.text = "Son performansınız, önceki performanslarınıza göre <color=green>%" +
                                       changePercentage.ToString("F1") +
                                       "</color> daha iyi durumda.";

        }
        else
        {
            ownPerformanceText.text = "Son performansınız, önceki performanslarınıza aynı seviyede seyretmiş.";
        }
    }

    public int calculateUserAge(string dateOfBirth)
    {
        DateTime now = DateTime.Now;
        DateTime dateOfBirthDate = DateTime.Parse(dateOfBirth);

        TimeSpan diffTime = now.Subtract(dateOfBirthDate);
        int age = Mathf.RoundToInt((float) diffTime.TotalDays / 365f) ;
        
        return age;
    }


    private void DetermineAgeGaps()
    {
        int userAge = calculateUserAge(DatabaseHandler.loggedInUser.dob);

        for (var i = 0; i < 4; i++)
        {
            if (userAge >= ageGaps[i, 0] && userAge < ageGaps[i, 1])
            {
                ageGapLower = ageGaps[i, 0];
                ageGapUpper = ageGaps[i, 1];
                break;
            }
        }
    }

}

