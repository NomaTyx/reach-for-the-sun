using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private CooldownBar _dashCooldownBar;
    [SerializeField] private CooldownBar _parryCooldownBar;
    [SerializeField] private CooldownBar _bounceCooldownBar;

    protected PlayerCombatActions player;
    protected AbilityManager _playerAbilities;

    [SerializeField] private GameObject AbilityIconTemplate;

    private Dictionary<string, AbilityBase> _abilityIcons;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerCombatActions>();
        _playerAbilities = player.gameObject.GetComponent<AbilityManager>();

        player.OnDashStarted += DashActivated;
        player.OnDashFinished += DashCooldown;
        player.OnParry += ParryCooldown;
        player.OnBounce += BounceCooldown;

        _playerAbilities.AbilitiesInitiated += OnAbilitiesInitiated;
    }

    private void OnDestroy()
    {
        player.OnDashStarted -= DashActivated;
        player.OnDashFinished -= DashCooldown;
        player.OnParry -= ParryCooldown;
        player.OnBounce -= BounceCooldown;

        _playerAbilities.AbilitiesInitiated -= OnAbilitiesInitiated;
    }

    private void DashActivated()
    {
        _dashCooldownBar.ShowIfAbilityActive();
    }

    private void DashCooldown(float cooldown)
    {
        _dashCooldownBar.ShowIfAbilityActive();
        _dashCooldownBar.StartCooldown(cooldown);
    }

    private void ParryCooldown(float cooldown)
    {
        _parryCooldownBar.StartCooldown(cooldown);
    }

    private void BounceCooldown(float cooldown)
    {
        _bounceCooldownBar.StartCooldown(cooldown);
    }

    private void OnAbilitiesInitiated()
    {
        foreach (AbilityBase a in _playerAbilities.Abilities.Values)
        {
            GameObject icon = Instantiate(AbilityIconTemplate, gameObject.transform);
            icon.GetComponentInChildren<TextMeshProUGUI>().text = a.AbilityName;
        }
    }
}
