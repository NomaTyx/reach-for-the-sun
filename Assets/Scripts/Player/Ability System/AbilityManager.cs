using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public Dictionary<string, AbilityBase> Abilities => _abilities;

    private Dictionary<string, AbilityBase> _abilities = new Dictionary<string, AbilityBase>();
    public event Action AbilitiesInitiated;

    public void InitAbilities()
    {
        //manually adding abilities for now
        _abilities["bounce"] = gameObject.AddComponent<AbilityBounce>();
        _abilities["dash"] = gameObject.AddComponent<AbilityDash>();
        _abilities["parry"] = gameObject.AddComponent<AbilityParry>();
        _abilities["flap"] = gameObject.AddComponent<AbilityFlap>();

        foreach (string key in _abilities.Keys)
        {
            _abilities[key].Init();
        }

        AbilitiesInitiated?.Invoke();
    }
}
