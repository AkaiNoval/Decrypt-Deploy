using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] List<GameObject> tutorialPanels = new List<GameObject>();
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] int popUpIndex;

    public int PopUpIndex { get => popUpIndex; set => popUpIndex = value; }

    private void Start()
    {
        Debug.Log(tutorialPanels.Count);
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < tutorialPanels.Count; i++)
        {
            if(i != PopUpIndex)
            {
                tutorialPanels[i].SetActive(false);
            }
        }
        if (PopUpIndex >= 0 && PopUpIndex < tutorialPanels.Count)
        {
            tutorialPanels[PopUpIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("Invalid index provided for panel activation!");
        }
        //for (int i = 0; i < tutorialPanels.Count; i++)
        //{
        //    if (i == PopUpIndex)
        //    {
        //        tutorialPanels[PopUpIndex].SetActive(true);
        //    }
        //    else
        //    {
        //        tutorialPanels[PopUpIndex].SetActive(false);
        //    }
        //}
    }
    public void NextPopUpIndex()
    {
        if (PopUpIndex == tutorialPanels.Count-1) return;
        PopUpIndex++;
            
    }
    public void BackPopUpIndex()
    {
        if (PopUpIndex == 0) return;
        PopUpIndex--;     
    }
    public void ReadyToBattle()
    {
        tutorialPanel.SetActive(false);
        GameManager.Instance.StartGame = true;
    }
}
