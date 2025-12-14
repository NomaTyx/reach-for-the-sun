using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private AbilityManager _playerAbilities;
    private float _spinSpeed = 10f;
    private float _twirlSpeed = 10f;

    private bool _isSpinning;

    private void Start()
    {
        _playerAbilities = GetComponent<AbilityManager>();
        _playerAbilities.AbilitiesInitiated += OnAbilitiesInitiated;
    }

    private void OnAbilitiesInitiated()
    {
        _playerAbilities.Abilities["dash"].AbilityActivated += StartSpinning;
        _playerAbilities.Abilities["dash"].AbilityFinished += StopSpinning;
        _playerAbilities.Abilities["dash"].AbilityCanceled += StopSpinning;

        _playerAbilities.Abilities["parry"].AbilityActivated += StartTwirling;
    }

    public void StartSpinning()
    {
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        _isSpinning = true;
        while (_isSpinning)
        {
            Vector3 targetAngles = transform.eulerAngles + 180f * Vector3.forward;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, _spinSpeed * Time.deltaTime); // lerp to new angles
            yield return null;
        }
    }

    private void StopSpinning()
    {
        _isSpinning = false;
    }

    public void StartTwirling()
    {
        StartCoroutine(Twirl());
    }

    private IEnumerator Twirl()
    {
        Vector3 targetAngles = transform.eulerAngles + 180f * Vector3.up;
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, _twirlSpeed * Time.deltaTime); // lerp to new angles
        yield return null;
    }
}
