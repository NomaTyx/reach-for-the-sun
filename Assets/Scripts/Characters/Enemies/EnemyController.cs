using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyWeapon))]
public class EnemyController : MonoBehaviour
{
    private Health _health;
    private EnemyWeapon _weapon;
    [SerializeField] private float tempVar_AttackCooldown = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _weapon = GetComponent<EnemyWeapon>();
        _health = GetComponent<Health>();

        _health.OnDamage += DamageBehavior;

        StartCoroutine(AggressiveState());
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
    void DamageBehavior(GameObject damagedObject)
    {
        Debug.Log("penis");
    }

    void Attack()
    {
        Debug.Log("Bam! Attacked");
    }

    //i might end up doing some crap with an enemy AI manager that sends instructions to enemies so that i can make them more coordinated
    //currently they operate independently tho
    private IEnumerator AggressiveState()
    {
        float AttackTime = 0;
        //this is (currently!) the default AI behavior, so we're just looping forever
        while (true)
        {
            Debug.Log("coroutine is going");
            AttackTime += Time.deltaTime;
            if(AttackTime > tempVar_AttackCooldown)
            {
                Attack();
                AttackTime = 0;
            }
            yield return null;
        }
    }
}
