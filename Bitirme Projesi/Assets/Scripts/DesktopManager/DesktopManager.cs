using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DesktopManager : MonoBehaviour
{
    public Button[] desktopButtons;
    public GameObject[] desktopPanels;

    public Button[] gameCategoryButtons;
    public GameObject[] gameCategoryPanels;

    public Button[] minigameButtons;

    public GameObject accountSettingsCanvas;
    public GameObject applicationSettingsCanvas;

    public Button openSettingsButton;
    public GameObject settingsButtonsPanel;
    public Image settingsBorder;


    public Dictionary<Button, GameObject> panelButtonMatch;


    public Button[] statisticCategoriesButtons;
    public GameObject[] minigameSelectPanels;
    public Button[] displayStatisticMiniGameButtons;
    public Dictionary<Button, GameObject> statisticCategoryButtonPanelMatch;


    public Image leftMenuBar;
    public Image upperMenuBar;
    public Image background;
   

    public Sprite[] leftMenuBarImages;
    public Sprite[] upperMenuBarImages;
    public Sprite[] backgroundImages;
 


    public Color selectedButtonColor;
    public Color unselectedColor;

    public void Start()
    {
        panelButtonMatch = new Dictionary<Button, GameObject>();
        statisticCategoryButtonPanelMatch = new Dictionary<Button, GameObject>();

        AssignListenerToButtons();
        AssignDictionary();

        ResetButtonColors(desktopButtons);
        ResetPanels(desktopPanels);

        ResetButtonColors(gameCategoryButtons);
        ResetPanels(gameCategoryPanels);



 

        desktopPanels[0].SetActive(true);

        Image firstButtonImage = desktopButtons[0].GetComponent<Image>();
        firstButtonImage.color = selectedButtonColor;
      

        int startingWindow = 0;

        ActivatePanel(statisticCategoriesButtons[startingWindow], minigameSelectPanels[startingWindow], true);
        ActivatePanel(displayStatisticMiniGameButtons[startingWindow], null, false);
        ActivatePanel(gameCategoryButtons[startingWindow], gameCategoryPanels[startingWindow], true);


        displayStatisticMiniGameButtons[startingWindow].onClick.Invoke();

        openSettingsButton.GetComponentInChildren<Text>().text = DatabaseHandler.loggedInUser.name;

        SettingsManager.instance.InitializePanels();



    }

    public void ResetButtonColors(Button[] buttons)
    {
        

        for (int i = 0; i < buttons.Length; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            if (buttonImage.color == selectedButtonColor)
            {
                buttonImage.color = unselectedColor;
                break;
            }

        }

    }

    public void ResetPanels(GameObject[] panels)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].activeSelf)
            {
                panels[i].SetActive(false);
                break;
            }
        }

    }

    public void PanelSelect()
    {

        GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
        Button clickedPanelButton = selectedGameObject.GetComponent<Button>();

        if (clickedPanelButton != null)
        {
            GameObject panelToOpen;
            panelButtonMatch.TryGetValue(clickedPanelButton, out panelToOpen);

            if (panelToOpen != null && !panelToOpen.activeSelf) 
            {

                bool isCategoryButton = false;
                if (!clickedPanelButton.CompareTag("CategoryButton"))
                {
                    ResetButtonColors(desktopButtons);
                    ResetPanels(desktopPanels);
                }
                else
                {
                    ResetButtonColors(gameCategoryButtons);
                    ResetPanels(gameCategoryPanels);
                    isCategoryButton = true;
                }
                ActivatePanel(clickedPanelButton, panelToOpen, isCategoryButton);


            }

            StopAllCoroutines();
        }

    }


    public void StatitsicPanelSelect()
    {
        StaisticsPanelManager.instance.mailAckText.text = "";

        GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
        Button clickedPanelButton = selectedGameObject.GetComponent<Button>();

        if (clickedPanelButton != null)
        {
            GameObject panelToOpen;
            statisticCategoryButtonPanelMatch.TryGetValue(clickedPanelButton, out panelToOpen);

            if (panelToOpen != null && !panelToOpen.activeSelf) 
            {

                bool isCategoryButton = false;
                if (!clickedPanelButton.CompareTag("StatisticButton"))
                {
                    ResetButtonColors(statisticCategoriesButtons);
                    ResetPanels(desktopPanels);
                }
                else
                {
                    ResetButtonColors(statisticCategoriesButtons);
                    ResetPanels(minigameSelectPanels);
                    isCategoryButton = true;
                }
                ActivatePanel(clickedPanelButton, panelToOpen, isCategoryButton);



                Button firstMinigameButtonInCategory = panelToOpen.GetComponentInChildren<Button>();
                firstMinigameButtonInCategory.GetComponent<Image>().color = selectedButtonColor;
                firstMinigameButtonInCategory.onClick.Invoke();

        

            }
        }

    }


    public void AssignListenerToButtons()
    {
        for (int i = 0; i < desktopButtons.Length; i++)
        {
            desktopButtons[i].onClick.AddListener(PanelSelect);
        }

        for (int i = 0; i < gameCategoryButtons.Length; i++)
        {
            gameCategoryButtons[i].onClick.AddListener(PanelSelect);
        }

        for (int i = 0; i < statisticCategoriesButtons.Length; i++)
        {
            statisticCategoriesButtons[i].onClick.AddListener(StatitsicPanelSelect);
        }

        for (int i = 0; i < displayStatisticMiniGameButtons.Length; i++)
        {
            Minigame minigameScript = displayStatisticMiniGameButtons[i].GetComponent<Minigame>();
            displayStatisticMiniGameButtons[i].onClick.AddListener(() => { StaisticsPanelManager.instance.DisplayForMinigame(minigameScript); });
        
        }

        for (int i = 0; i < minigameButtons.Length; i++)
        {
            Minigame minigameScript = minigameButtons[i].GetComponent<Minigame>();
            int indexOfThisMinigame = minigameScript.sceneIndex;
            minigameButtons[i].onClick.AddListener(() => { SceneManagement.instance.loadSceneCall(indexOfThisMinigame); });
 
        }

    }
    public void AssignDictionary()
    {


        for (int i = 0; i < desktopPanels.Length; i++)
        {
            panelButtonMatch.Add(desktopButtons[i], desktopPanels[i]);
           
        }

        for (int i = 0; i < gameCategoryPanels.Length; i++)
        {
            panelButtonMatch.Add(gameCategoryButtons[i], gameCategoryPanels[i]);
     
        }
        for (int i = 0; i < statisticCategoriesButtons.Length; i++)
        {
            statisticCategoryButtonPanelMatch.Add(statisticCategoriesButtons[i], minigameSelectPanels[i]);
        }

    }

    public void ActivatePanel(Button button, GameObject panel, bool isGamePanel)
    {

        if (button != null)
        {
            Image image = button.GetComponent<Image>();
            image.color = selectedButtonColor;
         
        }
      

        if (panel != null)
        {
            panel.SetActive(true);
            int index = 0;
            GameObject temporaryPanel = null;
            if (isGamePanel)
            {
                for (index = 0; index < gameCategoryButtons.Length; index++)
                {
                    if (gameCategoryPanels[index] == panel || minigameSelectPanels[index] == panel)
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < panel.transform.childCount; i++)
                {
                    temporaryPanel = panel.transform.GetChild(i).gameObject;
                    if (temporaryPanel.CompareTag("GamePanel") || temporaryPanel.CompareTag("StatisticGamePanel"))
                    {
                        if (temporaryPanel.activeSelf)
                        {
                            Image buttonImage;
                            if (temporaryPanel.CompareTag("GamePanel"))
                            {
                               
                               buttonImage = gameCategoryButtons[index].GetComponent<Image>();
                                    

                            }
                            else
                            {
                               buttonImage =  statisticCategoriesButtons[index].GetComponent<Image>();
                            }

                            buttonImage.color = selectedButtonColor;                         
                            break;
                        }
                        index++;
                      
                    }

                }
            }

            Debug.Log(index);
            
           
            upperMenuBar.sprite = upperMenuBarImages[index];
            leftMenuBar.sprite = leftMenuBarImages[index];


            if (panel.CompareTag("StatisticGamePanel") || (temporaryPanel && temporaryPanel.CompareTag("StatisticGamePanel")))
            {
      
                index = backgroundImages.Length - 1;
                
            }
         
       
        
            background.sprite = backgroundImages[index];


        }

    }




    public void ManageSettingsButtonsPanel()
    {

        if (settingsButtonsPanel.activeSelf)
        {
            settingsButtonsPanel.SetActive(false);
            settingsBorder.gameObject.SetActive(false);
        }
        else
        {
            settingsButtonsPanel.SetActive(true);
            settingsBorder.gameObject.SetActive(true);
        }
    }


    public void CallLogOut()
    {
        SettingsManager.instance.LogOut();
    }
    public void CallQuitRequest()
    {
        SettingsManager.instance.QuitRequest();
    }

    public void OpenAccountSettings()
    {
        SettingsManager.instance.CloseAllCanvasses();
        accountSettingsCanvas.SetActive(true);
    }

    public void OpenAppSettings()
    {
        SettingsManager.instance.CloseAllCanvasses();
        applicationSettingsCanvas.SetActive(true);
    }

}

