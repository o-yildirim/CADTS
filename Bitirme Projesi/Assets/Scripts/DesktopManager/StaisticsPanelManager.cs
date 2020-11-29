using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaisticsPanelManager : MonoBehaviour
{
    public Text percentageAmongUsers;
    public Text ownPerformanceText;

    Dictionary<string, Statistic> statisticsToAnalyze;
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
        statisticsToAnalyze = new Dictionary<string, Statistic>();
        int average = 0;
        int counter = 0;
        string lastPerformanceDate = "";
        DatabaseHandler.GetUserStatistics(email, category, game, statistics =>
        {
            if(statistics == null)
            {
                Debug.Log("Basaramadik abi");
                return;
            }

            foreach (var statistic in statistics)
            {
                
                //Debug.Log($"{statistic.Key} tarihindeki skor: {statistic.Value.minigameScore}");
                statisticsToAnalyze.Add(statistic.Key, statistic.Value);
                if(counter == statistics.Count)
                {
                    lastPerformanceDate = statistic.Key;
                    Debug.Log(statistics.Count);
                    break;
                }
                
                average += statistic.Value.minigameScore;
                counter++;
            }

     
            int currentPerformance = statisticsToAnalyze[lastPerformanceDate].minigameScore;
            //int currentPerformanceScore =

            average = average / counter;
            
            Debug.Log(average);
            Debug.Log(currentPerformance);

            /*foreach (var date in datesToAnalyze)
            {
                Debug.Log($"{date.Key.ToString()} tarihindeki skor: {date.Value.minigameScore}");
                //Local variable üzerinde işlem uygulayarak analiz yapılıp kullanıcıya bilgi verilebilir
            }*/
        });

    }
}
