using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticManager : MonoBehaviour
{
    public static StatisticManager instance;

    public GameObject statisticPanel;
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

 
    public void showStatistics()
    {
        this.questionAskedText.text = "Toplam sorulan soru sayısı: " +  GameController.instance.questionsAsked.ToString();
        this.correctAnsweredText.text ="Doğru cevaplanan soru sayısı:  " + GameController.instance.correctAnswered.ToString();
        this.scoreText.text ="Toplam skor: " + GameController.instance.score.ToString();
        this.statisticPanel.SetActive(true);
    }
    

}
