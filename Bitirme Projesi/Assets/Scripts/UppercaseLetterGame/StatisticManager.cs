using UnityEngine;
using UnityEngine.UI;

public class StatisticManager : MonoBehaviour
{
    public static StatisticManager instance;

    public GameObject statisticPanel;
    private UppercaseLetterStatistic statistic;

    public Text scoreText;
    public Text questionAskedText;
    public Text correctAnsweredText;

    public Button returnToMenuButton;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        returnToMenuButton.onClick.AddListener(SceneManagement.instance.loadMainMenu);
    }

    public void InitializeStatistics()
    {
       
        //ALTTAKİ SATIRDA MİNİGAME OBJESİ KULLANILABİLİR BELKİ
        statistic = new UppercaseLetterStatistic(
                                        DatabaseHandler.loggedInUser,
                                        "attention",
                                        "UppercaseLetterGame",
                                         GameController.instance.score,
                                         GameController.instance.questionsAsked,
                                         GameController.instance.correctAnswered,
                                         GameController.instance.questionsAsked - GameController.instance.correctAnswered - 1 //mantık hatası olabilir
                                        );
    }
 
    public void ShowStatistics()
    {
        this.questionAskedText.text = "Toplam sorulan soru sayısı: " +  statistic.questionsAsked.ToString();
        this.correctAnsweredText.text ="Doğru cevaplanan soru sayısı:  " +statistic.correctAnswers.ToString();
        this.scoreText.text ="Toplam skor: " + statistic.minigameScore.ToString();
        this.statisticPanel.SetActive(true);
    }

    public void InsertStatistics()
    {
        DatabaseHandler.InsertStatistic(statistic);
    }
    
   

}
