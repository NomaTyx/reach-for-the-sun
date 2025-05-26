using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private CooldownBar _dashCooldownBar;
    PlayerCombatActions player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerCombatActions>();

        player.OnDash += DashCooldown;
    }

    private void OnDestroy()
    {
        player.OnDash -= DashCooldown;
    }

    private void DashCooldown(float cooldown)
    {
        _dashCooldownBar.StartCooldown(cooldown);
    }
}
