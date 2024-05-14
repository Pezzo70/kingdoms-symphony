using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using Kingdom.UI;
using TMPro;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    public GameObject creditsTitle;
    public GameObject creditsPersona;

    public Animator backgroundAnimator;
    public Animator logoAnimator;

    public CreditsTitle[] titles;
    public CreditsPersona[] personas;

    private List<GameObject> _instantiatedObjects = new List<GameObject>();

    void OnEnable() => StartCoroutine(StartCredits());

    void OnDisable()
    {
        StopAllCoroutines();
        for (int i = 0; i < _instantiatedObjects.Count; i++)
            Destroy(_instantiatedObjects[i]);
        _instantiatedObjects.Clear();
    }

    private IEnumerator StartCredits()
    {
        backgroundAnimator.SetBool("FadeIn", true);

        yield return new WaitForSeconds(2);

        for (int i = 0; i < titles.Length; i++)
        {
            InstantiateChild(titles[i]);

            List<CreditsPersona> validPersonas = personas
                .Where(obj => obj.role.Contains(titles[i].UICreditTitle))
                .OrderBy(persona => persona.personaName)
                .ToList();

            yield return new WaitForSeconds(0.75f);

            for (int j = 0; j < validPersonas.Count; j++)
            {
                InstantiateChild(validPersonas[j]);
                yield return new WaitForSeconds(0.75f);
            }

            yield return new WaitForSeconds(1.75f);
        }

        while (_instantiatedObjects.Any(obj => obj.GetComponent<TextMeshProUGUI>().alpha > 0))
            yield return new WaitForSeconds(1f);

        logoAnimator.SetBool("FadeIn", true);

        yield return new WaitForSeconds(3f);

        logoAnimator.SetBool("FadeIn", false);
        logoAnimator.SetBool("FadeOut", true);

        yield return new WaitForSeconds(2f);

        logoAnimator.SetBool("FadeOut", false);

        backgroundAnimator.SetBool("FadeIn", false);
        backgroundAnimator.SetBool("FadeOut", true);
        yield return new WaitForSeconds(2f);

        logoAnimator.SetBool("FadeOut", false);

        FindObjectOfType<MenuManager>().ReturnFlow();
    }

    private void InstantiateChild(ScriptableObject so)
    {
        GameObject targetPrefab = null;
        string content = "";
        bool localize = false;

        switch (so)
        {
            case CreditsTitle ct:
                targetPrefab = creditsTitle;
                content = ct.key;
                localize = true;
                break;
            case CreditsPersona cp:
                targetPrefab = creditsPersona;
                content = cp.personaName;
                break;
        }

        GameObject prefab = Instantiate(targetPrefab);
        TextMeshProUGUI tmp = prefab.GetComponent<TextMeshProUGUI>();
        Animator animator = prefab.GetComponent<Animator>();
        if (localize)
        {
            LocalizedTextMeshProUGUI localizedTMP = prefab.GetComponent<LocalizedTextMeshProUGUI>();
            localizedTMP.LocalizationKey = "Menu.Credits." + content;
        }
        else
        {
            tmp.text = content;
        }

        _instantiatedObjects.Add(prefab);
        prefab.transform.SetParent(this.transform);
        animator.SetBool("RollAndFade", true);
    }
}
