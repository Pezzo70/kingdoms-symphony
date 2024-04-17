using UnityEngine;

public class UIFadeController : MonoBehaviour
{
    Animator FadeController;

    void Start() => FadeController = GetComponent<Animator>();  
    void OnMouseEnter() => FadeController.Play("Fade_In");
    void OnMouseExit() => FadeController.Play("Fade_Out");     
}
