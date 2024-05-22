using System.Collections.Generic;
using Kingdom.Effects;
using UnityEngine;

public class PlayerScrollsUsed : MonoBehaviour
{
    public GameObject scrollPrefab;
    public GameObject scrollsContainer;
    private List<GameObject> _scrollUseds;

    void Awake()
    {
        _scrollUseds = new List<GameObject>();
    }

    void OnEnable()
    {
        EventManager.AddScroll += HandleAddScroll;
        EventManager.ScrollRemoved += HandleScrollRemoved;
        EventManager.MusicSheetOpen += HandleMusicSheetOpen;
    }

    void OnDisable()
    {
        EventManager.AddScroll -= HandleAddScroll;
        EventManager.ScrollRemoved -= HandleScrollRemoved;
        EventManager.MusicSheetOpen -= HandleMusicSheetOpen;
    }

    private void HandleAddScroll(ScrollDTO scroll)
    {
        GameObject prefab = Instantiate(scrollPrefab, scrollsContainer.transform);
        prefab.GetComponent<ScrollUsed>().SetData(scroll);
        _scrollUseds.Add(prefab);
    }

    private void HandleScrollRemoved(ScrollDTO dto)
    {
        GameObject target = _scrollUseds.Find(
            obj => obj.GetComponent<ScrollUsed>().scrollDTO.Scroll.scrollID == dto.Scroll.scrollID
        );
        _scrollUseds.Remove(target);
        Destroy(target);
    }

    private void HandleMusicSheetOpen(bool open)
    {
        if (open)
        {
            _scrollUseds.ForEach(obj => obj.SetActive(false));
        }
        else
        {
            _scrollUseds.ForEach(obj => obj.SetActive(true));
        }
    }
}
