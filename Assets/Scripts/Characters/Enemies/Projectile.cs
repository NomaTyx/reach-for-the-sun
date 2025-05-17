using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject _target;
    private float _damage;
    private float _speed;
    private Rigidbody _rb;

    public Projectile(GameObject target, float damage, float speed)
    {
        _target = target;
        _damage = damage;
        _speed = speed;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //var targetRotation = Quaternion.LookRotation()
        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();
        Vector3 target = Vector3.RotateTowards(transform.forward, targetDirection, Time.fixedDeltaTime * 0.3f, 0);
        transform.rotation = Quaternion.LookRotation(target);
        _rb.linearVelocity = transform.forward * _speed;
    }

    //
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Health hitHealth)) return;

        hitHealth.Damage(new DamageInfo(_damage, gameObject, other.gameObject));
    }
}
