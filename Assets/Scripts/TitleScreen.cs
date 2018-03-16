using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using Rewired;

public class TitleScreen : MonoBehaviour
{
    public InputInMainMenu inputs;

    [Header("References")]
    public Image Title;

    [Header("Values")]
    public float fadeDuration;

    private Player[] players;

    private bool start;
    private Tween fade;

    void Awake()
    {
        players = new Player[4];
        for (int i = 0; i < 4; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }
    }

    private void Start()
    {
        StartCoroutine(DisplayTitle());
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[i].GetButtonDown("Start") && !inputs.InTitle)
            {
                StartCoroutine(PressPlay());
            }
        }
    }

    IEnumerator Fade(float value, float duration)
    {
        fade = Title.DOFade(value, duration);
        yield return new WaitWhile(() => (fade.IsPlaying()));
    }

    IEnumerator DisplayTitle()
    {
        yield return Fade(1f, fadeDuration);
    }

    IEnumerator PressPlay()
    {
        if(fade != null)
        {
            fade.Kill();
            StopCoroutine(Fade(0f, 0f));
        }

        yield return Fade(0f, fadeDuration * 0.5f);

        inputs.InTitle = false;

        // TODO Display PANALS !
    }
}
