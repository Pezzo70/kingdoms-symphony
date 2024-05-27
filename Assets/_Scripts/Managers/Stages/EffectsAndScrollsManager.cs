using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization.Scripts;
using Kingdom.Audio;
using Kingdom.Enums;
using Kingdom.Enums.MusicTheory;
using Kingdom.Enums.Scrolls;
using Kingdom.Level;
using UnityEngine;

namespace Kingdom.Effects
{
    public class EffectsAndScrollsManager : PersistentSingleton<EffectsAndScrollsManager>
    {
        [HideInInspector]
        public List<Note> playedNotes;
        public List<EffectDTO> onGoingEffects;
        public List<ScrollDTO> onGoingScrolls;
        public List<BurnedScrollDTO> burnedScrolls;

        protected override void Awake()
        {
            base.Awake();
            playedNotes = new List<Note>();
            onGoingEffects = new List<EffectDTO>();
            onGoingScrolls = new List<ScrollDTO>();
            burnedScrolls = new List<BurnedScrollDTO>();
        }

        void OnEnable()
        {
            EventManager.CastScroll += HandleCastScroll;
            EventManager.BurnScroll += HandleBurnScroll;
            EventManager.AddEffect += HandleAddEffect;
            EventManager.TurnChanged += HandleTurnChanged;
        }

        void OnDisable()
        {
            EventManager.CastScroll -= HandleCastScroll;
            EventManager.BurnScroll -= HandleBurnScroll;
            EventManager.AddEffect -= HandleAddEffect;
            EventManager.TurnChanged -= HandleTurnChanged;
        }

        private void HandleCastScroll(Scroll scroll)
        {
            Clef[] clefs = new Clef[] { };
            Chords[] chords = new Chords[] { };
            Scale[] scales = new Scale[] { };
            Modes[] modes = new Modes[] { };
            int novr = scroll.numberOfValidRandom;
            string[] names = new string[novr];

            if (novr > 0)
            {
                if (scroll.validClef.Length > 0)
                    clefs = GetRandomsTFromScroll(
                        scroll.validClef,
                        novr,
                        ref names,
                        "Theory.Clef."
                    );
                else if (scroll.validChords.Length > 0)
                    chords = GetRandomsTFromScroll(
                        scroll.validChords,
                        novr,
                        ref names,
                        "Theory.Chord."
                    );
                else if (scroll.validScales.Length > 0)
                    scales = GetRandomsTFromScroll(
                        scroll.validScales,
                        novr,
                        ref names,
                        "Theory.Scale."
                    );
                else if (scroll.validModes.Length > 0)
                    modes = GetRandomsTFromScroll(
                        scroll.validModes,
                        novr,
                        ref names,
                        "Theory.Modes."
                    );
            }

            ScrollDTO scrollDTO = new ScrollDTO(scroll, clefs, chords, scales, modes, names);
            onGoingScrolls.Add(scrollDTO);
            EventManager.AddScroll?.Invoke(scrollDTO);
        }

        private void HandleBurnScroll(Scroll scroll)
        {
            BurnedScrollDTO burnedScrollDTO = new BurnedScrollDTO(
                scroll.scrollID,
                PlaythroughContainer.Instance.currentTurn.Item2,
                PlaythroughContainer.Instance.currentTurn.Item2 + scroll.cooldown + 1
            );
            burnedScrolls.Add(burnedScrollDTO);
        }

        private void HandleAddEffect(EffectDTO effect)
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
                onGoingEffects
                    .Where(
                        obj => obj.Target == effect.Target && obj.EffectType == effect.EffectType
                    )
                    .Count() > 0
                && effectTypesShouldAddTurns.Contains(effect.EffectType)
            )
            {
                var effectsThatShouldAddTurns = onGoingEffects.Where(
                    obj => obj.Target == effect.Target && obj.EffectType == effect.EffectType
                );

                effectsThatShouldAddTurns
                    .ToList()
                    .ForEach(obj => obj.EffectExpireOnTurn = effect.EffectExpireOnTurn + 1);
                return;
            }

