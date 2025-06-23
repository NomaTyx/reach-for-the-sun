using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    private List<EnemyController> _enemiesInScene = new List<EnemyController>();
    private int _numOfActiveBullets = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (EnemyController enemy in FindObjectsByType<EnemyController>(FindObjectsSortMode.None)) {
            _enemiesInScene.Add(enemy);
        }

        foreach (EnemyController go in _enemiesInScene) Debug.Log(go.gameObject.name);
    }

    public void RegisterSpawnedBullet()
    {
        _numOfActiveBullets++;
    }

    public void RegisterDestroyedBullet()
    {
        _numOfActiveBullets--;
    }
}
