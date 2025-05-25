using Unity.VisualScripting;
using UnityEngine;

public class RocketLauncher : EnemyWeapon
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileDamage = 1;
    [SerializeField] private float _projectileSpeed = 15;

    protected override void Attack(GameObject target, GameObject instigator)
    {
        Rocket proj = Instantiate(_projectile, transform.position, Quaternion.identity).GetComponent<Rocket>();
        proj.Init(target, new DamageInfo(_projectileDamage, instigator, target), _projectileSpeed);
    }
}
