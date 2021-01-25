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
        Resolution currentResolution = Screen.currentResolution;
        for(int i =0; i< supportedResolutions.Length; i++)
        {
            if (currentResolution.Equals(supportedResolutions[i]))
            {
                selectedResolutionIndex = i;
                break;
            }
        }

       // Debug.Log(currentResolution.width + "x" + currentResolution.height);

    }

    public static void ApplySettings()
    {
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
            string newResolutionOption = supportedResolutions[i].width + "x" + supportedResolutions[i].height;
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






}
