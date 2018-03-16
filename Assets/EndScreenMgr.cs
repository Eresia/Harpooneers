using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;
using System.Collections;

public class EndScreenMgr : MonoBehaviour {
    
    public Image title;
    public TextMeshProUGUI text;

    private void Awake()
    {
        GameManager.instance.endScreenMgr = this;

        text.enabled = false;
        gameObject.SetActive(false);
    }

    public void Display()
    {
        gameObject.SetActive(true);

        // TODO Fade in
        
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        Tween fade = title.DOFade(1f, 1.5f).SetEase(Ease.Linear);

        yield return new WaitWhile(() => (fade.IsPlaying()));

        GameManager.instance.IsEndScreen = true;
        text.enabled = true;
    }
}
