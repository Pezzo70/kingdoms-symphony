using System;
using System.Linq;
using Kingdom.Effects;
using Kingdom.Enums.Player;
using UnityEngine;

namespace Kingdom.Player
{
    public class PlayerStats
    {
        private float _maxMoral;
        private float _currentMoral;
        private int _maxMana;
        private int _currentMana;
        private int _manaPerTurn;
        private int _maxSheetBars;
        private int _availableSheetBars;

        public float MaxMoral
        {
            get => _maxMoral;
        }

        public float CurrentMoral
        {
            get => _currentMoral;
        }

        public int MaxMana
        {
            get => _maxMana;
        }

        public int CurrentMana
        {
            get => _currentMana;
        }

        public int ManaPerTurn
        {
            get => _manaPerTurn;
        }

        public int MaxSheetBars
        {
            get => _maxSheetBars;
        }

        public int AvailableSheetBars
        {
            get => _availableSheetBars;
        }

        public PlayerStats() { }

        public void CreateNewPlayerStats(CharacterID characterID)
        {
            PlayerData playerData = PlayerContainer.Instance.PlayerData;
            _maxMoral = playerData.GetMoral(characterID);
            _currentMoral = _maxMoral;
            _maxMana = playerData.GetMana(characterID);
            _currentMana = _maxMana;
            _manaPerTurn = _maxMana;
            _maxSheetBars = playerData.GetSheetBars(characterID);
            _availableSheetBars = _maxSheetBars;
        }

        public void CleanseAllEffects(CharacterID characterID)
        {
            PlayerData playerData = PlayerContainer.Instance.PlayerData;
            _maxMana = playerData.GetMana(characterID);
            _manaPerTurn = _maxMana;
            _maxSheetBars = playerData.GetSheetBars(characterID);
            _availableSheetBars = _maxSheetBars;
        }

        public void ReduceMoral(float value)
        {
            if (
                EffectsAndScrollsManager
                    .Instance
                    .onGoingEffects
                    .Find(
                        obj =>
                            obj.EffectType == EffectType.CompleteMitigation
                            && obj.EffectTarget == EffectTarget.Player
                    ) != null
            )
            {
                value = 0f;
            }
            else
            {
                EffectsAndScrollsManager
                    .Instance
                    .onGoingEffects
                    .Where(
                        obj =>
                            obj.EffectType == EffectType.PlayerMitigation
                            && obj.EffectTarget == EffectTarget.Player
                    )
                    .ToList()
                    .ForEach(effect =>
                    {
                        ScrollsAndEffectsHandler.ValidateAndExecuteEffectAction(effect, ref value);
                    });
            }

            value = Mathf.CeilToInt(value);

            if (_currentMoral - value <= 0f)
            {
                _currentMoral -= value;
                _currentMoral = Mathf.Clamp(_currentMoral, 0f, _maxMoral);
                EventManager.OnPlayerMoralChange?.Invoke();
                EventManager.OnPlayersDeath?.Invoke();
            }
            else
            {
                _currentMoral -= value;
                EventManager.OnPlayerMoralChange?.Invoke();
            }
        }

        public void GainMoral(float value)
        {
            if (
                EffectsAndScrollsManager
                    .Instance
                    .onGoingEffects
                    .Find(
                        obj =>
                            obj.EffectType == EffectType.PreventPlayerHeal
                            && obj.EffectTarget == EffectTarget.Player
                    ) != null
            )
            {
                return;
            }

            _currentMoral += value;
            _currentMoral = Mathf.Clamp(_currentMoral, 0f, _maxMoral);
            EventManager.OnPlayerMoralChange?.Invoke();
        }

        public void SpendMana(int quantity)
        {
            if (quantity > _currentMana)
                return;

            _currentMana -= quantity;

            if (_manaPerTurn > _maxMana)
                _currentMana = Math.Clamp(_currentMana, 0, _manaPerTurn);
            else
                _currentMana = Math.Clamp(_currentMana, 0, _maxMana);

            EventManager.OnPlayerManaChange?.Invoke();
        }

        public void GainMana(int quantity, bool add)
        {
            if (add)
                _currentMana += quantity;
            else
                _currentMana = quantity;

            if (_manaPerTurn > _maxMana)
                _currentMana = Math.Clamp(_currentMana, 0, _manaPerTurn);
            else
                _currentMana = Math.Clamp(_currentMana, 0, _maxMana);

            EventManager.OnPlayerManaChange?.Invoke();
        }

        public void ChangeManaPerTurn(int value) => _manaPerTurn = value;

        public void ChangeAvailableSheetBars(int quantity) =>
            _availableSheetBars = Math.Clamp(quantity, 1, MaxSheetBars);
    }
}
