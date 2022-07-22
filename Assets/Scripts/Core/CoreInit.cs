using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreInit : MonoBehaviour
{
    private void Awake()
    {
        InitializeCore();
    }

    // Для того, чтобы можно было играть сразу со сцены Game
    void InitializeCore()
    {
        if (Core.Instance == null)
            Core.Initialize();
    }
}
