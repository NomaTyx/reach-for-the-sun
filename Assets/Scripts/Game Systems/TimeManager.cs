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
        //perhaps make this lerp?
        Debug.Log("bullettimed");
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

        Debug.Log("hitstopped");
        yield return new WaitForSecondsRealtime(durationMilliseconds / 1000);

        if(!_gamePaused)
        {
            Time.timeScale = previousTimeScale;
        }
    }
}
