using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class ResolutionManager
{
    public static Resolution[] supportedResolutions;
    public static string[] modes = { "Tam ekran", "Pencere"};

    public static void InitializeResolutions()
    {
        supportedResolutions = Screen.resolutions;
    }
    public static void ApplySettings()
    {
        int selectedResolutionIndex = SettingsManager.instance.screenResolutionDropdown.value;
        int selectedModeIndex = SettingsManager.instance.screenModeDropdown.value;

        bool fullScreen = true;
        if (selectedModeIndex == 0) fullScreen = true;
        else if (selectedModeIndex == 1) fullScreen = false;

        Screen.SetResolution(supportedResolutions[selectedResolutionIndex].width, supportedResolutions[selectedResolutionIndex].height,fullScreen,supportedResolutions[selectedResolutionIndex].refreshRate); 
    }

    public static void InitializeModesToDropdown(Dropdown dropdownButton)
    {
        dropdownButton.ClearOptions();
        List<string> modes = new List<string> { "Tam ekran", "Pencere" };
        dropdownButton.AddOptions(modes);

        if (Screen.fullScreen) dropdownButton.value = 0;
        else dropdownButton.value = 1;

        dropdownButton.RefreshShownValue();
    }

    public static void InitializeResolutionsToDropdown(Dropdown dropdownButton)
    {
        dropdownButton.ClearOptions();

        List<string> resolutions = new List<string>();

        int currentResolution = 0;

        for (int i = 0; i < supportedResolutions.Length; i++)
        {
            string newResolutionOption = supportedResolutions[i].ToString();
            resolutions.Add(newResolutionOption);
            if(string.Equals(newResolutionOption,Screen.currentResolution.ToString()))
            {
                currentResolution = i;
            }
       
        }

        dropdownButton.AddOptions(resolutions);
        dropdownButton.value = currentResolution;
        dropdownButton.RefreshShownValue();
    }








}
