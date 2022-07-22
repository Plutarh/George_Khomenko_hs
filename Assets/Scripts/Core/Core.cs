using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour
{
    public static EGameState GameState => Instance._gameState;

    [SerializeField] private Config config;


    [SerializeField] private EGameState _gameState;



    public static Core Instance;

    public enum EGameState
    {
        Play,
        Menu
    }


    public enum EGameOverReason
    {
        Win,
        Lose
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.root.gameObject);
        Config.Instance = config;
        Instance = this;
        CheckFirstScene();
    }


    public static Core Initialize()
    {
        if (Instance)
            return Instance;

        var coreResource = Resources.Load("CoreRig");
        if (coreResource == null)
        {
            Debug.LogError("CoreRig not exist in Resources folder");
            return null;
        }

        GameObject coreRig = Instantiate(coreResource) as GameObject;
        Instance = coreRig.GetComponentInChildren<Core>();

        return Instance;
    }

    public void ChangeGameState(EGameState newState)
    {
        if (_gameState == newState) return;

        _gameState = newState;

        Debug.Log("Change state to " + _gameState);
        GlobalEvents.OnGameStateChanged?.Invoke(_gameState);
    }

    void CheckFirstScene()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            _gameState = EGameState.Play;
        }
        else
        {
            _gameState = EGameState.Menu;
        }
    }

    public void LoadGameScene()
    {
        ChangeGameState(EGameState.Play);
        SceneLoader.LoadScene("Game");
    }

    public void LoadMenuScene()
    {
        SceneLoader.LoadScene("Menu");
        ChangeGameState(EGameState.Menu);
    }
}
