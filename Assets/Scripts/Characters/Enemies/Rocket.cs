using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _parriedProjectileLifetime = 1f;
    private GameObject _target;
    private float _damage;
    private float _speed;
    private Rigidbody _rb;

    private IEnumerator _currentState;

    public void Init(GameObject target, float damage, float speed)
    {
        _rb = GetComponent<Rigidbody>();
        _target = target;
        _damage = damage;
        _speed = speed;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        ChangeState(ActiveState());
    }

    private void ChangeState(IEnumerator newState)
    {
        if (_currentState != null) StopCoroutine(_currentState);

        _currentState = newState;
        StartCoroutine(_currentState);
    }

    private IEnumerator ActiveState()
    {
        //need to nullcheck because the rocket exists without a target for the first frame it's alive
        //yield return null waits til the next frame, by which point it's been initialized. perfect.
        if (_target == null) yield return null;

        while(true)
        {
            Vector3 targetDirection = _target.transform.position - transform.position;
            targetDirection.Normalize();
            Vector3 target = Vector3.RotateTowards(transform.forward, targetDirection, Time.fixedDeltaTime * 0.3f, 0);
            transform.rotation = Quaternion.LookRotation(target);
            _rb.linearVelocity = transform.forward * _speed;
            yield return null;
        }
    }

    private void ParryProjectile()
    {
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, _parriedProjectileLifetime);
        ChangeState(ParriedState());
    }

    private IEnumerator ParriedState()
    {
        while(true)
        {
            _rb.linearVelocity = transform.forward * _speed;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Health hitHealth)) return;

        hitHealth.Damage(new DamageInfo(hitHealth.Current, gameObject, other.gameObject));

        Destroy(gameObject);
    }
}
