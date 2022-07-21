using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField] private Config config;

    private EPlayState _playState;
    private EGameState _gameState;

    public static Core Instance;

    public enum EGameState
    {
        Play,
        Menu
    }

    public enum EPlayState
    {
        Menu,
        Play,
        GameOver
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

    public void ChangePlayState(EPlayState newState)
    {
        if (_playState == newState) return;
        _playState = newState;
        GlobalEvents.OnPlayStateChanged?.Invoke(_playState);
    }

    public void ChangeGameState(EGameState newState)
    {
        if (_gameState == newState) return;
        _gameState = newState;
        GlobalEvents.OnGameStateChanged?.Invoke(_gameState);
    }
}
