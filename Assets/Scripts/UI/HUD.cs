using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    protected PlayerController player;
    protected AbilityManager _playerAbilities;

    [SerializeField] private GameObject AbilityIconTemplate;

    private Dictionary<string, GameObject> _abilityIcons = new Dictionary<string, GameObject>();

    void Start()
    {
        player = GameManager.Instance.Player;
        _playerAbilities = player.gameObject.GetComponent<AbilityManager>();

        _playerAbilities.AbilitiesInitiated += OnAbilitiesInitiated;
    }

    private void OnDestroy()
    {
        _playerAbilities.AbilitiesInitiated -= OnAbilitiesInitiated;
    }

    private void OnAbilitiesInitiated()
    {
        foreach (AbilityBase a in _playerAbilities.Abilities.Values)
        {
            GameObject icon = Instantiate(AbilityIconTemplate, gameObject.transform);
            icon.GetComponentInChildren<TextMeshProUGUI>().text = a.AbilityName;
            a.AbilityActivated += icon.GetComponent<CooldownBar>().ShowIfAbilityActive;
            a.AbilityFinished += icon.GetComponent<CooldownBar>().StartCooldown;
            a.AbilityCanceled += icon.GetComponent<CooldownBar>().ShowIfAbilityActive;
            icon.GetComponent<CooldownBar>().Init(a.AbilityCooldownDuration);
            _abilityIcons[a.AbilityName] = icon;
        }
    }
}
