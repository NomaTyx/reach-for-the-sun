using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityParry : Ability
{
    private float _parryCooldown = 2f;
    private float _parryRange = 5f;

    private Rigidbody _rb;

    private List<HomingProjectile> _projectilesList;

    public override void Init()
    {
        base.Init();
        AbilityName = "Parry";
        //hardcoding cooldowns and durations for now, until i can decide where to store them
        AbilityCooldownDuration = _parryCooldown;
        AbilityEffectDuration = 0;

        _rb = GameManager.Instance.Player.GetComponent<Rigidbody>();
    }

    public override void TryUse()
    {
        //check only the projectile layer
        Collider[] colliders = Physics.OverlapSphere(transform.position, _parryRange);
        _projectilesList = new List<HomingProjectile>();

        foreach (Collider c in colliders)
        {
            c.TryGetComponent(out HomingProjectile rocket);

            if (rocket != null)
            {
                _projectilesList.Add(rocket);
                
            }
        }

        //parry should not be considered activated if there are no projectiles in range
        if (_projectilesList.Count > 0)
        {
            base.TryUse();
        }
    }

    public override void Effect(bool doCooldown)
    {
        base.Effect(doCooldown);
        //propel player per thing parried.
        foreach (HomingProjectile rocket in _projectilesList)
        {
            rocket.ParryProjectile();
        }
    }
}
