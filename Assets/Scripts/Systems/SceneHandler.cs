using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private static SceneHandler _instance;

    public static SceneHandler Instance => _instance;

    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1;

    private string _sceneName;

    private void Start()
    {
        if (_instance != null)
        {
            Debug.Log("There already exists an instance of this singleton!");
            Destroy(_instance);
            return;
        }

        _instance = this;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }
}