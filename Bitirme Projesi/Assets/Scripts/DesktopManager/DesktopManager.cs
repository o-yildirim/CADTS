using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
public class DesktopManager : MonoBehaviour
{
    public Button[] desktopButtons;
    public GameObject[] desktopPanels;

  

    public Dictionary<Button,GameObject> panelButtonMatch; 

    public Color selectedButtonColor;
    public Color unselectedColor = Color.gray;
   
    public void Start()
    {
        panelButtonMatch = new Dictionary<Button, GameObject>();

        AssignListenerToButtons();
        AssignDictionary();

        ResetButtonColors();
        ResetPanels();

        ActivatePanel(desktopButtons[0], desktopPanels[0]);
    }
 
    public void ResetButtonColors()
    {
        for(int i = 0; i< desktopButtons.Length; i++)
        {
            Image buttonImage = desktopButtons[i].GetComponent<Image>();
            buttonImage.color = unselectedColor;
        }
    }

    public void ResetPanels()
    {
        for(int i = 0; i < desktopPanels.Length; i++)
        {
            desktopPanels[i].SetActive(false);
        }
    }

    public void PanelSelect()
    {
        Debug.Log("PanelSelect");
        GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
        Button clickedPanelButton = selectedGameObject.GetComponent<Button>();
        Debug.Log("Clicked the button: " + clickedPanelButton.transform.name);
        if(clickedPanelButton != null)
        {
            GameObject panelToOpen;
            panelButtonMatch.TryGetValue(clickedPanelButton,out panelToOpen);
            Debug.Log("Panel to open: " + panelToOpen.transform.name);  
            if(panelToOpen!= null && !panelToOpen.activeSelf)
            {
                ResetButtonColors();
                ResetPanels();

                ActivatePanel(clickedPanelButton, panelToOpen);
               

            }
        }
        else { Debug.Log("Null"); }
    }

    public void AssignListenerToButtons()
    {
        for(int i = 0; i < desktopButtons.Length; i++)
        {
            desktopButtons[i].onClick.AddListener(PanelSelect);
        }
    }
    public void AssignDictionary()
    {

        for (int i = 0; i < desktopPanels.Length; i++)
        {
            panelButtonMatch.Add(desktopButtons[i], desktopPanels[i]);
           // Debug.Log(desktopButtons[i] + "  " + desktopPanels[i]);
        }
    
   }

    public void ActivatePanel(Button button, GameObject panel)
    {

        Debug.Log("Activate panel");

        button.GetComponent<Image>().color = selectedButtonColor;
        panel.SetActive(true);
    }
}
*/

public class DesktopManager : MonoBehaviour
{
    public Button[] desktopButtons;
    public GameObject[] desktopPanels;

    public Button[] gameCategoryButtons;
    public GameObject[] gameCategoryPanels;

    public Button[] minigameButtons;


    public Dictionary<Button, GameObject> panelButtonMatch;


    public Color selectedButtonColor;
    public Color unselectedColor;

    public void Start()
    {
        panelButtonMatch = new Dictionary<Button, GameObject>();

        AssignListenerToButtons();
        AssignDictionary();

        ResetButtonColors(desktopButtons);
        ResetPanels(desktopPanels);

        ResetButtonColors(gameCategoryButtons);
        ResetPanels(gameCategoryPanels);

        ActivatePanel(desktopButtons[0], desktopPanels[0]);
        ActivatePanel(gameCategoryButtons[0], gameCategoryPanels[0]);
    }

    public void ResetButtonColors(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Image buttonImage = buttons[i].GetComponent<Image>();
            buttonImage.color = unselectedColor;
        }
    }

    public void ResetPanels(GameObject[] panels)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
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

        for(int i = 0; i < minigameButtons.Length; i++)
        {
            minigameButtons[i].onClick.AddListener(SceneManagement.instance.loadMinigame);
        }

    }
    public void AssignDictionary()
    {
       

        for (int i = 0; i < desktopPanels.Length; i++)
        {
            panelButtonMatch.Add(desktopButtons[i], desktopPanels[i]);
            Debug.Log(desktopButtons[i] + "  " + desktopPanels[i]);
        }
        
        for(int i = 0; i<gameCategoryPanels.Length ; i++)
        {
            panelButtonMatch.Add(gameCategoryButtons[i], gameCategoryPanels[i]);
            Debug.Log(gameCategoryButtons[i] + "  " + gameCategoryPanels[i]);
        }

    }

    public void ActivatePanel(Button button, GameObject panel)
    {

        button.GetComponent<Image>().color = selectedButtonColor;
        panel.SetActive(true);
    }
}
