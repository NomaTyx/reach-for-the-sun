using Unity.VisualScripting;
using UnityEngine;

public class HomingProjectileWeapon : EnemyWeapon
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileDamage = 1;
    [SerializeField] private float _projectileSpeed = 15;

    protected override void Attack(GameObject target, GameObject instigator)
    {
        HomingProjectile proj = Instantiate(_projectile, transform.position, Quaternion.identity).GetComponent<HomingProjectile>();
        proj.Init(target, new DamageInfo(_projectileDamage, instigator, target), _projectileSpeed);
        EnemyAIManager.Instance.RegisterSpawnedBullet();
    }
}
