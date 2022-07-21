using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    [Header("Main Panel to tween animations")]
    public RectTransform tween;

    [Header("Animation Type")]
    public AnimationType animationType = AnimationType.noAnimation;
    public Animator animator;
    public RuntimeAnimatorController animatorController;

    bool unscaledTime = true;

    [Header("Time during which the panel cannot be closed")]
    public float panelShowHoldTime;
    float _panelShowHoldTime;

    [Header("Tween Animation Settings")]
    public UpdateType updateType;

    // Время для шага твин анмаций
    public float timeToTweenAction = 0.15f;

    // Дефолтный размер панели при появление (эффект пружины)
    Vector3 defaultShowSizeVector = new Vector3(1.2f, 1.2f, 1);

    // Дефолтный размер панели при скрытие (эффект пружины)
    Vector3 defaultHideSizeVector = new Vector3(1.2f, 1.2f, 1);

    // Если нужна кастомная последовательность векторов,которые будут использоваться при появление панели
    public List<Vector3> customShowStepsVector = new List<Vector3>();
    public static List<UIPanel> uiPanelStack = new List<UIPanel>();

    public static List<UIPanel> all = new List<UIPanel>();

    [Header("Вызов после показа панели")]
    /// <summary>
    /// Ивент при показе
    /// </summary>
    public UnityEvent OnShow;

    [Header("Вызов после скрытия панели")]
    /// <summary>
    /// Ивент при скрытие
    /// </summary>
    public UnityEvent OnHide;


    [Header("Вызов когда просят показать панель")]
    /// <summary>
    /// Ивент при вызове показа
    /// </summary>
    public UnityEvent OnCallShow;

    [Header("Вызов когда просят закрыть панель")]
    /// <summary>
    /// Ивент при вызвое скрытия
    /// </summary>
    public UnityEvent OnCallHide;

    /// <summary>
    /// Ивент при скрытие,для открытия следующей панели
    /// </summary>
    public UnityEvent OnHideWithShowNext;
    /// <summary>
    /// Эта панель может открыть след панель,не вызывая свой базовый OnHide ивент
    /// </summary>
    public bool canOpenNextPanel;

    public CanvasGroup canvasGroup;

    /// <summary>
    /// Делей до закрытия панели
    /// </summary>
    public bool delayHold;

    public enum AnimationType
    {
        TweenSize,
        TweenSlide,
        Animator,
        Fade,
        noAnimation
    }

    private void OnValidate()
    {
        if (transform.childCount == 0) return;
        if (tween == null && transform.GetChild(0).name == "Tween") tween = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public virtual void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();

        unscaledTime = true;

        all.Add(this);

        //transform.localPosition = Vector3.zero;
    }
    public virtual void Start()
    {

    }

    public virtual void OnEnable()
    {
        if (!canvasGroup)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (!canvasGroup)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
    }

    public virtual void OnDisable()
    {

    }

    public virtual void Update()
    {
        HoldPanelTimer();
    }

    public void OpenNextPanelWithoutHideEvent()
    {
        canOpenNextPanel = true;
    }

    void HoldPanelTimer()
    {
        if (_panelShowHoldTime > 0) _panelShowHoldTime -= Time.deltaTime;
        else
        {
            if (delayHold)
            {
                Hide();
                _panelShowHoldTime = panelShowHoldTime;
            }
        }

    }

    #region SHOW_REGION

    public void SetActive(bool isActive)
    {
        if (isActive) Show();
        else Hide();
    }

    public void ShowAdditive()
    {

    }

    IEnumerator IEShowPanel(UIPanel panel, bool hideOthers = true)
    {
        panel.ShowPanel();

        // проигнорируем тему с закрытием всех панелей, потому что панели могут быть вложены в другие панели
        yield break;

        float waitTime = 0f;

        //Вначале скрываем уже открытые панели
        if (hideOthers)
        {
            foreach (var p in UIPanel.all)
            {
                if (p != panel && p.gameObject.activeSelf)
                {
                    //p.Hide();
                    p.gameObject.SetActive(false);
                    waitTime = p.timeToTweenAction;
                }
            }
        }
        yield return new WaitForSeconds(0f);
        panel.ShowPanel();
    }


    public void Show()
    {
        UIController.Instance.StartCoroutine(IEShowPanel(this));
        canvasGroup.interactable = true;
    }

    public void QuickShow()
    {
        if (tweenAction != null)
        {
            tweenAction.Kill();
        }

        tween.DOKill(true);

        if (!canvasGroup)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (!canvasGroup)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;

        gameObject.SetActive(true);
        OnCallShow?.Invoke();
        OnShow?.Invoke();
    }

    /*
    public virtual void OnCallShow()
    {
        OnCallShow?.Invoke();
    }*/

    void ShowPanel()
    {
        OnCallShow?.Invoke();

        if (!canvasGroup)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (!canvasGroup)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (panelShowHoldTime > 0) _panelShowHoldTime = panelShowHoldTime;

        switch (animationType)
        {
            case AnimationType.TweenSize:
                TweenSizeShow();
                break;
            case AnimationType.TweenSlide:
                TweenSlideShow();
                break;
            case AnimationType.Animator:
                AnimatorShow();
                break;
            case AnimationType.noAnimation:
                gameObject.SetActive(true);
                OnShow?.Invoke();
                break;
            case AnimationType.Fade:
                TweenFadeShow();
                break;
        }

        Transform parent = transform.parent;
        while (parent != null)
        {
            parent.gameObject.SetActive(true);
            parent = parent.parent;
        }
    }

    Tween tweenAction;

    void TweenFadeShow()
    {
        if (tween == null)
        {
            Debug.LogError($"Tween Ref is NULL in {name}", this);
            return;
        }

        // Убьем все лишние ссылки на этот твин объект
        tween.DOKill(true);
        gameObject.SetActive(true);

        tween.localScale = Vector3.one;
        canvasGroup.alpha = 0;

        tweenAction = canvasGroup.DOFade(1, timeToTweenAction).SetUpdate(unscaledTime).OnComplete(() => { OnShow?.Invoke(); });
    }

    void TweenSizeShow()
    {
        if (tween == null)
        {
            Debug.LogError($"Tween Ref is NULL in {name}", this);
            return;
        }

        // Убьем все лишние ссылки на этот твин объект
        tween.DOKill(true);
        gameObject.SetActive(true);

        tween.localScale = Vector3.one;

        // Обнулим скейл панели
        tween.localScale = Vector3.zero;
        // Если кастомных настроек нет,просто дефолт анимация
        if (customShowStepsVector.Count == 0)
        {
            tween.DOScale(defaultShowSizeVector, timeToTweenAction).SetUpdate(unscaledTime).OnComplete(() =>
            {
                tween.DOScale(Vector3.one, timeToTweenAction).SetUpdate(unscaledTime).OnComplete(() =>
                {
                    OnShow?.Invoke();
                    canOpenNextPanel = false;
                });
            });
        }
        else
        {
            StartCoroutine(ETweeSizeShow());
        }
    }

    IEnumerator ETweeSizeShow()
    {
        int step = 0;
        while (step < customShowStepsVector.Count)
        {
            tween.DOScale(customShowStepsVector[step], timeToTweenAction).OnComplete(() =>
            {
                step++;
            });
            yield return new WaitForSecondsRealtime(timeToTweenAction);
        }
    }

    private void OnDestroy()
    {
        all.Remove(this);
    }

    void TweenSlideShow()
    {

    }

    void AnimatorShow()
    {
        gameObject.SetActive(true);
        if (animator) animator.SetTrigger("Show");
    }
    #endregion

    #region HIDE_REGION
    public void Hide()
    {
        if (_panelShowHoldTime > 0)
        {
            delayHold = true;
            return;
        }
        OnCallHide?.Invoke();
        HideNoEvent();
        canvasGroup.interactable = false;
        delayHold = false;
    }
    public void HideNoEvent()
    {
        if (!canvasGroup)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (!canvasGroup) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        switch (animationType)
        {
            case AnimationType.TweenSize:
                TweenSizeHide();
                break;
            case AnimationType.TweenSlide:
                TweenSlideHide();
                break;
            case AnimationType.Animator:
                AnimatorHide();
                break;
            case AnimationType.noAnimation:
                gameObject.SetActive(false);
                OnHide.Invoke();
                break;
            case AnimationType.Fade:
                TweenFadeHide();
                break;
        }
    }


    void TweenFadeHide()
    {
        if (tween == null)
        {
            Debug.LogError("Tween Ref is NULL", this);
            return;
        }

        // Убьем все лишние ссылки на этот твин объект
        tween.DOKill(true);
        canvasGroup.alpha = 1;
        tweenAction = canvasGroup.DOFade(0, timeToTweenAction).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
            OnHide?.Invoke();
        });
    }

    void TweenSizeHide()
    {
        if (tween == null)
        {
            Debug.LogError("Tween Ref is NULL", this);
            return;
        }

        // Убьем все лишние ссылки на этот твин объект
        tween.DOKill(true);
        canvasGroup.alpha = 1;

        tween.DOScale(defaultHideSizeVector, timeToTweenAction).SetUpdate(unscaledTime).OnComplete(() =>
        {
            tween.DOScale(Vector3.zero, timeToTweenAction).SetUpdate(updateType, unscaledTime).OnComplete(() =>
             {
                 gameObject.SetActive(false);
                 if (!canOpenNextPanel) OnHide?.Invoke();
                 else
                 {
                     OnHideWithShowNext?.Invoke();
                     canOpenNextPanel = false;
                 }
             });
        });
    }

    // Если мы накопили кучу панелей для открытия,они буду открываться последовательно,а не разом
    void ShowNextQueuePanel()
    {
        if (uiPanelStack.Count > 0)
        {
            uiPanelStack.RemoveAt(0);
            if (uiPanelStack.Count > 0)
            {
                uiPanelStack[0].Show();
            }
        }
    }

    void TweenSlideHide()
    {

    }

    void AnimatorHide()
    {
        if (animator) animator.SetTrigger("Hide");
    }

    public void HideEvent()
    {
        gameObject.SetActive(false);
    }

    public void SetTweenAnimation()
    {
        animationType = AnimationType.TweenSize;
        animator.runtimeAnimatorController = null;
    }

    public void SetAnimatorAnimation()
    {
        animationType = AnimationType.Animator;
        animator.runtimeAnimatorController = animatorController;
    }

    #endregion
}
