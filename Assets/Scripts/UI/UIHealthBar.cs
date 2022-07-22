using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthFillImage;
    private Pawn _pawn;

    public void SetPawn(Pawn pawn)
    {
        _pawn = pawn;
        _pawn.OnTakedDamage += RefreshBar;

        ResetBar();
    }

    void ResetBar()
    {
        _healthFillImage.fillAmount = _pawn.GetHealth01();
    }

    void RefreshBar()
    {
        DOTween.To(() => _healthFillImage.fillAmount, x => _healthFillImage.fillAmount = x, _pawn.GetHealth01(), 0.15f);
    }

    private void OnDestroy()
    {
        if (_pawn)
            _pawn.OnTakedDamage -= RefreshBar;
    }
}
