using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAbility : MonoBehaviour
{
    [SerializeField] private float interval;
    public GameObject triggerGameObject;
    private Coroutine coroutine;
    [SerializeField] bool isCoroutineRunning;
    void Start()
    {
        // Start the coroutine when the script is initialized
        //coroutine = StartCoroutine(TriggerCoroutine());
    }
    private void OnEnable()
    {
        if (isCoroutineRunning == false)
        {
            isCoroutineRunning = true;
            coroutine = StartCoroutine(TriggerCoroutine());     
        }
        
    }
    private IEnumerator TriggerCoroutine()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(interval); // Wait for the specified interval

            ActivateTrigger(); // Activate the trigger game object

            yield return new WaitUntil(() => !triggerGameObject.activeSelf); // Wait until the trigger game object is deactivated

            // Coroutine will continue to loop and wait for the next interval
            isCoroutineRunning = false;
        }
    }
    private void ActivateTrigger()
    {
        triggerGameObject.SetActive(true);
    }
}

