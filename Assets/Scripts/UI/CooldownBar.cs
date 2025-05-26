using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    //i don't want to have to serialize this, but getcomponentinchildren gets the component from the parent as well, so this is just the best i can do.
    [SerializeField] private Image _fillBar;

    private float _cooldownStartTime;
    private float _cooldownDuration;

    public void StartCooldown(float cooldown)
    {
        Debug.Log("started cooldown");
        _cooldownStartTime = Time.time;
        _cooldownDuration = cooldown;
        _fillBar.fillAmount = 1;
        StartCoroutine(CooldownFill());
    }

    private IEnumerator CooldownFill()
    {
        while (_fillBar.fillAmount > 0)
        {
            _fillBar.fillAmount = 1 - (Time.time - _cooldownStartTime) / _cooldownDuration;
            Debug.Log("filling");
            yield return null;
        }
        yield return null;
    }
}
