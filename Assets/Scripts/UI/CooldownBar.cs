using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    //i don't want to have to serialize this, but getcomponentinchildren gets the component from the parent as well, so this is just the best i can do.
    [SerializeField] private Image _fillBar;
    [SerializeField] private Image _activityIcon; //i really don't have a good name for this sorry whoever is reading this lol (unless you're julia, fuck you)

    private float _cooldownStartTime;
    private float _cooldownDuration;

    public void Init(AbilityBase ability)
    {
        _cooldownDuration = ability.AbilityCooldownDuration;
        GetComponentInChildren<TextMeshProUGUI>().text = ability.AbilityName;
    }

    public void ShowIfAbilityActive()
    {
        _activityIcon.enabled = !_activityIcon.enabled;
    }

    public void StartCooldown()
    {
        _cooldownStartTime = Time.time;
        _fillBar.fillAmount = 1;
        ShowIfAbilityActive();
        StartCoroutine(CooldownFill());
    }

    private IEnumerator CooldownFill()
    {
        while (_fillBar.fillAmount > 0)
        {
            _fillBar.fillAmount = 1 - (Time.time - _cooldownStartTime) / _cooldownDuration;
            yield return null;
        }
        yield return null;
    }
}
