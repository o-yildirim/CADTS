﻿using UnityEngine;
using UnityEngine.UI;

public class StatisticPanelManager : MonoBehaviour
{
    public static StatisticPanelManager instance;
    public GameObject gameCanvas;
    public GameObject statisticPanel;
    public Button exitButton;

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
    }
}
