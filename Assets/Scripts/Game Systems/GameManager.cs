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
    public Camera Camera;
    public AbilityData AbilityData;

    private void Awake()
    {
        //caching player component for easy referencing
        Player = FindFirstObjectByType<PlayerController>();
        Camera = FindFirstObjectByType<Camera>();

        if (Instance != null)
        {
            Debug.Log("There's already an instance of this singleton!");
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }
}
