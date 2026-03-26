using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Health _health;
    private EnemyWeapon _weapon;
    private PlayerController _target;

    void OnEnable()
    {
        
    }

    private void Start()
    {
        _weapon = GetComponentInChildren<EnemyWeapon>();
        _health = GetComponent<Health>();

        _health.OnDamage += DamageBehavior;
        _health.OnDeath += DeathBehavior;

        _target = GameManager.Instance.Player;
        StartCoroutine(AggressiveState());
    }

    private void OnDestroy()
    {
        _health.OnDamage -= DamageBehavior;
        _health.OnDeath -= DeathBehavior;
    }

    /// <summary>
    /// the behavior that happens when an enemy takes damage
    /// </summary>
    void DamageBehavior(GameObject damagedObject)
    {
        Debug.Log($"{gameObject.name} took damage");
    }

    void DeathBehavior(GameObject deadObject)
    {
        //for some fucking reason you need to SPECIFY to look for inactive components. cry.
        GetComponentInChildren<Shatterer>(true).gameObject.SetActive(true);
    }

    //i might end up doing some crap with an enemy AI manager that sends instructions to enemies so that i can make them more coordinated
    //currently they operate independently tho
    private IEnumerator AggressiveState()
    {
        //this is (currently!) the default AI behavior, so we're just looping forever
        while (true)
        {
            if (_target != null)
                _weapon.TryAttack(_target.gameObject, gameObject); //should it really be trying to attack every frame? TODO: Reexamine.
            
            yield return null;
        }
    }

    private void Update()
    {
        if (_target != null)
            transform.LookAt(_target.transform);
    }
}
