using System.Collections;
using UnityEngine;

public class DisclaimerHandler : MonoBehaviour
{
    public Animator disclaimerAnimator;
    public Animator menuAnimator;
    public GameObject menu;

    void Start()
    {
        menu.SetActive(false);
        StartCoroutine(ShowDisclaimerAndWait());
    }

    private IEnumerator ShowDisclaimerAndWait()
    {
        disclaimerAnimator.SetBool("FadeIn", true);
        yield return new WaitForSeconds(15f);
        disclaimerAnimator.SetBool("FadeIn", false);
        disclaimerAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1.5f);
        menu.SetActive(true);
        var cv = menu.GetComponent<CanvasGroup>();
        cv.blocksRaycasts = false;
        menuAnimator.SetBool("FadeIn", true);
        yield return new WaitForSeconds(1f);
        cv.blocksRaycasts = true;
    }
}
