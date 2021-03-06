﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI Tuto;
	public Image Frame;
    public TutoRock Rock;

	public GameObject tutoParent;

    [Header("Values")]
	public float TimeBetweenLetters = 0.02f;
    public float TimeBetweenTextTransitions = 1f;
	public float StartTime = 3f;
	public float FrameSpawnTime = 1f;
    public float ToyTime = 5f;
    public float rockSpeed = 5f;
	public float rockDepth = 5f;
	public float TutoEndTime = 3f;

    [Header("LD")]
    public GameObject LDparent;

	[Space]

	public float waveAmplitude;

	public float waveAppearTime;

    [Header("Texts")]
    public string[] Texts;

	private bool hasExploded;

	private Coroutine progressionCoroutine;

    public bool IsSkipping { get; private set; }

    private void Start()
    {
        LDparent.SetActive(false);
        GameManager.instance.tutorial = this;
		if(GameManager.instance.tutoEnabled){
			Tuto.text = "";
			hasExploded = true;
			progressionCoroutine = StartCoroutine(Progression());
			// Get all fishing boats to lock inputs.
		}

		else{
			StartCoroutine(EndTutoCoroutine(0));
		}
    }

	public void SkipTuto()
    {
		if(progressionCoroutine != null)
        {
			StopCoroutine(progressionCoroutine);
		}

		StartCoroutine(EndTutoCoroutine(TutoEndTime));
	}

	private IEnumerator EndTutoCoroutine(float tutoEndTime)
    {
        IsSkipping = true;

		Vector3 endPos = Rock.Mover.SelfTransform.position;
		endPos.y -= 10f;

		int waveId = GameManager.instance.ground.ZoneWaveId;
		WaveOptions wave = GameManager.instance.ground.waveManager.Waves[waveId];
		wave.amplitude = waveAmplitude;
		GameManager.instance.ground.waveManager.ChangeWave(waveId, wave);

		Rock.Mover.enabled = false;
		Rock.Mover.SelfTransform.DOMove(endPos, 10f / rockSpeed);

        GameManager.instance.shipMgr.ResurrectAll();

        // Activate Boss
        Frame.DOFade(0f, FrameSpawnTime);
		Tuto.DOFade(0f, FrameSpawnTime);
        
		yield return new WaitWhile(() => DOTween.IsTweening(Frame));
		yield return new WaitForSeconds(tutoEndTime);
		GameManager.instance.OnEndTuto();

        LDparent.SetActive(true);

        Destroy(tutoParent);
	}

    IEnumerator Progression()
    {
		yield return new WaitForSeconds(StartTime);

		// UI
		Frame.DOFade(1f, FrameSpawnTime);
		yield return new WaitWhile(() => DOTween.IsTweening(Frame));

        // Intro
        yield return PrintText(0);
        yield return new WaitForSeconds(TimeBetweenTextTransitions);

        PassToStep(1);        // Unlock Move

        // Movement
        yield return PrintText(1);
        yield return new WaitForSeconds(ToyTime);
		
		int waveId = GameManager.instance.ground.ZoneWaveId;
		WaveOptions wave = GameManager.instance.ground.waveManager.Waves[waveId];

		float time = 0;

		do{
			wave.amplitude = (waveAmplitude / waveAppearTime) * time;
			GameManager.instance.ground.waveManager.ChangeWave(waveId, wave);
			yield return null;
			time += Time.deltaTime;
		}while(time < waveAppearTime);

        yield return PrintText(2);
        yield return new WaitForSeconds(ToyTime);

        PassToStep(2);         //Unlock Harpoon

        //Harpoon
		Vector3 rockPosition = Rock.Mover.SelfTransform.position;		

		do{
			yield return null;
			rockPosition.y += rockSpeed * Time.deltaTime;
			Rock.Mover.SelfTransform.position = rockPosition;
		}while(rockPosition.y < GameManager.instance.ground.GetTransformInfo(rockPosition).position.y);

		Rock.Mover.enabled = true;

        yield return PrintText(3);
        yield return new WaitForSeconds(TimeBetweenTextTransitions);
        yield return PrintText(4);
        yield return new WaitForSeconds(TimeBetweenTextTransitions);
		yield return new WaitUntil(() => Rock.HarpoonHit);
        yield return PrintText(5);
        yield return new WaitWhile(() => Rock.HarpoonHit);
		yield return new WaitForSeconds(ToyTime);

        PassToStep(3);        //Unlock Bombs

        //Bombs
        yield return PrintText(6);
		hasExploded = false;
		yield return new WaitUntil(() => hasExploded);
        yield return PrintText(7);
        yield return new WaitForSeconds(ToyTime);

        //Death & Resurect
        yield return PrintText(8);
        yield return new WaitForSeconds(TimeBetweenTextTransitions);
		GameManager.instance.shipMgr.ResurrectAll();
        GameManager.instance.shipMgr.ChoosePlayerManagerToAttack().Death();
        yield return PrintText(9);
        yield return new WaitUntil(AllPlayerAlive);

        //Lost
        yield return PrintText(10);
        yield return new WaitForSeconds(ToyTime);

        //End
        yield return PrintText(11);
        yield return new WaitForSeconds(ToyTime);

		StartCoroutine(EndTutoCoroutine(TutoEndTime));
    }

    IEnumerator PrintText(int i)
    {
        Texts[i] = Texts[i].Replace('\\', '\n');
        Tuto.text = Texts[i];

        int visibleCharacters = Texts[i].Length;
        
        for(int j = 0; j < visibleCharacters+1; j++){

            Tuto.maxVisibleCharacters = j;
            yield return new WaitForSeconds(TimeBetweenLetters);
        }

		// string stock = "";
		// bool inTag = false;

        // Tuto.text = "";
		// Texts[i] = Texts[i].Replace('\\', '\n');

        // foreach (char c in Texts[i])
        // {
		// 	if (c == '>')
		// 	{
		// 		inTag= false;
		// 		Tuto.text += stock + c;
		// 	}
		// 	else if (c == '<' || inTag)
		// 	{
		// 		inTag = true;
		// 		stock += c;
		// 	}
		// 	else
		// 	{
		// 		Tuto.text += c;
		// 		yield return new WaitForSeconds(TimeBetweenLetters);
		// 	}
        // }
    }

	private bool AllPlayerAlive(){
		return (GameManager.instance.shipMgr.playerAlive == GameManager.instance.nbOfPlayers);
	}

    private void PassToStep(int step)
    {
        GameManager.instance.shipMgr.LockInputs(step);
    }
	
	private void OnExplode(){
		if(!hasExploded){
			hasExploded = true;
		}
	}
}
