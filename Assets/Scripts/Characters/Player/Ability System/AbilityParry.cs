using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AbilityParry : AbilityBase
{
    private float _parryCooldown = 2f;
    private float _parryRange = 5f;

    public override void Init()
    {
        base.Init();
        AbilityName = "Parry";
        //hardcoding cooldowns and durations for now, until i can decide where to store them
        CooldownDuration = _parryCooldown;
        EffectDuration = 0;

        cooldownWFS = new WaitForSecondsRealtime(CooldownDuration);
    }

    // Update is called once per frame
    public override void Effect(bool doCooldown)
    {
        //check only the projectile layer
        Collider[] colliders = Physics.OverlapSphere(transform.position, _parryRange);

        foreach (Collider c in colliders)
        {
            c.TryGetComponent<Rocket>(out Rocket rocket);

            if (rocket != null)
            {
                rocket.ParryProjectile();
            }
        }
        base.Effect(doCooldown);
    }
}
