using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public void LoadScene(int sceneID) => SceneManager.LoadScene(sceneID);

    public void LoadScene(Scene scene) => SceneManager.LoadScene(scene.name);

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);

    public void GoToMenu() => SceneManager.LoadScene("Menu");
}
