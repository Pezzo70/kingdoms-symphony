using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFlow : MonoBehaviour
{   
    GameObject[] childs;public void Start()
    {
        childs = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            childs[i] = transform.GetChild(i).gameObject;      
    }
    public void ReturnFlow()
    {
        var menu = childs.First(sc => sc.name == "Menu");
        var charSelec = childs.First(sc => sc.name == "CharacterSelection");

        if(charSelec?.activeInHierarchy ?? false)
        {
            charSelec.SetActive(false);
            menu.SetActive(true);
        }
        
    }

    public void RunScene()
    {
        SceneManager.LoadScene("FlorestScene");
    }
}