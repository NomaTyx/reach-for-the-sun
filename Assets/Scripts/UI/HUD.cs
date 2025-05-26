using UnityEngine;

public class HUD : MonoBehaviour
{
    PlayerCombatActions player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerCombatActions>();

        player.OnDash += DashCooldown;
    }

    private void OnDestroy()
    {
        player.OnDash -= DashCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DashCooldown(float cooldown)
    {
        Debug.Log("Dash cooled down");
    }
}
