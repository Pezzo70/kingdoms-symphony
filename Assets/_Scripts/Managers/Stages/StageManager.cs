using System.Collections;
using Kingdom.Audio;
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
        EventManager.ShowPopUp(
            (
                "InGame.PopUp.Message.0",
                false,
                true,
                () =>
                {
                    pauseGameObject.SetActive(false);
                    EventManager.LevelTransition?.Invoke(Kingdom.Enums.LevelTransitionOption.In);
                    StartCoroutine(WaitToLoad(0));
                },
                false,
                0f,
                false,
                new Vector3(0f, 0f, 0f),
                true
            )
        );
    }

    public void StartNewRun(int characterID)
    {
        CharacterID character = (CharacterID)characterID;
        PlaythroughContainer.Instance.CreateNewPlaythrough(character);
        EventManager.LevelTransition?.Invoke(Kingdom.Enums.LevelTransitionOption.In);
        StartCoroutine(WaitToLoad(1));
    }

    public void GoToNextLevel()
    {
        Level nextLevel = PlaythroughContainer.Instance.GetNextLevel();
        EventManager.LevelTransition?.Invoke(Kingdom.Enums.LevelTransitionOption.In);
        StartCoroutine(WaitToLoad(nextLevel.sceneID));
    }

    public void EndGame()
    {
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
