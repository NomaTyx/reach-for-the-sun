using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private AbilityManager _playerAbilities;
    private float _rotationSpeed = 10f;

    private bool _isSpinning;

    private void Start()
    {
        _playerAbilities = GetComponent<AbilityManager>();
        _playerAbilities.AbilitiesInitiated += OnAbilitiesInitiated;
    }

    private void OnDestroy()
    {
        _playerAbilities.Abilities["dash"].AbilityActivated -= Spin;
        _playerAbilities.Abilities["dash"].AbilityFinished -= StopSpinning;
        _playerAbilities.Abilities["dash"].AbilityCanceled -= StopSpinning;
        _playerAbilities.AbilitiesInitiated -= OnAbilitiesInitiated;
    }

    private void OnAbilitiesInitiated()
    {
        _playerAbilities.Abilities["dash"].AbilityActivated += Spin;
        _playerAbilities.Abilities["dash"].AbilityFinished += StopSpinning;
        _playerAbilities.Abilities["dash"].AbilityCanceled += StopSpinning;
    }

    public void Spin()
    {
        StartCoroutine(StartSpin());
    }

    private IEnumerator StartSpin()
    {
        _isSpinning = true;
        while (_isSpinning)
        {
            Vector3 targetAngles = transform.eulerAngles + 180f * Vector3.forward;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, _rotationSpeed * Time.deltaTime); // lerp to new angles

            Debug.Log("Spun!");
            yield return null;
        }
    }

    private void StopSpinning()
    {
        _isSpinning = false;
    }
}
