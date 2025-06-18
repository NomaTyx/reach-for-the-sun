using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private GameObject _target;
    private DamageInfo _damageInfo;
    private float _speed;
    private Rigidbody _rb;

    private IEnumerator _currentState;
    private bool _isParried = false;

    public void Init(GameObject target, DamageInfo damageInfo, float speed)
    {
        _rb = GetComponent<Rigidbody>();
        _target = target;
        _damageInfo = damageInfo;
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

    private void Move()
    {
        Vector3 targetDirection = _target.transform.position - transform.position;
        targetDirection.Normalize();
        Vector3 target = Vector3.RotateTowards(transform.forward, targetDirection, Time.fixedDeltaTime * 0.3f, 0);
        transform.rotation = Quaternion.LookRotation(target);
        _rb.linearVelocity = transform.forward * _speed;
    }

    private IEnumerator ActiveState()
    {
        //need to nullcheck because the rocket exists without a target for the first frame it's alive
        //yield return null waits til the next frame, by which point it's been initialized. perfect.
        if (_target == null) yield return null;

        while (true)
        {
            Move();
            yield return null;
        }
    }

    public void ParryProjectile()
    {
        //Destroy(gameObject, _parriedProjectileLifetime);
        _isParried = true;
        ChangeState(ParriedState());
    }

    private IEnumerator ParriedState()
    {
        _target = _damageInfo.Instigator.gameObject;
        transform.LookAt(_target.transform.position);
        _damageInfo = new DamageInfo(_damageInfo.Amount, _damageInfo.Instigator, _damageInfo.Instigator);

        while (true)
        {
            Move();
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Health hitHealth)) return;

        if (hitHealth.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (!_isParried)
            {
                hitHealth.Damage(_damageInfo);
                Destroy(gameObject);
            }
            
        }
        else if (hitHealth.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            if(_isParried) 
            {
                hitHealth.Damage(_damageInfo);
                Destroy(gameObject);
            }
        }
        
    }
}
