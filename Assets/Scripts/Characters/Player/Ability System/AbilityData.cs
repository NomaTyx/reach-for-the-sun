using System;
using System.Collections.Generic;
using UnityEngine;


/* UNUSED SCRIPT, JUST KEEPING FOR REFERENCE'S SAKE */

[CreateAssetMenu(fileName = "Abilities", menuName = "Available Abilities")]
public class AbilityData : ScriptableObject
{
    public int GenericInteger;
    public DashData DashData;
    public BounceData BounceData;

    public List<BaseAbilityData> Abilities()
    {
        //add all the abilities into this
        return new List<BaseAbilityData>();
    }
}

[Serializable]
public class BaseAbilityData
{

}

[Serializable]
public class DashData : BaseAbilityData
{
    [SerializeField] private float _dashTime = 2.5f;
    [SerializeField] private float _dashCooldown = 2.5f;
    public float DashTime => _dashTime;
    public float DashCool => _dashTime;
}

[Serializable]
public class BounceData : BaseAbilityData
{
    [SerializeField] private float _bounceRange = 10;
    [SerializeField] private float _bounceForce = 100;
    [SerializeField] private float _bounceCooldown = 5f;

    public float BounceRange => _bounceRange;
    public float BounceForce => _bounceForce;
    public float BounceCooldown => _bounceCooldown;
}