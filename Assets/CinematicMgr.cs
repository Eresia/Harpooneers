using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using DG.Tweening;
using TMPro;

public class CinematicMgr : MonoBehaviour {

    public Image topPart;
    public Image bottomPart;
    public TextMeshProUGUI bossNameText;

    public float borderAnimDuration;
    public float fadeDuration;

    public void Awake()
    {
        GameManager.instance.cinematicMgr = this;

        topPart.fillAmount = 0f;
        bottomPart.fillAmount = 0f;

        Color alpha0 = Color.white;
        alpha0.a = 0f;

        bossNameText.color = alpha0;
    }

    public IEnumerator Play(string bossName = "The boss")
    {
        bossNameText.text = bossName;

        topPart.DOFillAmount(1f, borderAnimDuration).SetEase(Ease.Linear);
        Tween borderAnim = bottomPart.DOFillAmount(1f, borderAnimDuration).SetEase(Ease.Linear);

        yield return new WaitWhile(() => (borderAnim.IsPlaying()));

        bossNameText.DOFade(1f, fadeDuration);
    }

    public IEnumerator Stop()
    {
        Tween fadeAnim = bossNameText.DOFade(0f, fadeDuration);

        yield return new WaitWhile(() => (fadeAnim.IsPlaying()));

        topPart.DOFillAmount(0f, borderAnimDuration).SetEase(Ease.Linear);
        bottomPart.DOFillAmount(0f, borderAnimDuration).SetEase(Ease.Linear);
    }
}
