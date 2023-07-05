using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    

    public static event Action GameStarted; // Event declaration using Action delegate
    public static event Action GameOver; // Event declaration using Action delegate

    public bool StartGame;

    [SerializeField] bool isGameStarted;
    [SerializeField] bool isGameOver;
    
    //public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
    public bool IsGameStarted
    {
        get => isGameStarted; 
        set => isGameStarted = value;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void OnEnable()
    {
        UnitObjective.EndGame += InvokeGameOver;
    }
    private void OnDestroy()
    {
        UnitObjective.EndGame -= InvokeGameOver;
    }
    private void Update()
    {
        if (StartGame == false) return;
        if (!IsGameStarted)
        {
            GameStarted?.Invoke(); 
            IsGameStarted = true;
        }
    }
    void InvokeGameOver()
    {
        Debug.Log("InvokeGameManager");
        if (!IsGameOver)
        {
            Debug.Log("InvokeGameManager");
            GameOver?.Invoke();
            IsGameOver = true;
        }
    }
}
