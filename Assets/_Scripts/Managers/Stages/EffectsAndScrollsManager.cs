using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kingdom.Audio;
using Kingdom.Enums.MusicTheory;
using Kingdom.Enums.Scrolls;
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

        public void ClearAllEffectsAndScrolls()
        {
            onGoingEffects.Clear();
            onGoingScrolls.Clear();
            burnedScrolls.Clear();
        }
    }

    public class EffectDTO
    {
        private string _gameObjectName;
        private EffectTarget _effectTarget;
        private bool _triggerOnTurnStart;
        private EffectType _effectType;
        private bool _shouldAppearOnHUD;
        private int _effectStartedOnTurn;
        private int _effectExpireOnTurn;
        private float _modifier;

        public string GameObjectName
        {
            get => _gameObjectName;
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
        public int EffectStartedOnTurn
        {
            get => _effectStartedOnTurn;
        }
        public int EffectExpireOnTurn
        {
            get => _effectExpireOnTurn;
        }
        public float Modifier
        {
            get => _modifier;
        }

        public EffectDTO(
            string gameObjectName,
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
            _effectTarget = effectTarget;
            _triggerOnTurnStart = triggerOnTurnStart;
            _effectType = effectType;
            _shouldAppearOnHUD = shouldAppearOnHUD;
            _effectStartedOnTurn = effectStartedOnTurn;
            _effectExpireOnTurn = effectExpireOnTurn;
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

        public ReadOnlyCollection<Clef> TargetClefs
        {
            get => Array.AsReadOnly(_targetClefs);
        }

        public ReadOnlyCollection<Chords> TargetChords
        {
            get => Array.AsReadOnly(_targetChords);
        }

        public ReadOnlyCollection<Scale> TargetScales
        {
            get => Array.AsReadOnly(_targetScales);
        }

        public ReadOnlyCollection<Modes> TargetModes
        {
            get => Array.AsReadOnly(_targetModes);
        }

        public ReadOnlyCollection<string> RandomTargetsNames
        {
            get => Array.AsReadOnly(_randomTargetsNames);
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

        public ScrollID Scroll
        {
            get => _scroll;
        }

        public int CanBeUsedOnTurn
        {
            get => _canBeUsedOnTurn;
        }

        public BurnedScrollDTO(ScrollID scroll, int canBeUsedOnTurn)
        {
            _scroll = scroll;
            _canBeUsedOnTurn = canBeUsedOnTurn;
        }
    }
}
