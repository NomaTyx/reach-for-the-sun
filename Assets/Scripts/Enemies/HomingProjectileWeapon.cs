using Unity.VisualScripting;
using UnityEngine;

public class HomingProjectileWeapon : EnemyWeapon
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileDamage = 1;
    [SerializeField] private float _projectileSpeed = 15;

    protected override void Attack(GameObject target, GameObject instigator)
    {
        //this goes in here rather than EnemyWeapon because not all weapons will be projectile weapons (?)
        //but more importantly because TryAttack is called every frame, and if the max number of projectiles is reached i want to leave some breathing room
        if (EnemyAIManager.Instance.TryRegisterSpawnedBullet()) 
        {
            HomingProjectile proj = Instantiate(_projectile, transform.position, Quaternion.identity).GetComponent<HomingProjectile>();
            proj.Init(target, new DamageInfo(_projectileDamage, instigator, target), _projectileSpeed);
        }
    }
}
