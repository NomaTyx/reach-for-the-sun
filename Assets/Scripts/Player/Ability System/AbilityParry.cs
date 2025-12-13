using UnityEngine;

public class AbilityParry : Ability
{
    private float _parryCooldown = 2f;
    private float _parryRange = 5f;

    private Rigidbody _rb;

    public override void Init()
    {
        base.Init();
        AbilityName = "Parry";
        //hardcoding cooldowns and durations for now, until i can decide where to store them
        AbilityCooldownDuration = _parryCooldown;
        AbilityEffectDuration = 0;

        _rb = GameManager.Instance.Player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public override void Effect(bool doCooldown)
    {
        //check only the projectile layer
        Collider[] colliders = Physics.OverlapSphere(transform.position, _parryRange);

        int numberOfRockets = 0;

        foreach (Collider c in colliders)
        {
            c.TryGetComponent<HomingProjectile>(out HomingProjectile rocket);

            if (rocket != null)
            {
                numberOfRockets++;
                rocket.ParryProjectile();
            }
        }

        if (numberOfRockets != 0)
        {
            base.Effect(doCooldown);
            //propel player per thing parried.
            return;
        }
        CancelAbility();
    }
}
