using UnityEngine;

public class RocketLauncher : EnemyWeapon
{
    [SerializeField] private Projectile _projectile;

    protected override void Attack(GameObject target, GameObject instigator)
    {
        Projectile proj = Instantiate(_projectile);
    }
}
