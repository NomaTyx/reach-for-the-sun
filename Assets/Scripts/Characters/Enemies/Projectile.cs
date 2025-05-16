using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject _target;
    private float _damage;
    private float _speed;

    public Projectile(GameObject target, float damage, float speed)
    {
        _target = target;
        _damage = damage;
        _speed = speed;
    }

    private void Update()
    {
        //var targetRotation = Quaternion.LookRotation()
    }

    //
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Health hitHealth)) return;

        hitHealth.Damage(new DamageInfo(_damage, gameObject, other.gameObject));
    }
}
