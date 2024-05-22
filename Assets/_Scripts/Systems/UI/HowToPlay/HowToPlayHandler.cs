using Assets.SimpleLocalization.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayHandler : MonoBehaviour
{
    public Sprite[] cards;
    public Image image;
    public LocalizedTextMeshProUGUI text;
    public TextMeshProUGUI page;

    private int _currentCardIndex = 0;

    void Start()
    {
        UpdateCardContent();
    }

    public void NextCard()
    {
        _currentCardIndex = _currentCardIndex == cards.Length - 1 ? 0 : _currentCardIndex + 1;
        UpdateCardContent();
    }

    public void PreviousCard()
    {
        _currentCardIndex = _currentCardIndex == 0 ? cards.Length - 1 : _currentCardIndex - 1;
        UpdateCardContent();
    }

    private void UpdateCardContent()
    {
        image.sprite = cards[_currentCardIndex];
        text.LocalizationKey = "HowToPlay." + _currentCardIndex.ToString();
        page.text = (_currentCardIndex + 1).ToString();
    }
}
