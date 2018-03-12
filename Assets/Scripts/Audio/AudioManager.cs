using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent (typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{

	[System.Serializable]
	public enum PossibleSound
	{
		SEA
	}

	public AudioClip menuMusic;
	[Range(0f, 1f)]
	public float menuMusicVolume;
	public AudioClip inGameMusic;
	[Range(0f, 1f)]
	public float inGameMusicVolume;

	[Space]

	private Dictionary<PossibleSound, AudioSource> persistantSounds;

	private AudioSource musicController;
	private AudioClip actualMusic;
	private List<AudioSource> oneTimeSounds;

	private void Awake ()
	{
		musicController = GetComponent<AudioSource> ();
		oneTimeSounds = new List<AudioSource> ();

		persistantSounds = new Dictionary<PossibleSound, AudioSource> ();
	}

	void Start ()
	{
		if (menuMusic != null) {
			PlayMusic (menuMusic, menuMusicVolume);
		}
	}

	void Update ()
	{
		List<int> removingSource = new List<int> ();
		for (int i = 0; i < oneTimeSounds.Count; i++) {
			if ((oneTimeSounds [i] == null) || !oneTimeSounds [i].isPlaying) {
				removingSource.Insert (0, i);
			}
		}

		foreach (int i in removingSource) {
			if (oneTimeSounds [i] != null) {
				AudioSource song = oneTimeSounds [i];
				Destroy (song);
			}
			oneTimeSounds.RemoveAt (i);
		}
	}

	public void PlaySoundOneTime (AudioClip sound, float volume = 1f)
	{
		//AudioSource newSource = targetObject.AddComponent<AudioSource>();
		AudioSource newSource = gameObject.AddComponent<AudioSource> ();
		newSource.clip = sound;
		newSource.volume = volume;
		newSource.Play ();
		oneTimeSounds.Add (newSource);
	}


	public void PlaySoundsOneTime (AudioClip[] sounds, float volume)
	{
		foreach (AudioClip s in sounds) {
			PlaySoundOneTime (s, volume);
		}
	}

	public void PlayRandomSoundOneTimeIn (AudioClip[] soundArray, float volume)
	{
		int choice = Random.Range (0, soundArray.Length);
		PlaySoundOneTime (soundArray [choice], volume);
	}

	public void CreatePersistantSound (PossibleSound id, AudioClip sound, float volume)
	{
		if (persistantSounds.ContainsKey (id)) {
			if (persistantSounds [id] != null) {
				persistantSounds [id].Stop ();
			}
		}

		//persistantSounds[id] = targetObject.AddComponent<AudioSource>();
		persistantSounds [id] = gameObject.AddComponent<AudioSource> ();
		persistantSounds [id].clip = sound;
		persistantSounds [id].loop = true;
		persistantSounds [id].volume = volume;
	}

	public void PlayPersistantSound (PossibleSound id){
		PlayPersistantSound(id, 0.1f);
	}

	public void PlayPersistantSound (PossibleSound id, float timeFade)
	{
		if (persistantSounds.ContainsKey (id) && (persistantSounds [id] != null)) {
			if (!persistantSounds [id].isPlaying) {
				//Killer l'autre tween
				DOTween.Kill (id, false);
			
				persistantSounds [id].volume = 0;

				persistantSounds [id].Play ();

				persistantSounds [id].DOFade (1, timeFade);
			}
		}
	}

	public void TooglePausePersistantSong (PossibleSound id)
	{
		if (persistantSounds.ContainsKey (id)) {
			if (persistantSounds [id].isPlaying) {
				persistantSounds [id].Pause ();
			} else {
				persistantSounds [id].UnPause ();
			}
		}
	}

	public void StopPersistantSound (PossibleSound id){
		StopPersistantSound(id, 0.1f);
	}

	public void StopPersistantSound (PossibleSound id, float timeFade)
	{
		if (persistantSounds.ContainsKey (id)) {
			persistantSounds [id].DOFade (0, timeFade)
				.OnComplete (() => PersistantSoundReset (id))
				.OnKill (() => persistantSounds [id].volume = 1f)
				.SetId (id);
		}
	}

	void PersistantSoundReset (PossibleSound id)
	{
		persistantSounds [id].Stop ();
		persistantSounds [id].volume = 1f;
	}

	public void PlayMusic (AudioClip newMusic, float timeFade){
		PlayMusic(newMusic, timeFade, musicController.volume);
	}

	public void PlayMusic (AudioClip newMusic, float timeFade, float volume)
	{
		if (newMusic == null) {
			if (actualMusic != null) {
				musicController.DOFade (0, timeFade).OnComplete (() => actualMusic = null);
			}
		} else if (!newMusic.Equals (actualMusic)) {
			musicController.DOFade (0, timeFade).OnComplete (() => FadeMusicPlay (newMusic, volume, timeFade));
		}
	}

	void FadeMusicPlay (AudioClip newMusic, float actualVolume, float timeFade)
	{
		musicController.Stop ();
		musicController.clip = newMusic;
		musicController.Play ();
		musicController.DOFade (actualVolume, timeFade);

	}

	public void TooglePauseMusic ()
	{
		if (musicController.isPlaying) {
			musicController.Pause ();
		} else {
			musicController.UnPause ();
		}
	}
}
