﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticPanelManager : MonoBehaviour
{
    public static StatisticPanelManager instance;
    public GameObject statisticPanel;
    public bool isActive = false;

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

    void Start()
    {
        
    }

    void Update()
    {
        if (isActive)
        {
            statisticPanel.SetActive(true);
        }
    }
}
