using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 1;
    [SerializeField] private float _currentHealth = 1;
    [SerializeField] private float _damageTestPercent = 10;

    public float Max => _maxHealth;
    public float Current => _currentHealth;
    public float Percentage => _currentHealth / _maxHealth;
    public float Missing => _maxHealth - _currentHealth;
    public bool IsAlive => _currentHealth > 0;

    public Event OnDamage;
    public Event OnDeath;

    /// <summary>
    /// damage method
    /// </summary>
    /// <param name="info"></param>
    public void Damage(DamageInfo info)
    {
        _currentHealth -= info.Amount;

    }

    /// <summary>
    /// temporary heal method
    /// </summary>
    public void Heal()
    {

    }

    [ContextMenu("Damage test 10%")]
    public void DamageTest()
    {
        DamageInfo info = new DamageInfo(_maxHealth * (_damageTestPercent / 100), gameObject, gameObject);
        Damage(info);
    }
}
