using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyWeapon))]
public class EnemyController : MonoBehaviour
{
    private Health _health;
    private EnemyWeapon _weapon;
    private PlayerController _target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _weapon = GetComponent<EnemyWeapon>();
        _health = GetComponent<Health>();
        _target = FindFirstObjectByType<PlayerController>(); //dunno if the enemy should track the player by its Health component but that's what makes sense to me?

        _health.OnDamage += DamageBehavior;
        _health.OnDeath += DeathBehavior;

        StartCoroutine(AggressiveState());
    }

    private void OnDestroy()
    {
        _health.OnDamage -= DamageBehavior;
    }

    /// <summary>
    /// the behavior that happens when an enemy takes damage
    /// </summary>
    void DamageBehavior(GameObject damagedObject)
    {
        Debug.Log("penis");
    }

    void DeathBehavior(GameObject deadObject)
    {
        //for some fucking reason you need to SPECIFY to look for inactive components. cry.
        GetComponentInChildren<EnemyShatterer>(true).gameObject.SetActive(true);
    }

    //i might end up doing some crap with an enemy AI manager that sends instructions to enemies so that i can make them more coordinated
    //currently they operate independently tho
    private IEnumerator AggressiveState()
    {
        //this is (currently!) the default AI behavior, so we're just looping forever
        while (true)
        {   
            _weapon.TryAttack(_target.gameObject, gameObject); //should it really be trying to attack every frame? TODO: Reexamine.
            
            yield return null;
        }
    }
}
