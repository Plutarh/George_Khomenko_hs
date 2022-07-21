using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContext : MonoBehaviour
{
    [SerializeField] private PointGenerator _pointGenerator;
    [SerializeField] private CharacterInstaller _characterInstaller;

    private void Awake()
    {
        InitializeCore();
        InitializeModules();
    }

    void InitializeModules()
    {
        _pointGenerator.Initialize();
        _characterInstaller.Initialize();
    }

    // Для того, чтобы можно было играть сразу со сцены Game
    void InitializeCore()
    {
        if (Core.Instance == null)
            Core.Initialize();
    }

}
