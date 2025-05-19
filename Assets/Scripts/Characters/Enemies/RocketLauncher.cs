using Unity.VisualScripting;
using UnityEngine;

public class RocketLauncher : EnemyWeapon
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileDamage;
    [SerializeField] private float _projectileSpeed;

    protected override void Attack(GameObject target, GameObject instigator)
    {
        Rocket proj = Instantiate(_projectile, transform.position, Quaternion.identity).GetComponent<Rocket>();
        proj.Init(target, _projectileDamage, _projectileSpeed);
    }
}
