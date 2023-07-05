using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreenPopUp : MonoBehaviour
{
    [SerializeField] GameObject ObjectToActive;
    private void OnEnable()
    {
        GameManager.GameOver += SetThisActive;
    }
    private void OnDestroy()
    {
        GameManager.GameOver -= SetThisActive;
    }

    void SetThisActive()
    {
        if (!ObjectToActive.activeSelf)
        {
            ObjectToActive.SetActive(true);
        }
    }
}
