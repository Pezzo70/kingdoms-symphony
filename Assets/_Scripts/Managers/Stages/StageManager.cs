using System.Collections;
using Kingdom.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public void LoadScene(int sceneID)
    {
        EventManager.LevelTransition.Invoke(Kingdom.Enums.LevelTransitionOption.In);
        StartCoroutine(WaitToLoad(sceneID));
    }

    public void GoToMenu() => SceneManager.LoadScene("MenuScene");

    private IEnumerator WaitToLoad(int sceneID)
    {
        AudioSystem.Instance.OnSceneUnloaded();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(sceneID);
    }
}
