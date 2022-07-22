using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    static Coroutine _loadRoutine;

    private void Awake()
    {
        Instance = this;
    }

    public static void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
    public static void LoadScene(string sceneName)
    {
        _loadRoutine = Instance.StartCoroutine(IELoadSceneRoutine(sceneName));
    }

    static IEnumerator IELoadSceneRoutine(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!async.isDone)
        {
            yield return null;
        }

        GlobalEvents.OnNewSceneLoaded?.Invoke();
    }
}
