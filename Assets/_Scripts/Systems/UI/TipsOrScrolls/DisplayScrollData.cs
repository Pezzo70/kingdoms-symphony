using System;
using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using Kingdom.Effects;
using Kingdom.Level;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScrollData : MonoBehaviour
{
    public GameObject castButton;
    public LocalizedTextMeshProUGUI title;
    public LocalizedTextMeshProUGUI objective;
    public LocalizedTextMeshProUGUI effect;
    public LocalizedTextMeshProUGUI cooldown;
    public GameObject manaContainer;
    public GameObject playerManaPrefab;
    public bool displayCastButton;
    private List<GameObject> _instantiadedMana = new List<GameObject>();

    void OnDisable()
    {
        for (int i = 0; i < _instantiadedMana.Count; i++)
        {
            Destroy(_instantiadedMana[i]);
        }

        _instantiadedMana.Clear();
        title.LocalizationKey = "";
        objective.LocalizationKey = "";
        effect.LocalizationKey = "";
    }

    public void SetDisplayData(Scroll scroll)
    {
        int additionalMana = EffectsAndScrollsManager
            .Instance
            .onGoingEffects
            .Where(
                obj =>
                    obj.EffectTarget == EffectTarget.Player
                    && obj.EffectType == EffectType.AdditionalManaScrollCost
            )
            .Aggregate(0, (total, next) => total + (int)next.Modifier);

        int manaCost = scroll.manaRequired + additionalMana;

        cooldown.LocalizationKey = "Scrolls.Menu.Cooldown";
        cooldown.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", scroll.cooldown.ToString())
            }
        );

        if (displayCastButton)
        {
            ScrollDTO used = EffectsAndScrollsManager
                .Instance
                .onGoingScrolls
                .Find(obj => obj.Scroll.scrollID == scroll.scrollID);

            BurnedScrollDTO burned = EffectsAndScrollsManager
                .Instance
                .burnedScrolls
                .Find(obj => obj.Scroll == scroll.scrollID);

            bool notEnoughManaToCast =
                manaCost > PlaythroughContainer.Instance.PlayerStats.CurrentMana;

            if (used != null || burned != null || notEnoughManaToCast)
            {
                castButton.GetComponent<Selectable>().interactable = false;
            }

            if (used != null)
            {
                cooldown.gameObject.SetActive(false);
            }
            else if (burned != null)
            {
                cooldown.gameObject.SetActive(true);
                cooldown.LocalizationKey = "Scrolls.Menu.Available";
                int turn = burned.CanBeUsedOnTurn - burned.internalCounter;
                cooldown.Replace(
                    new Tuple<string, string>[] { new Tuple<string, string>("-X", turn.ToString()) }
                );
                castButton.GetComponent<Selectable>().interactable = false;
            }
            else if (notEnoughManaToCast == false)
            {
                castButton.GetComponent<Selectable>().interactable = true;
            }

            castButton.SetActive(true);
        }
        else
            castButton.SetActive(false);

        title.LocalizationKey = "Scrolls.Name." + (int)scroll.scrollID;
        objective.LocalizationKey = "Scrolls.Objective." + (int)scroll.scrollID;
        effect.LocalizationKey = "Scrolls.Effect." + (int)scroll.scrollID;

        effect.Replace(
            new Tuple<string, string>[]
            {
                new Tuple<string, string>("-X", scroll.xFactor.ToString()),
                new Tuple<string, string>("-Y", scroll.yFactor.ToString()),
                new Tuple<string, string>("-Z", scroll.zFactor.ToString()),
                new Tuple<string, string>("-W", scroll.wFactor.ToString()),
            }
        );

        for (int i = 0; i < manaCost; i++)
        {
            GameObject manaInstance = Instantiate(playerManaPrefab, manaContainer.transform);
            _instantiadedMana.Add(manaInstance);
        }

        EventManager.OpenScroll?.Invoke(scroll);
    }
}
