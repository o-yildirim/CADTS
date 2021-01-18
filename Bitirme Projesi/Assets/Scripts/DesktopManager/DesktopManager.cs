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

    

    public Button openSettingsButton;
    public GameObject settingsButtonsPanel;


    public Dictionary<Button, GameObject> panelButtonMatch;


    public Button[] statisticCategoriesButtons;
    public GameObject[] minigameSelectPanels;
    public Button[] displayStatisticMiniGameButtons;
    public Dictionary<Button, GameObject> statisticCategoryButtonPanelMatch;



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



        ActivatePanel(desktopButtons[0], desktopPanels[0]);

        int startingWindow = 0;
        ActivatePanel(gameCategoryButtons[startingWindow], gameCategoryPanels[startingWindow]);

        ActivatePanel(statisticCategoriesButtons[startingWindow], minigameSelectPanels[startingWindow]);
        ActivatePanel(displayStatisticMiniGameButtons[startingWindow], null);

        Debug.Log("Calling invoke");

       
        displayStatisticMiniGameButtons[startingWindow].onClick.Invoke();
        
        //StaisticsPanelManager.instance.DisplayForMinigame(displayStatisticMiniGameButtons[startingWindow].GetComponent<Minigame>());

        openSettingsButton.GetComponentInChildren<Text>().text = DatabaseHandler.loggedInUser.name;

    }

    public void ResetButtonColors(Button[] buttons)
    {
        /*
        for (int i = 0; i < buttons.Length; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            buttonImage.color = unselectedColor;

        }
        */
        for (int i = 0; i < buttons.Length; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            if(buttonImage.color == selectedButtonColor)
            {
                buttonImage.color = unselectedColor;
                break;
            }
           
        }
        
    }

    public void ResetPanels(GameObject[] panels)
    {

        /*
       for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        } 
        */

        
        for(int i = 0; i < panels.Length; i++)
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
                

                if (!clickedPanelButton.CompareTag("CategoryButton"))
                {                
                    ResetButtonColors(desktopButtons);
                    ResetPanels(desktopPanels);
                }
                else
                {
                    ResetButtonColors(gameCategoryButtons);
                    ResetPanels(gameCategoryPanels);
                }
                ActivatePanel(clickedPanelButton, panelToOpen);


            }

            StopAllCoroutines();
        }
     
    }


    public void StatitsicPanelSelect()
    {

        GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
        Button clickedPanelButton = selectedGameObject.GetComponent<Button>();

        if (clickedPanelButton != null)
        {
            GameObject panelToOpen;
            statisticCategoryButtonPanelMatch.TryGetValue(clickedPanelButton, out panelToOpen);

            if (panelToOpen != null && !panelToOpen.activeSelf) //BURALARDA SUB PANEL MI DIYE BIR IF KOYULACAK
            {


                if (!clickedPanelButton.CompareTag("StatisticButton"))
                {
                    ResetButtonColors(statisticCategoriesButtons);
                    ResetPanels(desktopPanels);
                }
                else
                {
                    ResetButtonColors(statisticCategoriesButtons);
                    ResetPanels(minigameSelectPanels);
                }
                ActivatePanel(clickedPanelButton, panelToOpen);



                StopAllCoroutines();

                Button firstMinigameButtonInCategory = panelToOpen.GetComponentInChildren<Button>();
                firstMinigameButtonInCategory.onClick.Invoke();
                firstMinigameButtonInCategory.GetComponent<Image>().color = selectedButtonColor;



            }
        }

    }


    public void AssignListenerToButtons()
    {
        for (int i = 0; i < desktopButtons.Length; i++)
        {
            desktopButtons[i].onClick.AddListener(PanelSelect);
        }

        for(int i = 0; i< gameCategoryButtons.Length; i++)
        {
            gameCategoryButtons[i].onClick.AddListener(PanelSelect);
        }

        for(int i = 0; i < statisticCategoriesButtons.Length; i++)
        {
            statisticCategoriesButtons[i].onClick.AddListener(StatitsicPanelSelect);
        }

        for(int i = 0; i < displayStatisticMiniGameButtons.Length; i++)
        {
            Minigame minigameScript = displayStatisticMiniGameButtons[i].GetComponent<Minigame>();
            displayStatisticMiniGameButtons[i].onClick.AddListener(() => { StaisticsPanelManager.instance.DisplayForMinigame(minigameScript); });
            Debug.Log("Added listener to: " + displayStatisticMiniGameButtons[i].name);
        }
     
        for(int i = 0; i < minigameButtons.Length; i++)
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
        
        for(int i = 0; i<gameCategoryPanels.Length ; i++)
        {
            panelButtonMatch.Add(gameCategoryButtons[i], gameCategoryPanels[i]);
           // Debug.Log(gameCategoryButtons[i] + "  " + gameCategoryPanels[i]);
        }
        for(int i = 0; i< statisticCategoriesButtons.Length; i++)
        {
            statisticCategoryButtonPanelMatch.Add(statisticCategoriesButtons[i], minigameSelectPanels[i]);
        }

    }

    public void ActivatePanel(Button button, GameObject panel)
    {

        if (button != null)
        {
            button.GetComponent<Image>().color = selectedButtonColor;
        }
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void ManageSettingsButtonsPanel()
    {

        if (settingsButtonsPanel.activeSelf)
        {
            settingsButtonsPanel.SetActive(false);
        }
        else
        {
            settingsButtonsPanel.SetActive(true);
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
       // Debug.Log("Account Settings");
        //Ayrı bir canvas falan açılır
    }

    public void OpenAppSettings()
    {
       // Debug.Log("Application Settings");
        //Ayrı bir canvas falan açılır
    }

}
