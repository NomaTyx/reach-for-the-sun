using System;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private CooldownBar _dashCooldownBar;
    [SerializeField] private CooldownBar _parryCooldownBar;
    PlayerCombatActions player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerCombatActions>();

        player.OnDash += DashCooldown;
        player.OnParry += ParryCooldown;
    }

    private void OnDestroy()
    {
        player.OnDash -= DashCooldown;
    }

    private void DashCooldown(float cooldown)
    {
        _dashCooldownBar.StartCooldown(cooldown);
    }

    private void ParryCooldown(float cooldown)
    {
        _parryCooldownBar.StartCooldown(cooldown);
    }
}
