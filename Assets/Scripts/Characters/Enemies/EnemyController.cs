using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Health _health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = GetComponent<Health>();
        _health.OnDamage += DamageBehavior;
    }

    private void OnDestroy()
    {
        _health.OnDamage -= DamageBehavior;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// the behavior that happens when an enemy takes damage
    /// </summary>
    void DamageBehavior()
    {
        Debug.Log("penis");
    }
}
