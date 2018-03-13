using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
	public Text Tuto;
	
	[Header("Values")]
	public float TimeBetweenLetters = 0.02f;
	public float MovementTime = 5f;

	[Header("Texts")]
	public string[] Texts;

	private void Start()
	{
		StartCoroutine(Progression());
	}

	IEnumerator Progression()
	{
		//Intro
		yield return PrintText(0);
		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); //Input A @ Player0
		//Tuto Mouvement
		yield return PrintText(1);
		yield return new WaitForSeconds(MovementTime);

		//Tuto Harpon
		yield return PrintText(2);

		//Tuto Bombe

		
	}

	IEnumerator PrintText(int i)
	{
		Tuto.text = "";
		foreach(char c in Texts[i])
		{
			Tuto.text += c;
			yield return new WaitForSeconds(TimeBetweenLetters);
		}
	}
}
