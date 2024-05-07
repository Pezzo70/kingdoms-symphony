using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class MenuManager : MonoBehaviour
{
    GameObject[] childs;

    [SerializeField]
    GameObject canvas;
    InputSystemUIInputModule inputModule;

    public void Start()
    {
        inputModule = GetComponent<InputSystemUIInputModule>();

        childs = new GameObject[canvas.transform.childCount];
        for (int i = 0; i < childs.Length; i++)
            childs[i] = canvas.transform.GetChild(i).gameObject;

        inputModule.cancel.action.performed += delegate
        {
            ReturnFlow();
        };
    }

    public void ReturnFlow()
    {
        var menu = childs.First(sc => sc.name == "Menu");
        var sceneActive = childs.FirstOrDefault(go => go.name != "Menu" && go.activeInHierarchy);

        if (sceneActive != null)
        {
            AudioSystem.Instance.Play(UIAction.Return);
            sceneActive.SetActive(false);
            menu.SetActive(true);
        }
    }
}
