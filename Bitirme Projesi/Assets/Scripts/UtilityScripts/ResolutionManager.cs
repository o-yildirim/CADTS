using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class ResolutionManager
{
    public static Resolution[] supportedResolutions;
    public static string[] modes = { "Tam ekran", "Pencere"};
    
    public static int selectedResolutionIndex;
    public static int selectedModeIndex;

    public static bool fullScreen = true;

    public static void InitializeResolutions()
    {
        supportedResolutions = Screen.resolutions;       
    }

    public static void ApplySettings()
    {
        selectedResolutionIndex = SettingsManager.instance.screenResolutionDropdown.value;
        selectedModeIndex = SettingsManager.instance.screenModeDropdown.value;

        PlayerPrefs.SetInt("ResolutionIndex",selectedResolutionIndex);
        PlayerPrefs.SetInt("ModeIndex", selectedModeIndex);

        if (selectedModeIndex == 0) fullScreen = true;
        else if (selectedModeIndex == 1) fullScreen = false;


        Screen.SetResolution(supportedResolutions[selectedResolutionIndex].width, supportedResolutions[selectedResolutionIndex].height,fullScreen);
  
    }

    public static void InitializeModesToDropdown(Dropdown dropdownButton)
    {
        dropdownButton.ClearOptions();
        //List<string> modes = new List<string> { "Tam ekran","Pencere","Çerçevesiz pencere" };
        List<string> modes = new List<string> { "Tam ekran", "Pencere" };
        dropdownButton.AddOptions(modes);
        dropdownButton.onValueChanged.AddListener(ScreenModeChanged);
    }

    public static void InitializeResolutionsToDropdown(Dropdown dropdownButton)
    {
        dropdownButton.ClearOptions();

        List<string> resolutions = new List<string>();
        for (int i = 0; i < supportedResolutions.Length; i++)
        {
            string newResolutionOption = supportedResolutions[i].ToString();
            resolutions.Add(newResolutionOption);
        }
        dropdownButton.AddOptions(resolutions);
        dropdownButton.onValueChanged.AddListener(ResolutionValueChanged);
      

    }

    public static void ResolutionValueChanged(int newPos)
    {
        selectedResolutionIndex = newPos;
    }

    public static void ScreenModeChanged(int newMode)
    {
        selectedModeIndex = newMode;

        if (selectedModeIndex == 0) fullScreen = true;
        else if (selectedModeIndex == 1) fullScreen = false;
       

        
    }

    public static int GetSelectedResIndex()
    {
        Resolution currentResolution = new Resolution();
        currentResolution.width = Screen.width;
        currentResolution.height = Screen.height;
        currentResolution.refreshRate = Screen.currentResolution.refreshRate;

        int currentIndex = 0;

        for (int i = 0; i < supportedResolutions.Length; i++)
        {         
            if (currentResolution.Equals(supportedResolutions[i]))
            {
                currentIndex = i;
                break;
            }
        }

        //Debug.Log("Current: " + currentResolution.ToString());
        //Debug.Log("Selected: "+ supportedResolutions[currentIndex].ToString());
        return currentIndex;
    }

    public static int GetSelectedModeIndex()
    {
        if (Screen.fullScreen) return 0;
        else return 1;
    }

    public static void ResetSelected()
    {
        selectedResolutionIndex = GetSelectedResIndex();
        selectedModeIndex = GetSelectedModeIndex();
    }






}
