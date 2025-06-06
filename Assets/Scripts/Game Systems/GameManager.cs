using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    /*TODO implement states:
     - Dead state
     - gameplay state
     - UI state
    */

    public GameStateBase State;
    public PlayerController Player;
    private void Start()
    {
        //caching player component for easy referencing
        Player = FindFirstObjectByType<PlayerController>();

        if (Instance != null)
        {
            Debug.Log("There's already an instance of this singleton!");
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }
}
