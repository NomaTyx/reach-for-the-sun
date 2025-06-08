using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilities", menuName = "Available Abilities")]
public class AbilityData : ScriptableObject
{
    public int GenericInteger;
    public SpecificAbilityData SpecificAbilityData;
    public SpecificAbilityData2 SpecificAbilityData2;
    public DashData DashData;

    public List<BaseAbilityData> BaseAbilities()
    {
        //add all the abilities into this
        return new List<BaseAbilityData>();
    }
}

[Serializable]
public class BaseAbilityData
{
    public int GenericInteger2;
    public string GenericString;
}

[Serializable]
public class SpecificAbilityData : BaseAbilityData
{
    [SerializeField] private float temp1;
}

[Serializable]
public class SpecificAbilityData2 : BaseAbilityData
{
    [SerializeField] private string temp2;
}

[Serializable]
public class DashData : BaseAbilityData
{
    [SerializeField] private float _dashTime;
    public float DashTime => _dashTime;
}