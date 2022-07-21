using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif

public class UIGameplayCanvas : MonoBehaviour
{
    [SerializeField] private UIStateButtonPanel _stateButtonPanel;
    [SerializeField] private UIHealthBar _healthBar;

    private UIPanel _uiPanel;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_uiPanel == null)
            _uiPanel = GetComponent<UIPanel>();


        if (_uiPanel != null)
        {
            if (_uiPanel.OnHide.GetPersistentEventCount() == 0)
                UnityEventTools.AddPersistentListener(_uiPanel.OnHide, OnHide);

            if (_uiPanel.OnShow.GetPersistentEventCount() == 0)
                UnityEventTools.AddPersistentListener(_uiPanel.OnShow, OnShow);

            if (_uiPanel.OnCallHide.GetPersistentEventCount() == 0)
                UnityEventTools.AddPersistentListener(_uiPanel.OnCallHide, OnCallHide);

            if (_uiPanel.OnCallShow.GetPersistentEventCount() == 0)
                UnityEventTools.AddPersistentListener(_uiPanel.OnCallShow, OnCallShow);
        }
    }
#endif

    public void OnCallShow()
    {
        _healthBar.SetPawn(Character.Instance);
        _stateButtonPanel.CreateStateButtons();
    }

    public void OnCallHide()
    {

    }

    public void OnShow()
    {

    }

    public void OnHide()
    {

    }


    public void BackToMenuScene()
    {

    }


}
