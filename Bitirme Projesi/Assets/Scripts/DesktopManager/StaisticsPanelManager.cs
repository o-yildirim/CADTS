using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaisticsPanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        //Sayfa her açıldığında databaseden istatistik çekecek
        Debug.Log("Istatistik paneli açıldı");
       
        string email = "emre.kardaslar@gmail.com";
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
        });  
    }
}
