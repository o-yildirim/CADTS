using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StaisticsPanelManager : MonoBehaviour
{
    public Text percentageAmongUsers;
    public Text ownPerformanceText;
  

    
    private void OnEnable()
    {
        //Sayfa her açıldığında databaseden istatistik çekecek
        Debug.Log("Istatistik paneli açıldı");



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
        string category = "attention";
        string game = "UppercaseLetterGame";
        //Bu bilgiler Unity tarafından gelecek
        Dictionary<string, Statistic> statisticsToAnalyze = new Dictionary<string, Statistic>();
        int average = 0;

        DatabaseHandler.GetUserStatistics(email, category, game, statistics =>
        {
            if (statistics == null)
            {
                ownPerformanceText.text = "Bu oyuna ait bir istatistiğiniz bulunmamaktadır.";
                 return;
            }

            foreach (var statistic in statistics)
            {
               //Debug.Log($"{statistic.Key} tarihindeki skor: {statistic.Value.minigameScore}");
                statisticsToAnalyze.Add(statistic.Key, statistic.Value);
                average += statistic.Value.minigameScore;
            }

            average = (average - statistics.Values.Last().minigameScore) / (statisticsToAnalyze.Count - 1);
            int lastPerformance = statistics.Values.Last().minigameScore;

             Debug.Log("Ortalama: " + average);
             Debug.Log("Son performans" + lastPerformance);

            int difference = average - lastPerformance;
            int changePercentage = Math.Abs(difference) * 100 / average;
            if(difference > 0)//Kötü durum
            {
                
                ownPerformanceText.text = "Son performansınız, önceki performanslarınıza göre %" +
                                           changePercentage.ToString() +
                                           " daha kötü durumda.";
            }
            else if(difference < 0)// iyi durum
            {
                ownPerformanceText.text = "Son performansınız, önceki performanslarınıza göre %" +
                                           changePercentage.ToString() +
                                           " daha iyi durumda.";
            }
            else
            {
                ownPerformanceText.text = "Son performansınız, önceki performanslarınıza aynı seviyede seyretmiş.";
            }

         }
      );
       
     
        

    }
}
