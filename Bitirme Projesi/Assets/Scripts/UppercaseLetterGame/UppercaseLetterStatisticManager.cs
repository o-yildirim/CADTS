using UnityEngine;
using UnityEngine.UI;

public class UppercaseLetterStatisticManager : MonoBehaviour
{
    public static UppercaseLetterStatisticManager instance;


    public float reactionTimeMultiplier = 2.5f;
    public float correctAnswerMultiplier = 5f;
    public float wrongAnswerMultiplier = 2.5f;

    public GameObject statisticPanel;
    private UppercaseLetterStatistic statistic;

    public Text scoreText;
    public Text questionAskedText;
    public Text correctAnsweredText;
    public Text averageReactionTimeText;

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

        if (GameController.instance.reactionTimeAverage == 0f)
        {
            GameController.instance.score = 0f;
        }
        else
        {
            GameController.instance.score = ((1 / GameController.instance.reactionTimeAverage) * reactionTimeMultiplier) +
                                            (GameController.instance.correctAnswered * correctAnswerMultiplier) -
                                            (wrongAnswerMultiplier * (GameController.instance.questionsAsked - GameController.instance.correctAnswered - 1));
        }
        statistic = new UppercaseLetterStatistic(
                                        DatabaseHandler.loggedInUser,
                                        "attention",
                                        "UppercaseLetterGame",
                                         GameController.instance.score,
                                         GameController.instance.questionsAsked,
                                         GameController.instance.correctAnswered,
                                         GameController.instance.reactionTimeAverage                              
                                        );
    }
 
    public void ShowStatistics()
    {
        this.questionAskedText.text = "Toplam sorulan soru sayısı: " +  statistic.questionsAsked.ToString();
        this.correctAnsweredText.text ="Doğru cevaplanan soru sayısı:  " +statistic.correctAnswers.ToString();
        this.averageReactionTimeText.text = "Ortalama tepki süresi: " + statistic.averageReactionTime.ToString("F4") + " s";
        this.scoreText.text ="Toplam skor: " + statistic.minigameScore.ToString();
        this.statisticPanel.SetActive(true);
    }

    public void InsertStatistics()
    {
        DatabaseHandler.InsertStatistic(statistic);
    }
    
   

}
