using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] List<UIPanel> _allUIPanels = new List<UIPanel>();
    [SerializeField] UIPanel _gameplayPanel;
    [SerializeField] UIPanel _loadingPanel;
    [SerializeField] UIPanel _menuPanel;


    public static UIController Instance;

    private void Awake()
    {
        Instance = this;
        CheckFirstScene();
        GlobalEvents.OnNewSceneLoaded += OnNewSceneLoaded;
    }

    // Специально, чтобы можно было играть с любой сцены и октрывался нужный UI
    void CheckFirstScene()
    {
        DisableAllPanels();
        if (SceneManager.GetActiveScene().name == "Game")
        {
            _loadingPanel.Hide();
            _gameplayPanel.Show();
        }
        else
        {
            _menuPanel.QuickShow();
        }
    }

    void OnNewSceneLoaded()
    {
        DisableAllPanels();

        StartCoroutine(CallDelayAction(0.3f, () => _loadingPanel.Hide()));
        switch (Core.GameState)
        {
            case Core.EGameState.Play:
                StartCoroutine(CallDelayAction(0.3f, () => _gameplayPanel.Show()));
                break;
            case Core.EGameState.Menu:
                _menuPanel.QuickShow();
                break;
        }

    }

    public void ShowLoadingPanel()
    {
        _loadingPanel.Show();
    }

    public void HideLoadingPanel()
    {
        _loadingPanel.Hide();
    }

    IEnumerator CallDelayAction(float delay, System.Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
    }

    void DisableAllPanels()
    {
        foreach (var panel in _allUIPanels)
        {
            if (panel == null) continue;
            panel.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GlobalEvents.OnNewSceneLoaded -= OnNewSceneLoaded;
    }
}
