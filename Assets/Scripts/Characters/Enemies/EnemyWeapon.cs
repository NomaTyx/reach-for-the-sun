using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    //as of RftS Alpha, there's only one weapon type which is a homing projectile. but i'm using this
    //as an excuse to practice making scalable systems, so i'm making a more dynamic weapon system instead
    //of hardcoding it.
    
    [SerializeField] private float tempVar_AttackCooldown = 1f;
    [SerializeField] private Projectile _projectile; //this might also be useless code if i decide to do the weapon system.
    [SerializeField] private float _projectileSpeed = 10f;

    private float _lastAttackTime;

    private void Start()
    {
        _lastAttackTime = Time.time;
    }

    public void TryAttack(GameObject target, GameObject instigator)
    {
        //TODO: add cooldown logic and stuff like that
        if (_lastAttackTime > Time.time + tempVar_AttackCooldown)
        {
            Attack(target, instigator);
            _lastAttackTime = Time.time;
        }
        
    }

    protected virtual void Attack(GameObject target, GameObject instigator)
    {
        Debug.Log("this enemy doesn't have a weapon equipped!");
    }
}
