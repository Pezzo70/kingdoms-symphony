using System.Collections;
using Kingdom.Player;
using UnityEngine;

public class DisclaimerHandler : MonoBehaviour
{
    public Animator disclaimerAnimator;
    public Animator menuAnimator;
    public GameObject menu;

    void Start()
    {
        menu.SetActive(false);

        if (PlayerContainer.Instance.PlayerConfig.SawDisclaimer)
            StartCoroutine(DisplayMenu());
        else
        {
            PlayerContainer.Instance.PlayerConfig.SetSawDisclaimer(true);
            StartCoroutine(ShowDisclaimerAndWait());
        }
    }

    private IEnumerator ShowDisclaimerAndWait()
    {
        disclaimerAnimator.SetBool("FadeIn", true);
        yield return new WaitForSeconds(15f);
        disclaimerAnimator.SetBool("FadeIn", false);
        disclaimerAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(DisplayMenu());
    }

    private IEnumerator DisplayMenu()
    {
        menu.SetActive(true);
        var cv = menu.GetComponent<CanvasGroup>();
        cv.blocksRaycasts = false;
        menuAnimator.SetBool("FadeIn", true);
        yield return new WaitForSeconds(1f);
        cv.blocksRaycasts = true;
    }
}
