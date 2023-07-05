using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpWindow : MonoBehaviour
{
    public TMP_Text popupText;
    public GameObject window;
    private Animator popupAnimator;

    bool canPopUp;

    private void Awake()
    {
        popupAnimator = GetComponent<Animator>();
        window.SetActive(false);
        canPopUp=true;
    }
    public void ShowPopup(string text)
    {
        
        if (!canPopUp) return;
        window.SetActive(true);
        popupText.text = text;
        popupAnimator.Play("PopupAnimation");
        StartCoroutine(WaitBeforePopUp());
    }
    IEnumerator WaitBeforePopUp()
    {
        canPopUp = false;
        yield return new WaitForSeconds(3f);
        window.SetActive(false);
        canPopUp = true;
    }
}
