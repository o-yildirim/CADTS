using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Minigame : MonoBehaviour
{
    public string minigameName;
    public string minigameCategory;
    public int sceneIndex;

    public string displayName;

    void Start()
    {
        GetComponentInChildren<Text>().text = displayName;
    }
    
}
