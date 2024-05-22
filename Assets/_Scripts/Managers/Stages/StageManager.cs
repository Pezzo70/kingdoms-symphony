using System.Collections;
using Kingdom.Audio;
using Kingdom.Effects;
using Kingdom.Enums.Player;
using Kingdom.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public void LoadScene(int sceneID)
    {
        EventManager.LevelTransition?.Invoke(Kingdom.Enums.LevelTransitionOption.In);
        StartCoroutine(WaitToLoad(sceneID));
    }

    public void SurrenderAndGoToMenu()
    {
        GameObject pauseGameObject = FindAnyObjectByType<PauseHandler>()
            .gameObject
            .transform
            .GetChild(0)
            .gameObject;
        EventManager
            .ShowPopUp
            ?.Invoke(
                (
                    "InGame.PopUp.Message.0",
                    false,
                    true,
                    () =>
                    {
                        pauseGameObject.SetActive(false);
                        EffectsAndScrollsManager.Instance.ClearAllEffectsAndScrolls();
                        EventManager
                            .LevelTransition
                            ?.Invoke(Kingdom.Enums.LevelTransitionOption.In);
                        StartCoroutine(WaitToLoad(0));
                    },
                    false,
                    0f,
                    false,
                    Vector3.zero,
                    true
                )
            );
    }

    public void StartNewRun(int characterID)
    {
        CharacterID character = (CharacterID)characterID;
        PlaythroughContainer.Instance.CreateNewPlaythrough(character);
        EffectsAndScrollsManager.Instance.ClearAllEffectsAndScrolls();
        EventManager.LevelTransition?.Invoke(Kingdom.Enums.LevelTransitionOption.In);
        StartCoroutine(WaitToLoad(1));
    }

    public void GoToNextLevel()
    {
        Level nextLevel = PlaythroughContainer.Instance.GetNextLevel();
        EffectsAndScrollsManager.Instance.ClearAllEffectsAndScrolls();
        EventManager.LevelTransition?.Invoke(Kingdom.Enums.LevelTransitionOption.In);
        StartCoroutine(WaitToLoad(nextLevel.sceneID));
    }

    public void EndGame()
    {
        EffectsAndScrollsManager.Instance.ClearAllEffectsAndScrolls();
        EventManager.LevelTransition?.Invoke(Kingdom.Enums.LevelTransitionOption.In);
        StartCoroutine(WaitToLoad(0));
    }

    private IEnumerator WaitToLoad(int sceneID)
    {
        AudioSystem.Instance.OnSceneUnloaded();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(sceneID);
    }
}
