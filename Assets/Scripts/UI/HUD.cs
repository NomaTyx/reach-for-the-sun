using System;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private CooldownBar _dashCooldownBar;
    [SerializeField] private CooldownBar _parryCooldownBar;
    [SerializeField] private CooldownBar _bounceCooldownBar;
    PlayerCombatActions player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerCombatActions>();

        player.OnDashStarted += DashActivated;
        player.OnDashFinished += DashCooldown;
        player.OnParry += ParryCooldown;
    }

    private void OnDestroy()
    {
        player.OnDashStarted -= DashActivated;
        player.OnDashFinished -= DashCooldown;
        player.OnParry -= ParryCooldown;
    }

    private void DashActivated()
    {
        _dashCooldownBar.ShowIfAbilityActive();
    }

    private void DashCooldown(float cooldown)
    {
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
