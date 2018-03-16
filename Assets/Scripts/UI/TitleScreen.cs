using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using Rewired;

public class TitleScreen : MonoBehaviour
{	
	[Header("References")]
	public Image Title;

	[Header("Values")]
	public float fadeDuration;

	private Player[] players;

	private bool start;

	void Awake()
	{
		players = new Player[4];
		for (int i = 0; i < 4; i++) {
			players[i] = ReInput.players.GetPlayer(i);
		}
	}

	private void Start()
	{
		StartCoroutine (StartMenu());
	}


	private void Update()
	{
		for (int i = 0; i < 4; i++) {
			if (players [i].GetButtonDown ("Start") && !start) {
				start = true;

				StartCoroutine (PressPlay ());
			}
		}
	}

	IEnumerator Fade(float value)
	{
		Tween fade = Title.DOFade (value, fadeDuration);
		yield return new WaitWhile (() => (fade.IsPlaying()));
	}

	IEnumerator StartMenu()
	{
		yield return Fade (1f);
	}

	IEnumerator PressPlay()
	{
		yield return Fade (0f);

		// TODO Display PANALS !
	}
}
