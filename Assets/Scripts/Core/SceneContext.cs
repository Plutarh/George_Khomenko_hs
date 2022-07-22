using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContext : MonoBehaviour
{
    [SerializeField] private PointGenerator _pointGenerator;
    [SerializeField] private CharacterInstaller _characterInstaller;

    private void Awake()
    {
        InitializeModules();
    }

    void InitializeModules()
    {
        _pointGenerator.Initialize();
        _characterInstaller.Initialize();
    }



}
