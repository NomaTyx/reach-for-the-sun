using System.Collections.Generic;
using UnityEngine;

public class EnemyAIManager : MonoBehaviour
{
    public static EnemyAIManager Instance;
    private List<EnemyController> _enemiesInScene = new List<EnemyController>();
    private int _numOfActiveBullets = 0;
    private int _maxActiveBullets = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null)
        {
            Debug.Log("There's already an instance of this singleton!");
            return;
        }

        Instance = this;

        foreach (EnemyController enemy in FindObjectsByType<EnemyController>(FindObjectsSortMode.None)) {
            _enemiesInScene.Add(enemy);
        }
    }

    public bool TryRegisterSpawnedBullet()
    {
        if (_numOfActiveBullets < _maxActiveBullets)
        {
            _numOfActiveBullets++;
            return true;
        }
        return false;
    }

    public void RegisterDestroyedBullet()
    {
        _numOfActiveBullets--;
    }

    public void RegisterSpawnedEnemy(GameObject enemy) //passing in gameobject in case i need to do some sort of logic
    {
        _enemiesInScene.Add(enemy.GetComponent<EnemyController>());
    }
}
