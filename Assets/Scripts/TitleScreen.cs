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

    public Animator[] panels;

    public GameObject creditPanel;

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
            if (players[i].GetButtonDown("Submit") && inputs.InTitle)
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

        yield return Fade(0f, fadeDuration * 0.25f);

        inputs.InTitle = false;
        
        WaitForSeconds delay = new WaitForSeconds(0.25f);

        foreach (Animator panel in panels)
        {
            panel.SetTrigger("playAnim");
            // Feedback vagues

            Vector2 pos = GameManager.instance.ground.GetSeaPosition(panel.transform.position + new Vector3(0f, 0f, 100f));
            GameManager.instance.ground.waveManager.CreateImpact(pos, 0.5f, 0f, 0.01f, 1f, 1f, 5f);

            yield return delay;
        }

        yield return new WaitForSeconds(0.5f);

        creditPanel.SetActive(true);
    }
}
