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
    //public Image settingsImage;

    public Sprite[] leftMenuBarImages;
    public Sprite[] upperMenuBarImages;
    public Sprite[] backgroundImages;
    //public Image[] leftMenuBarImages;


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



        //ActivatePanel(desktopButtons[0], desktopPanels[0], false);

        desktopPanels[0].SetActive(true);

        int startingWindow = 0;
        ActivatePanel(gameCategoryButtons[startingWindow], gameCategoryPanels[startingWindow], true);

        ActivatePanel(statisticCategoriesButtons[startingWindow], minigameSelectPanels[startingWindow], true);
        ActivatePanel(displayStatisticMiniGameButtons[startingWindow], null, false);




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

            if (panelToOpen != null && !panelToOpen.activeSelf) //BURALARDA SUB PANEL MI DIYE BIR IF KOYULACAK
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

            if (panelToOpen != null && !panelToOpen.activeSelf) //BURALARDA SUB PANEL MI DIYE BIR IF KOYULACAK
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

                /*
                  for (int i = 0; i < panelToOpen.transform.childCount; i++)
                  {
                      Button button = panelToOpen.transform.GetChild(i).GetComponent<Button>();
                      if(button && button.gameObject.activeSelf)
                      {
                          button.GetComponent<Image>().color = selectedButtonColor;
                          button.onClick.Invoke();
                          Debug.Log(button.name);
                          break;                    
                      }
                  }
                */


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
            //Debug.Log("Added listener to: " + displayStatisticMiniGameButtons[i].name);
        }

        for (int i = 0; i < minigameButtons.Length; i++)
        {
            Minigame minigameScript = minigameButtons[i].GetComponent<Minigame>();
            int indexOfThisMinigame = minigameScript.sceneIndex;
            minigameButtons[i].onClick.AddListener(() => { SceneManagement.instance.loadSceneCall(indexOfThisMinigame); });
            //Debug.Log("Added listener to: " + minigameButtons[i].name + "with scene index:" + indexOfThisMinigame);
        }

    }
    public void AssignDictionary()
    {


        for (int i = 0; i < desktopPanels.Length; i++)
        {
            panelButtonMatch.Add(desktopButtons[i], desktopPanels[i]);
            //Debug.Log(desktopButtons[i] + "  " + desktopPanels[i]);
        }

        for (int i = 0; i < gameCategoryPanels.Length; i++)
        {
            panelButtonMatch.Add(gameCategoryButtons[i], gameCategoryPanels[i]);
            // Debug.Log(gameCategoryButtons[i] + "  " + gameCategoryPanels[i]);
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
            button.GetComponent<Image>().color = selectedButtonColor;
        }
        /* if (panel != null)
         {
             panel.SetActive(true);
             int index;

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
                 for (index = 0; index < gameCategoryPanels.Length; index++)
                 {
                     if (gameCategoryPanels[index].activeSelf|| minigameSelectPanels[index].activeSelf)
                     {
                         break;
                     }

                 }
             }

         */

        if (panel != null)
        {
            panel.SetActive(true);
            int index = 0;

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
                    Debug.Log(panel.transform.GetChild(i).name);
                    if (panel.transform.GetChild(i).CompareTag("GamePanel"))
                    {
                        if (panel.transform.GetChild(i).gameObject.activeSelf)
                        {
                            break;
                        }
                        index++;
                      
                    }

                }
            }

            Debug.Log(index);

            background.sprite = backgroundImages[index];
            upperMenuBar.sprite = upperMenuBarImages[index];
            leftMenuBar.sprite = leftMenuBarImages[index];


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

