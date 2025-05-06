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

    // Update is called once per frame
    void Update()
    {
        
    }

    void DamageBehavior()
    {
        Debug.Log("penis");
    }
}
