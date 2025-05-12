using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private bool _hitStopped = false;
    private bool _gamePaused = false;
    private void Start()
    {
        if (Instance != null)
        {
            Debug.Log("There's already an instance of this singleton!");
            return;
        }

        Instance = this;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void BulletTime(float modifier)
    {
        Time.timeScale = modifier;
    }

    public void HitStop(float durationMilliseconds)
    {
        if (!_hitStopped)
        {
            StartCoroutine(HitStopCoroutine(durationMilliseconds));
        }
    }

    private IEnumerator HitStopCoroutine(float durationMilliseconds)
    {
        float previousTimeScale = Time.timeScale;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(durationMilliseconds);

        if(!_gamePaused)
        {
            Time.timeScale = previousTimeScale;
        }
    }
}
