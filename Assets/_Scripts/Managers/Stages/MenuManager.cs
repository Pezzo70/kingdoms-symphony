using System.Linq;
using Kingdom.Audio;
using Kingdom.Enums;
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

        inputModule.cancel.action.performed += delegate { };
    }

    public void ReturnFlow()
    {
        GameObject menu = childs.First(sc => sc.name == "Menu");
        GameObject containerActive = childs.FirstOrDefault(
            go => go.name != "Menu" && go.activeInHierarchy
        );

        if (containerActive != null)
        {
            AudioSystem.Instance.Play(UIAction.Return);
            containerActive.SetActive(false);
            menu.SetActive(true);
        }
    }
}
