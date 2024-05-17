using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public void LoadScene(int sceneID)
    {
        EventManager.LevelTransition.Invoke(Kingdom.Enums.LevelTransitionOption.In);
        StartCoroutine(WaitToLoad(sceneID));
    }

    public void GoToMenu() => SceneManager.LoadScene("Menu");

    private IEnumerator WaitToLoad(int sceneID)
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(sceneID);
    }
}
