using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    protected PlayerController _player;
    protected AbilityManager _playerAbilities;

    [SerializeField] private GameObject AbilityIconTemplate;

    private Dictionary<string, CooldownBar> _abilityIcons = new Dictionary<string, CooldownBar>();

    void Start()
    {
        _player = GameManager.Instance.Player;
        _playerAbilities = _player.gameObject.GetComponent<AbilityManager>();

        _playerAbilities.AbilitiesInitiated += OnAbilitiesInitiated;
    }

    private void OnDestroy()
    {
        _playerAbilities.AbilitiesInitiated -= OnAbilitiesInitiated;
    }

    private void OnAbilitiesInitiated()
    {
        foreach (Ability a in _playerAbilities.Abilities.Values)
        {
            CooldownBar icon = Instantiate(AbilityIconTemplate, gameObject.transform).GetComponent<CooldownBar>();

            a.AbilityActivated += icon.ShowIfAbilityActive;
            a.AbilityFinished += icon.StartCooldown;
            a.AbilityCanceled += icon.ShowIfAbilityActive;

            icon.Init(a);

            _abilityIcons[a.AbilityName] = icon;
        }
    }
}
