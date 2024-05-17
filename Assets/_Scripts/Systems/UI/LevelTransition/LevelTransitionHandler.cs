using System.Collections;
using Kingdom.Audio;
using Kingdom.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitionHandler : PersistentSingleton<LevelTransitionHandler>
{
    public Animator circleBackgroundAnimator;
    public Image circleBackground;
    public Animator spinnerAnimator;
    private bool _firstTimeMenu = true;

    void OnEnable()
    {
        EventManager.LevelTransition += LevelTransitionEventHandler;
        SceneManager.sceneLoaded += HandleSceneLoader;
    }

    void OnDisable()
    {
        EventManager.LevelTransition -= LevelTransitionEventHandler;
        SceneManager.sceneLoaded -= HandleSceneLoader;
    }

    private void LevelTransitionEventHandler(LevelTransitionOption option)
    {
        switch (option)
        {
            case LevelTransitionOption.In:
                circleBackground.raycastTarget = true;
                StartCoroutine(HandleIn());
                break;
            case LevelTransitionOption.Out:
                StartCoroutine(HandleOut());
                break;
        }
    }

    private void HandleSceneLoader(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 0 && _firstTimeMenu)
        {
            _firstTimeMenu = false;
            return;
        }
        LevelTransitionEventHandler(LevelTransitionOption.Out);
    }

    private IEnumerator HandleIn()
    {
        circleBackgroundAnimator.Play("Circle_In");
        spinnerAnimator.Play("Fade_In");
        yield return new WaitForSeconds(2f);
        spinnerAnimator.Play("Loading_Spin");
    }

    private IEnumerator HandleOut()
    {
        yield return new WaitForSeconds(6f);
        AudioSystem.Instance.OnSceneLoaded(SceneManager.GetActiveScene());
        circleBackgroundAnimator.Play("Circle_Out");
        spinnerAnimator.Play("Fade_Out");
        yield return new WaitForSeconds(2f);
        circleBackground.raycastTarget = true;
    }
}
