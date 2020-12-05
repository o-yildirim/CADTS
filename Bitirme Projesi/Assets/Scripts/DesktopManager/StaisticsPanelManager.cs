using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StaisticsPanelManager : MonoBehaviour
{
    public Text percentageAmongUsers;
    public Text ownPerformanceText;
  
    public int[,] ageGaps = new int[,] { { 0, 17 }, { 18, 24 }, { 25, 36 } };

private void OnEnable()
    {
        //Sayfa her açıldığında databaseden istatistik çekecek
       // Debug.Log("Istatistik paneli açıldı");



        /*string email = "emre.kardaslar@gmail.com";
        email = email.Replace(".", ",");
        string category = "attention";
        string game = "UppercaseLetterGame";
        //Bu bilgiler Unity tarafından gelecek
        Dictionary<string, Date> datesToAnalyze = new Dictionary<string, Date>();
        DatabaseHandler.GetUserStatistics(email, category, game, dates =>
        {
            foreach (var date in dates)
            {
                Debug.Log($"{date.Key} tarihindeki skor: {date.Value.minigameScore}");
                datesToAnalyze.Add(date.Key, date.Value);
            }
            Debug.Log("LOCAL VARIABLE YAZDIRIYOR: \n");
            foreach (var date in datesToAnalyze)
            {
                Debug.Log($"{date.Key.ToString()} tarihindeki skor: {date.Value.minigameScore}");
                //Local variable üzerinde işlem uygulayarak analiz yapılıp kullanıcıya bilgi verilebilir
            }
        });  */

        string email = DatabaseHandler.loggedInUser.email;
        email = email.Replace(".", ",");
        string category = "attention"; //BURALAR
        string game = "UppercaseLetterGame"; // MINIGAME SCRIPTINDEN ATTRIBUTE OLARAK ÇEKİLECEK
        float lastPerformance = 0f;
        float average = 0f;
        //Bu bilgiler Unity tarafından gelecek
        Dictionary<string, Statistic> statisticsToAnalyze = new Dictionary<string, Statistic>();
      

        DatabaseHandler.GetUserStatistics(email, category, game, statistics =>
        {
            if (statistics == null)
            {
                ownPerformanceText.text = "Bu oyuna ait bir istatistiğiniz bulunmamaktadır.";
                return;
            }
            else if(statistics.Count == 1)
            {
                ownPerformanceText.text = "Son performansınızın kıyaslanabileceği başka istatistiğiniz bulunmamaktadır.";
                return;
            }

            foreach (var statistic in statistics)
            {
               //Debug.Log($"{statistic.Key} tarihindeki skor: {statistic.Value.minigameScore}");
                statisticsToAnalyze.Add(statistic.Key, statistic.Value);
                average += statistic.Value.minigameScore;
            }

        
  
            average = (average - statistics.Values.Last().minigameScore) / (statisticsToAnalyze.Count - 1);

            if (average == 0f)//Önceki istatistiklerinin ortalaması 0 ise % hesaplayamayız.
            {
                ownPerformanceText.text = "Son performansınız önceki performanslarınıza göre çok daha iyi durumda.";
                return;
            }

            lastPerformance = statistics.Values.Last().minigameScore;

             //Debug.Log("Ortalama: " + average);
             //Debug.Log("Son performans " + lastPerformance);

            float difference = average - lastPerformance;
            float changePercentage = Math.Abs(difference) * 100f / average;
            if(difference > 0)//Kötü durum
            {
                ownPerformanceText.text = "Son performansınız, önceki performanslarınıza göre <color=red>%" +
                                           changePercentage.ToString("F1") +
                                           "</color> daha kötü durumda.";
            }
            else if(difference < 0)// iyi durum
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
      );


        int userAge = calculateUserAge(DatabaseHandler.loggedInUser.dob);
        int ageGapLower = 0, ageGapUpper = 0;

//        Debug.Log("ageGaps.Length =  " + ageGaps.Length + "     ageGaps.GetLength(0) = " + ageGaps.GetLength(0));

        for (var i = 0; i < 3; i++)
        {
                if (userAge >= ageGaps[i,0] && userAge < ageGaps[i,1])
                {
                    ageGapLower = ageGaps[i,0];
                    ageGapUpper = ageGaps[i,1];
                    break;
                } 
        }


        //Debug.Log("LowerGap: " + ageGapLower + "UpperGap " + ageGapUpper);

        Debug.Log(ageGapLower + "   " + ageGapUpper);

        DatabaseHandler.GetGlobalStatistic(category,game,ageGapLower,ageGapUpper, globalStatistic =>
        {
             Debug.Log("Global ortalama:" + globalStatistic.averageScore + " " + globalStatistic.totalGamesPlayed + " " + globalStatistic.totalScore);

            //Son performansı genelle kıyaslamak
            float globalLastPerformanceDifference = globalStatistic.averageScore - lastPerformance;
            float globalLastPerformanceChangePercentage = Math.Abs(globalLastPerformanceDifference) * 100f / globalStatistic.averageScore;

            


            if (globalLastPerformanceDifference > 0)//Kötü durum
            {
                percentageAmongUsers.text = "Son performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper +")  " +
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

            float globalOverallPerformanceDifference = globalStatistic.averageScore - average;
            float globalOverallPerformanceChangePercentage = Math.Abs(globalOverallPerformanceDifference) * 100f / globalStatistic.averageScore;

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

        );
       
     
        

    }

    public int calculateUserAge(string dateOfBirth)
    {

        DateTime now = DateTime.Now;
        DateTime dateOfBirthDate = DateTime.Parse(dateOfBirth);

        TimeSpan diffTime = now.Subtract(dateOfBirthDate);
        //Debug.Log("diffTime.ToString(): " + diffTime.ToString());
        //Debug.Log("diffTime: " + diffTime);
        int age = Mathf.RoundToInt((float) diffTime.TotalDays / 365f) ;
        //Debug.Log("diffTime.Days: " + diffTime.Days + " diffTime.TotalDays: " + diffTime.TotalDays);
        //Debug.Log("age: " + age);

        return age;
    }

}

