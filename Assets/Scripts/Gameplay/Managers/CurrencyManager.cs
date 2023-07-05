using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager instance;

    [SerializeField] private int playerClonite;
    [SerializeField] private int playerScrap;
    [SerializeField] private bool isGameStarted;
    [SerializeField] float timeBetweenIncrement;

    private IEnumerator cloniteIncrementCoroutine;
    private bool isCoroutineRunning;
    

    public int PlayerClonite { get => playerClonite; set => playerClonite = value; }
    public int PlayerScrap { get => playerScrap; set => playerScrap = value; }
    public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }

    public static CurrencyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CurrencyManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("CurrencyManager");
                    instance = obj.AddComponent<CurrencyManager>();
                }
            }
            return instance;
        }
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
        GameManager.GameStarted += ChangeGameState;
    }
    private void OnDisable()
    {
        GameManager.GameStarted -= ChangeGameState;
    }
    private void Update()
    {   
        if (IsGameStarted && !isCoroutineRunning)
        {
            isCoroutineRunning = true;
            StartCloniteIncrement();
        }
    }

    public void StartCloniteIncrement()
    {
        Debug.Log("Start Increasing Clonite");
        if (cloniteIncrementCoroutine == null)
        {
            cloniteIncrementCoroutine = CloniteIncrementCoroutine();
            StartCoroutine(cloniteIncrementCoroutine);
        }
    }

    public void StopCloniteIncrement()
    {
        if (cloniteIncrementCoroutine != null)
        {
            StopCoroutine(cloniteIncrementCoroutine);
            cloniteIncrementCoroutine = null;
        }
    }

    private IEnumerator CloniteIncrementCoroutine()
    {
        while (true)
        {
            PlayerClonite++;
            yield return new WaitForSeconds(timeBetweenIncrement);
        }
    }
    //A really bad way to do this kind of stuff, well just keep it for demon for now
    void ChangeGameState()
    {
        IsGameStarted = true; 
    }
}

