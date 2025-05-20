using UnityEngine;

public class Rocket : MonoBehaviour
{
    private GameObject _target;
    private float _damage;
    private float _speed;
    private Rigidbody _rb;

    public void Init(GameObject target, float damage, float speed)
    {
        _target = target;
        _damage = damage;
        _speed = speed;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Debug.Log("thing was created!");
    }

    private void Update()
    {
        //need to nullcheck because the rocket exists without a target for the first frame it's alive
        if (_target == null) return;

        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.Normalize();
        Vector3 target = Vector3.RotateTowards(transform.forward, targetDirection, Time.fixedDeltaTime * 0.3f, 0);
        transform.rotation = Quaternion.LookRotation(target);
        _rb.linearVelocity = transform.forward * _speed;
    }

    //
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Health hitHealth)) return;

        hitHealth.Damage(new DamageInfo(hitHealth.Current, gameObject, other.gameObject));

        Destroy(gameObject);

        Debug.Log($"hit {other.name}");

        Debug.Log("Thing was destroyed!");
    }
}
