using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Effects;
using UnityEngine;

public class EffectAppliedHandler : MonoBehaviour
{
    public EffectTarget effectTarget;
    public GameObject effectTargetContainer;
    public GameObject effectPrefab;
    private List<GameObject> _effectsApplied;

    void Awake()
    {
        _effectsApplied = new List<GameObject>();
    }

    void OnEnable()
    {
        EventManager.AddEffect += HandleAddEffect;
        EventManager.RemoveEffect += HandleRemoveEffect;
        EventManager.MusicSheetOpen += HandleMusicSheetOpen;
    }

    void OnDisable()
    {
        EventManager.AddEffect -= HandleAddEffect;
        EventManager.RemoveEffect -= HandleRemoveEffect;
        EventManager.MusicSheetOpen -= HandleMusicSheetOpen;
    }

    private void HandleAddEffect(EffectDTO dto)
    {
        var effectTypesShouldAddTurns = new EffectType[]
        {
            EffectType.ReduceAvailableSheetBars,
            EffectType.CompleteMitigation,
            EffectType.PreventPlayerHeal,
            EffectType.PreventEnemyHeal,
            EffectType.AdditionalManaScrollCost,
            EffectType.ReduceMana,
            EffectType.Damage,
        };

        if (
            EffectsAndScrollsManager
                .Instance
                .onGoingEffects
                .Any(
                    obj =>
                        obj.GameObjectName == dto.GameObjectName && obj.EffectType == dto.EffectType
                ) && effectTypesShouldAddTurns.Contains(dto.EffectType)
        )
        {
            return;
        }

        GameObject prefab = Instantiate(effectPrefab, effectTargetContainer.transform);
        prefab.GetComponent<EffectApplied>().SetData(dto);
        _effectsApplied.Add(prefab);
    }

    private void HandleRemoveEffect(EffectDTO dto)
    {
        GameObject target = _effectsApplied.Find(
            obj => obj.GetComponent<EffectApplied>().effectDTO == dto
        );
        _effectsApplied.Remove(target);
        Destroy(target);
    }

    private void HandleMusicSheetOpen(bool open)
    {
        if (open)
        {
            _effectsApplied.ForEach(obj => obj.SetActive(false));
        }
        else
        {
            _effectsApplied.ForEach(obj => obj.SetActive(true));
        }
    }
}
