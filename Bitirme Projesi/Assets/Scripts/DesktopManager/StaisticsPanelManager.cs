using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaisticsPanelManager : MonoBehaviour
{

    public static StaisticsPanelManager instance;

    public GameObject panelToDisplay;
    public GameObject legend;
    public Button mailBtn;
    public Text mailAckText;
    public Dropdown mailOptions;
    private Dictionary<string, Statistic> statisticsToAnalyze = new Dictionary<string, Statistic>();
    private GlobalStatistic globalStatistic;

    public GraphicRaycaster raycaster;

    public Text titleText;
    public Text percentageAmongUsers;
    public Text lastPerformancePercentageAmongUsers;
    public Text ownPerformanceText;

    private float userAverageOverall;

    private float userAverageLastPerformanceExcluded;
    private float lastPerformance;


    private int userPerformanceCount = 0;

    private float globalAverage;

    private bool userStatsInitialized = false;
    private bool ageGapsDetermined = false;
    private bool globalInitialized = false;

    private bool doNotTouchOwnText = false;
    private bool doNotTouchGlobalText = false;

    private int ageGapLower;
    private int ageGapUpper;

    public Text loadingText;
    public float increaseTick = 0.2f;

    public GameObject overallToOverallPie;
    public Image pieOverallToOverall;

    public GameObject lastToOverallPie;
    public Image pieLastToOverall;

    private float successPercentageOverallToOverall;
    private float succesPercentageLastToOverall;

    public bool operatingForDisplay = false;


    public int[,] ageGaps = new int[,] { { 0, 14 }, { 15, 24 }, { 25, 64 }, { 65, 150 } };


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        mailOptions.ClearOptions();
        List<string> modes = new List<string> { "Kendime", "Yakınıma" };
        if (string.IsNullOrWhiteSpace(DatabaseHandler.loggedInUser.contactMail))
            modes.Remove("Yakınıma");

        mailOptions.AddOptions(modes);
       
        


    }

    public void InitializeGlobalAverage(string category, string game)
    {
        DatabaseHandler.GetGlobalStatistic(category, game, ageGapLower, ageGapUpper, globalStatistic =>
        {

            if (globalStatistic == null)
            {
                percentageAmongUsers.text = "Bu oyuna ait yaş aralığınıza (" + ageGapLower + "-" + ageGapUpper + ") ait hiç bir istatistik bulunmamaktadır.";
                globalInitialized = true;
                doNotTouchGlobalText = true;
                operatingForDisplay = false;
                raycaster.enabled = true;

                return;
            }

            this.globalStatistic = globalStatistic;
            globalAverage = globalStatistic.totalScore / globalStatistic.totalGamesPlayed;
            InformUserAboutGlobal();
            globalInitialized = true;

        }

        );

        
    }

    private void InformUserAboutGlobal()
    {
        if (!doNotTouchGlobalText && !doNotTouchOwnText)
        {
            activatePies();

            float globalAverageWithoutLastIncluded = (globalStatistic.totalScore - lastPerformance) / (globalStatistic.totalGamesPlayed - 1);
            float globalLastPerformanceDifference = (globalAverageWithoutLastIncluded - lastPerformance);
            float globalLastPerformanceChangePercentage = Math.Abs(globalLastPerformanceDifference) * 100f / globalAverageWithoutLastIncluded;
            

            if (globalLastPerformanceDifference > 0)
            {
                lastPerformancePercentageAmongUsers.text = "Son performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                          "diğer oyuncuların performanslarına göre <color=#FF7B7B>%" +
                                           globalLastPerformanceChangePercentage.ToString("F1") +
                                           "</color> daha kötü durumda.\n\n";
                succesPercentageLastToOverall = globalLastPerformanceChangePercentage;
                StartCoroutine(drawPie(succesPercentageLastToOverall, pieLastToOverall, Color.red));

            }
            else if (globalLastPerformanceDifference < 0)
            {
                lastPerformancePercentageAmongUsers.text = "Son performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                          "diğer oyuncuların performanslarına göre <color=#B0FC38>%" +
                                           globalLastPerformanceChangePercentage.ToString("F1") +
                                           "</color> daha iyi durumda.\n\n";
                succesPercentageLastToOverall = globalLastPerformanceChangePercentage;
                StartCoroutine(drawPie(succesPercentageLastToOverall, pieLastToOverall, Color.green));

            }
            else
            {
                lastPerformancePercentageAmongUsers.text = "Son performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                          "diğer oyuncuların performanslarına tamamen aynı seyretmiş.\n\n";

            }
          

          
            float globalOverallPerformanceDifference = globalAverage - userAverageOverall;
            float globalOverallPerformanceChangePercentage = Math.Abs(globalOverallPerformanceDifference) * 100f / globalAverage;
           
          

        

            if (globalOverallPerformanceDifference > 0)
            {
                percentageAmongUsers.text = "Genel performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                          "diğer oyuncuların performanslarına göre <color=#FF7B7B>%" +
                                           globalOverallPerformanceChangePercentage.ToString("F1") +
                                           "</color> daha kötü durumda.";

                successPercentageOverallToOverall = globalOverallPerformanceChangePercentage;
                StartCoroutine(drawPie(successPercentageOverallToOverall, pieOverallToOverall, Color.red));
            }
            else if (globalOverallPerformanceDifference < 0)
            {
                percentageAmongUsers.text = "Genel performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                          "diğer oyuncuların performanslarına göre <color=#B0FC38>%" +
                                           globalOverallPerformanceChangePercentage.ToString("F1") +
                                           "</color> daha iyi durumda.";

                successPercentageOverallToOverall = globalOverallPerformanceChangePercentage;
                StartCoroutine(drawPie(successPercentageOverallToOverall, pieOverallToOverall, Color.green));
            }
            else
            {
                percentageAmongUsers.text = "Genel performansınız, yaş aralığınızdaki (" + ageGapLower + "-" + ageGapUpper + ")  " +
                                            "diğer oyuncuların performanslarına tamamen aynı seyretmiş";

            }

        }
    }



    private void InitializeUserAverageAndLastPerformance(string email, string category, string game)
    {
       

        DatabaseHandler.GetUserStatistics(email, category, game, statistics =>
        {
            mailBtn.gameObject.SetActive(true);
            mailOptions.gameObject.SetActive(true);
            
            if (statistics == null || statistics.Count == 0)
            {
                ownPerformanceText.text = "Bu oyuna ait bir istatistiğiniz bulunmamaktadır.";
                mailBtn.gameObject.SetActive(false);
                mailOptions.gameObject.SetActive(false);
                userStatsInitialized = true;
                doNotTouchOwnText = true;
                
                operatingForDisplay = false;
                raycaster.enabled = true;
                return;
            }
            else if (statistics.Count == 1)
            {
                mailBtn.gameObject.SetActive(false);
                mailOptions.gameObject.SetActive(false);
                ownPerformanceText.text = "Son performansınızın kıyaslanabileceği başka istatistiğiniz bulunmamaktadır.";
                lastPerformance = statistics.Values.Last().minigameScore;
               
            }

            foreach (var statistic in statistics)
            {
           
                statisticsToAnalyze.Add(statistic.Key, statistic.Value);
                userAverageOverall += statistic.Value.minigameScore;
                userPerformanceCount++;
            }



            userAverageLastPerformanceExcluded = (userAverageOverall - statistics.Values.Last().minigameScore) / (statisticsToAnalyze.Count - 1);
            userAverageOverall = userAverageOverall / statisticsToAnalyze.Count;
           

            if (userAverageLastPerformanceExcluded == 0f)
            {
                ownPerformanceText.text = "Son performansınız önceki performanslarınıza göre çok daha iyi durumda.";
                return;
            }

            lastPerformance = statistics.Values.Last().minigameScore;
       

            userStatsInitialized = true;
            InformUserComparedToHisOwn();
            

        }
      );
    }

    private void InformUserComparedToHisOwn()
    {
        if (!doNotTouchOwnText)
        {
          

            float difference = userAverageLastPerformanceExcluded - lastPerformance;
            float changePercentage = Math.Abs(difference) * 100f / userAverageLastPerformanceExcluded;
            if (difference > 0)
            {
                ownPerformanceText.text = "Son performansınız, önceki performanslarınıza göre <color=#FF7B7B>%" +
                                           changePercentage.ToString("F1") +
                                           "</color> daha kötü durumda.";
            }
            else if (difference < 0)
            {
                ownPerformanceText.text = "Son performansınız, önceki performanslarınıza göre <color=#B0FC38>%" +
                                           changePercentage.ToString("F1") +
                                           "</color> daha iyi durumda.";

            }

            else
            {
                if (userPerformanceCount >= 2)
                {
                    ownPerformanceText.text = "Son performansınız, önceki performanslarınıza aynı seviyede seyretmiş.";
                }
            }
        }
    }

    public int calculateUserAge(string dateOfBirth)
    {
        DateTime now = DateTime.Now;
        DateTime dateOfBirthDate = DateTime.Parse(dateOfBirth);

        TimeSpan diffTime = now.Subtract(dateOfBirthDate);
        int age = Mathf.RoundToInt((float)diffTime.TotalDays / 365f);

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
        ageGapsDetermined = true;
    }

    public IEnumerator initDatabaseValues(string email, string category, string game)
    {

        raycaster.enabled = false;
        operatingForDisplay = true;
        

        panelToDisplay.SetActive(false);
        loadingText.enabled = true;
        statisticsToAnalyze.Clear();
        clearValues();

        InitializeUserAverageAndLastPerformance(email, category, game);
        while (!userStatsInitialized)
        {
            yield return null;

        }
        DetermineAgeGaps();
        while (!ageGapsDetermined)
        {
            yield return null;
        }
        InitializeGlobalAverage(category, game);
        while (!globalInitialized)
        {
            yield return null;
        }

        

        


        mailBtn.onClick.AddListener(() => {
            if (mailOptions.value == 0)
            {
                Debug.Log(email + " " + category + " " + game);
                string info = category + "," + game;
                Debug.Log(info);
                MailInfo mailInfo = new MailInfo(info);
                DatabaseHandler.sendMail(email, mailInfo);
            }

            else
            {
                Debug.Log(email + " " + category + " " + game);
                string contactMail = DatabaseHandler.loggedInUser.contactMail;
                string info = category + "," + game + "," + contactMail;
                Debug.Log(info);
                
                contactMail = contactMail.Replace(".", ",");
                MailInfo mailInfo = new MailInfo(info);
                DatabaseHandler.sendMailToContact(email, mailInfo);
            }     
        });



        loadingText.enabled = false;
        panelToDisplay.SetActive(true);
    
        
        raycaster.enabled = true;
        operatingForDisplay = false;


    }

    public void clearValues()
    {

        resetPies();
        deactivatePies();
        
        ownPerformanceText.text = "";
        percentageAmongUsers.text = "";
        lastPerformancePercentageAmongUsers.text = "";

        userStatsInitialized = false;
        globalInitialized = false;
        ageGapsDetermined = false;

        doNotTouchOwnText = false;
        doNotTouchGlobalText = false;

        userAverageOverall = 0f;
        userAverageLastPerformanceExcluded = 0f;
        lastPerformance = 0f;
        userPerformanceCount = 0;
        globalAverage = 0f;
        mailBtn.onClick.RemoveAllListeners();
    }

    public IEnumerator drawPie(float percentage,Image pieImage, Color color)
    {
        overallToOverallPie.SetActive(true);
        pieImage.fillAmount = 0f;
        pieImage.color = color;
        float angleToFill = percentage / 100f;
        

        while(pieImage.fillAmount <= angleToFill)
        {   
            float next = Mathf.MoveTowards(pieImage.fillAmount, angleToFill, increaseTick * Time.deltaTime);
            pieImage.fillAmount = next; 
            yield return null;
        }
        
    }

    public void activatePies()
    {
        legend.SetActive(true);
        lastToOverallPie.SetActive(true);
        overallToOverallPie.SetActive(true);
    }
    public void deactivatePies()
    {
        legend.SetActive(false);
        lastToOverallPie.SetActive(false);
        overallToOverallPie.SetActive(false);
    }

    public void resetPies()
    {
        pieLastToOverall.fillAmount = 0f;
        pieOverallToOverall.fillAmount = 0f;
    }
   

    public void DisplayForMinigame(Minigame gameToDisplay)
    {


        if (operatingForDisplay) return;

        string email = DatabaseHandler.loggedInUser.email;
        email = email.Replace(".", ",");
        string category = gameToDisplay.minigameCategory;
        string game = gameToDisplay.minigameName;
        titleText.text = gameToDisplay.displayName;

        StopAllCoroutines();
        StartCoroutine(initDatabaseValues(email, category, game));

    }
   
}