            onGoingEffects.Add(effect);
        }

        private void HandleTurnChanged(Turn turn)
        {
            onGoingScrolls.ForEach(obj => EventManager.ScrollRemoved?.Invoke(obj));

            if (turn == Turn.PlayersTurn)
            {
                burnedScrolls.ForEach(obj => obj.internalCounter++);
                onGoingEffects.ForEach(
                    obj => obj.internalCounter += obj.EffectTarget == EffectTarget.Player ? 1 : 0
                );
            }
            else
            {
                onGoingEffects.ForEach(
                    obj => obj.internalCounter += obj.EffectTarget == EffectTarget.Enemy ? 1 : 0
                );
            }

            burnedScrolls = burnedScrolls
                .Where(obj => obj.CanBeUsedOnTurn > obj.internalCounter)
                .ToList();

            List<EffectDTO> stillOnGoing = onGoingEffects
                .Where(obj => obj.EffectExpireOnTurn > obj.internalCounter)
                .ToList();

            onGoingEffects.ForEach(obj =>
            {
                if (!stillOnGoing.Contains(obj))
                {
                    EventManager.RemoveEffect?.Invoke(obj);
                }
            });

            onGoingEffects = stillOnGoing;

            onGoingScrolls.Clear();
        }

        public void ClearAllEffectsAndScrolls()
        {
            onGoingEffects.Clear();
            onGoingScrolls.Clear();
            burnedScrolls.Clear();
        }

        public void ClearAllPlayersEffects()
        {
            var filteredEffects = onGoingEffects
                .Where(obj => obj.EffectTarget != EffectTarget.Player)
                .ToList();
            onGoingEffects.ForEach(obj =>
            {
                if (!filteredEffects.Contains(obj))
                {
                    EventManager.RemoveEffect(obj);
                }
            });
            onGoingEffects = filteredEffects;
        }

        private T[] GetRandomsTFromScroll<T>(
            T[] values,
            int numberOfRandom,
            ref string[] names,
            string key
        )
        {
            List<T> listOfValues = values.ToList();
            List<T> valuesSelecteds = new List<T>();

            for (int i = 0; i < numberOfRandom; i++)
            {
                List<T> filtered = listOfValues
                    .Where(obj => !valuesSelecteds.Contains(obj))
                    .ToList();

                int randomIndex = UnityEngine.Random.Range(0, filtered.Count);

                valuesSelecteds.Add(filtered[randomIndex]);
                names[i] = LocalizationManager.Localize(key + filtered[randomIndex].ToString());
            }

            return valuesSelecteds.ToArray();
        }
    }

    public class EffectDTO
    {
        private string _gameObjectName;
        private string _displayText;
        private GameObject _target;
        private Sprite _effectIcon;
        private EffectTarget _effectTarget;
        private bool _triggerOnTurnStart;
        private EffectType _effectType;
        private bool _shouldAppearOnHUD;
        private int _effectExpireOnTurn;
        private float _modifier;
        public int internalCounter;

        public string GameObjectName
        {
            get => _gameObjectName;
        }

        public string DisplayText
        {
            get => _displayText;
        }

        public GameObject Target
        {
            get => _target;
        }

        public Sprite EffectIcon
        {
            get => _effectIcon;
        }

        public EffectTarget EffectTarget
        {
            get => _effectTarget;
        }
        public bool TriggerOnTurnStart
        {
            get => _triggerOnTurnStart;
        }
        public EffectType EffectType
        {
            get => _effectType;
        }
        public bool ShouldAppearOnHUD
        {
            get => _shouldAppearOnHUD;
        }
        public int EffectExpireOnTurn
        {
            get => _effectExpireOnTurn;
            set => _effectExpireOnTurn = value;
        }
        public float Modifier
        {
            get => _modifier;
        }

        public EffectDTO(
            string gameObjectName,
            string displayText,
            GameObject target,
            Sprite effectIcon,
            EffectTarget effectTarget,
            bool triggerOnTurnStart,
            EffectType effectType,
            bool shouldAppearOnHUD,
            int effectStartedOnTurn,
            int effectExpireOnTurn,
            float modifier
        )
        {
            _gameObjectName = gameObjectName;
            _displayText = displayText;
            _target = target;
            _effectTarget = effectTarget;
            _effectIcon = effectIcon;
            _triggerOnTurnStart = triggerOnTurnStart;
            _effectType = effectType;
            internalCounter = effectStartedOnTurn;
            _shouldAppearOnHUD = shouldAppearOnHUD;
            _effectExpireOnTurn = effectExpireOnTurn + 1;
            _modifier = modifier;
        }
    }

    public class ScrollDTO
    {
        private Scroll _scroll;
        private Clef[] _targetClefs;
        private Chords[] _targetChords;
        private Scale[] _targetScales;
        private Modes[] _targetModes;
        private string[] _randomTargetsNames;

        public Scroll Scroll
        {
            get => _scroll;
        }

        public Clef[] TargetClefs
        {
            get => _targetClefs;
        }

        public Chords[] TargetChords
        {
            get => _targetChords;
        }

        public Scale[] TargetScales
        {
            get => _targetScales;
        }

        public Modes[] TargetModes
        {
            get => _targetModes;
        }

        public string[] RandomTargetsNames
        {
            get => _randomTargetsNames;
        }

        public ScrollDTO(
            Scroll scroll,
            Clef[] targetClef,
            Chords[] targetChords,
            Scale[] targetScales,
            Modes[] targetModes,
            string[] targetNames
        )
        {
            _scroll = scroll;
            _targetClefs = targetClef;
            _targetChords = targetChords;
            _targetScales = targetScales;
            _targetModes = targetModes;
            _randomTargetsNames = targetNames;
        }
    }

    public class BurnedScrollDTO
    {
        private ScrollID _scroll;
        private int _canBeUsedOnTurn;
        public int internalCounter;

        public ScrollID Scroll
        {
            get => _scroll;
        }

        public int CanBeUsedOnTurn
        {
            get => _canBeUsedOnTurn;
        }

        public BurnedScrollDTO(ScrollID scroll, int internalCounterStartAt, int canBeUsedOnTurn)
        {
            _scroll = scroll;
            internalCounter = internalCounterStartAt;
            _canBeUsedOnTurn = canBeUsedOnTurn;
        }
    }
}
