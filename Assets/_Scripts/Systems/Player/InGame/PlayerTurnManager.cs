using System.Collections;
using System.Linq;
using Kingdom;
using Kingdom.Audio;
using Kingdom.Audio.Procedural;
using Kingdom.Effects;
using Kingdom.Enemies;
using Kingdom.Enums;
using Kingdom.Enums.Enemies;
using Kingdom.Enums.FX;
using Kingdom.Level;
using Kingdom.Player;
using UnityEngine;

public class PlayerTurnManager : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject playersOptions;
    public GameObject playerEffectIcon;
    public MusicSheet musicSheet;
    public GameObject pianoPortal;
    public Animator pianoPortalAnimator;
    public ParticleSystem pianoPortalParticleSystem;
    private Scroll _scrollOpen;

    private bool _enemiesTakingDamage = false;
    private bool _isDead = false;
    private bool _win = false;
    private bool _alternate = false;

    void Start()
    {
        PlaythroughContainer.Instance.currentTurn.Item2 = 0;
        EventManager.TurnChanged?.Invoke(Turn.PlayersTurn);
    }

    void OnEnable()
    {
        EventManager.TurnChanged += OnTurnChanged;
        EventManager.EnemyAttackExecuted += HandleEnemyAttack;
        EventManager.EnemiesEndTakingDamage += HandleEnemiesEndTakingDamage;
        EventManager.OnPlayersDeath += HandlePlayersDeath;
        EventManager.OnVictory += HandleOnVictory;
        EventManager.OpenScroll += HandleOpenScroll;
        EventManager.DamageEffectExecuted += HandleDamageEffectDTO;
    }

    void OnDisable()
    {
        EventManager.TurnChanged -= OnTurnChanged;
        EventManager.EnemyAttackExecuted -= HandleEnemyAttack;
        EventManager.EnemiesEndTakingDamage -= HandleEnemiesEndTakingDamage;
        EventManager.OnPlayersDeath -= HandlePlayersDeath;
        EventManager.OnVictory -= HandleOnVictory;
        EventManager.OpenScroll -= HandleOpenScroll;
        EventManager.DamageEffectExecuted -= HandleDamageEffectDTO;
    }

    public void SetWasOpenSheet(bool wasOpen)
    {
        musicSheet.wasOpen = wasOpen;
        EventManager.MusicSheetOpen?.Invoke(wasOpen);
    }

    private void HandleOpenScroll(Scroll scroll) => _scrollOpen = scroll;

    private void OnTurnChanged(Turn turn)
    {
        switch (turn)
        {
            case Turn.PlayersTurn:
                OnPlayerTurn();
                break;
            case Turn.EnemiesTurn:
                OnEnemyTurn();
                break;
        }
    }

    private void OnPlayerTurn()
    {
        float _ = 0f;

        EffectsAndScrollsManager
            .Instance
            .onGoingEffects
            .Where(
                obj =>
                    obj.Target == this.gameObject
                    && obj.EffectTarget == EffectTarget.Player
                    && obj.TriggerOnTurnStart == true
            )
            .ToList()
            .ForEach(obj => TheoryHandler.ValidateAndExecuteEffectAction(obj, ref _));

        if (_isDead || _win)
            return;

        EventManager.CantPause?.Invoke(false);
        PlayerStats ps = PlaythroughContainer.Instance.PlayerStats;

        int manaAffectedByEffects = EffectsAndScrollsManager
            .Instance
            .onGoingEffects
            .Where(
                obj =>
                    obj.Target == this.gameObject
                    && obj.EffectTarget == EffectTarget.Player
                    && (
                        obj.EffectType == EffectType.AdditionalMana
                        || obj.EffectType == EffectType.ReduceMana
                    )
            )
            .Aggregate(0, (total, next) => total + (int)next.Modifier);

        ps.ChangeManaPerTurn(ps.MaxMana + manaAffectedByEffects);
        ps.GainMana(ps.ManaPerTurn, false);

        EffectsAndScrollsManager
            .Instance
            .onGoingEffects
            .Where(
                obj =>
                    obj.Target == this.gameObject
                    && obj.EffectTarget == EffectTarget.Player
                    && obj.EffectType == EffectType.SpendMana
            )
            .ToList()
            .ForEach(obj => TheoryHandler.ValidateAndExecuteEffectAction(obj, ref _));

        playersOptions.SetActive(true);
    }

    public void CastScroll()
    {
        PlaythroughContainer.Instance.PlayerStats.SpendMana(_scrollOpen.manaRequired);
        EventManager.ScrollUsed?.Invoke(_scrollOpen.scrollID);
        EventManager.CastScroll?.Invoke(_scrollOpen);
    }

    private void OnEnemyTurn() => playersOptions.SetActive(false);

    private void HandleOnVictory(bool endGame) => StartCoroutine(OnVictoryCoroutine(endGame));

    private void HandleEnemiesEndTakingDamage() => _enemiesTakingDamage = false;

    public void EndPlayersTurn() => StartCoroutine(EndPlayersTurnRoutine());

    private (float damage, float massiveDamage) GetAttackDamage()
    {
        float damage = 0f;
        float massiveDamage = 0f;

        EffectsAndScrollsManager
            .Instance
            .onGoingScrolls
            .ForEach(obj =>
            {
                TheoryHandler.ValidateAndExecuteScrollAction(obj, ref damage, ref massiveDamage);
                EventManager.BurnScroll?.Invoke(obj.Scroll);
            });

        EffectsAndScrollsManager
            .Instance
            .onGoingEffects
            .Where(
                obj =>
                    obj.EffectTarget == EffectTarget.Player
                    && obj.EffectType == EffectType.DamageModifier
                    && obj.Target == this.gameObject
            )
            .ToList()
            .ForEach(effect => TheoryHandler.ValidateAndExecuteEffectAction(effect, ref damage));

        return (damage, massiveDamage);
    }

    private void HandlePlayersDeath()
    {
        if (_isDead)
            return;
        _isDead = true;
        playersOptions.SetActive(false);
        AudioSystem
            .Instance
            .Play(PlaythroughContainer.Instance.currentCharacterID, ActorAudioTypes.Damage);
        StartCoroutine(PlayersDeathCoroutine());
    }

    private IEnumerator EndPlayersTurnRoutine()
    {
        EventManager.CantPause?.Invoke(true);
        playersOptions.SetActive(false);
        pianoPortal.SetActive(true);
        AudioSystem.Instance.Play(FXID.Portal, FXState.Start);
        pianoPortalAnimator.Play("Portal_Spawn");
        yield return new WaitForSeconds(1f);
        pianoPortalAnimator.SetBool("Idle", true);
        yield return new WaitForSeconds(1f);

        Instrument instrument = GameObject
            .FindWithTag("InstrumentSource")
            .GetComponent<Instrument>();

        GameObject musicSheetParent = musicSheet.gameObject.transform.parent.gameObject;
        CanvasGroup musicSheetCanvas = musicSheetParent.GetComponent<CanvasGroup>();
        musicSheetCanvas.blocksRaycasts = false;
        musicSheetCanvas.alpha = 0;
        musicSheetCanvas.gameObject.SetActive(true);

        musicSheet.Play();
        pianoPortalParticleSystem.Play();

        string attackKey;

        if (_alternate)
        {
            _alternate = false;
            attackKey = "Attack_01";
        }
        else
        {
            _alternate = true;
            attackKey = "Attack_02";
        }

        playerAnimator.Play(attackKey);

        while (instrument.KeysPlayed.Count > 0)
            yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(1f);

        playerAnimator.Play("Idle");
        pianoPortalParticleSystem.Stop();

        (float damage, float massiveDamage) = GetAttackDamage();

        EventManager.EnemiesDamaged?.Invoke(damage, massiveDamage);

        _enemiesTakingDamage = true;
        while (_enemiesTakingDamage)
            yield return new WaitForSeconds(1f);

        //@TO-DO
        //DISPLAY ACCOMPLISHED SCROLLS
        //DISPLAY FAILED SCROLLS

        musicSheet.Clear();
        musicSheetCanvas.gameObject.SetActive(false);
        musicSheetCanvas.blocksRaycasts = true;
        musicSheetCanvas.alpha = 1;

        AudioSystem.Instance.Play(FXID.Portal, FXState.End);
        pianoPortalAnimator.SetBool("Idle", false);

        yield return new WaitForSeconds(1.5f);

        pianoPortal.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        EventManager.TurnChanged?.Invoke(Turn.EnemiesTurn);
    }

    private void HandleEnemyAttack(EnemyID enemyID, EnemyAttackID enemyAttackID)
    {
        if (_isDead)
            return;

        EnemyAttack enemyAttack = EnemiesContainer
            .Instance
            .enemies
            .First(enemy => enemy.enemyID == enemyID)
            .attacks
            .First(attack => attack.enemyAttackID == enemyAttackID);

        StartCoroutine(DamageCoroutine(enemyAttack.attackSymbol));
    }

    private void HandleDamageEffectDTO(EffectDTO effect) =>
        StartCoroutine(DamageCoroutine(effect.EffectIcon));

    private IEnumerator DamageCoroutine(Sprite icon)
    {
        SpriteRenderer spr = playerEffectIcon.GetComponent<SpriteRenderer>();
        spr.sprite = icon;
        Animator playerEffectIconAnimator = playerEffectIcon.GetComponent<Animator>();
        playerEffectIconAnimator.SetBool("EffectIconShow", true);
        playerAnimator.Play("Damage");
        yield return new WaitForSeconds(1f);
        playerAnimator.Play("Idle");
        playerEffectIconAnimator.SetBool("EffectIconShow", false);
        spr.color = new Color(255, 255, 255, 0f);
    }

    private IEnumerator PlayersDeathCoroutine()
    {
        playerAnimator.Play("Death");
        yield return new WaitForSeconds(2f);
        EventManager.EndGameDefeat?.Invoke();
    }

    private IEnumerator OnVictoryCoroutine(bool endGame)
    {
        playerAnimator.Play("Dance");
        _win = true;
        yield return new WaitForSeconds(3f);
        if (endGame)
            EventManager.EndGameVictory?.Invoke();
        else
            EventManager.PhaseVictory?.Invoke();
    }
}
