using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;
using System;

public class FadeInOut : MonoBehaviour {

    public Image img;
    public float fadeDuration;

    private Action onFinished;

    private void Reset()
    {
        img = GetComponent<Image>();
    }

    private void Awake()
    {
        GameManager.instance.FadeMgr = this;
    }

    public void FadeIn()
    {
        img.DOFade(0f, fadeDuration).SetEase(Ease.Linear);
    }

    public void FadeOut(Action onFinished)
    {
        this.onFinished = onFinished;

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Tween fade = img.DOFade(1f, fadeDuration).SetEase(Ease.Linear);

        yield return new WaitWhile(() => fade.IsPlaying());

        onFinished();
    }
}
