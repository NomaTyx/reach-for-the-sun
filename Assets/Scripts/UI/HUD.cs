using System;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private CooldownBar _dashCooldownBar;
    [SerializeField] private CooldownBar _parryCooldownBar;
    [SerializeField] private CooldownBar _bounceCooldownBar;
    PlayerCombatActions player;

    [SerializeField] private GameObject AbilityIconTemplate;

    private Dictionary<string, AbilityBase> _abilityIcons;

    void Start()
    {
        player = FindFirstObjectByType<PlayerCombatActions>();
        Instantiate(AbilityIconTemplate, gameObject.transform);
        Instantiate(AbilityIconTemplate, gameObject.transform);

        player.OnDashStarted += DashActivated;
        player.OnDashFinished += DashCooldown;
        player.OnParry += ParryCooldown;
        player.OnBounce += BounceCooldown;
    }

    private void OnDestroy()
    {
        player.OnDashStarted -= DashActivated;
        player.OnDashFinished -= DashCooldown;
        player.OnParry -= ParryCooldown;
        player.OnBounce -= BounceCooldown;
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
}
