using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    public Text Tuto;
	public Image Frame;
    public TutoRock Rock;
    public Transform RockTarget;

    [Header("Values")]
    public float TimeBetweenLetters = 0.02f;
    public float IntroTime = 5f;
    public float MovementTime = 5f;
    public float WaveTime = 5f;
    public float RockMovementTime = 5f;
	public float HarpoonTime = 5f;

    [Header("Texts")]
    public string[] Texts;

    private void Start()
    {
        StartCoroutine(Progression());

        // Get all fishing boats to lock inputs.
    }

    IEnumerator Progression()
    {
        //Intro
        yield return PrintText(0);
        yield return new WaitForSeconds(IntroTime);

        PassToStep(1);        //Unlock Move

        //Movement
        yield return PrintText(1);
        yield return new WaitForSeconds(MovementTime);
        yield return PrintText(2);
		//Start Waves
        yield return new WaitForSeconds(WaveTime);

        PassToStep(2);         //Unlock Harpoon

        //Harpoon
        Rock.transform.DOMove(RockTarget.position, RockMovementTime);
        yield return PrintText(3);
		yield return new WaitUntil(() => Rock.HarpoonHit);
        yield return PrintText(4);
        yield return new WaitWhile(() => Rock.HarpoonHit);
        yield return PrintText(5);
		yield return new WaitForSeconds(HarpoonTime);

        PassToStep(3);        //Unlock Bombs

        //Bombs
        yield return PrintText(6);
		yield return new WaitUntil(() => Rock.RockExplode);
        yield return PrintText(7);
        yield return new WaitForSeconds(WaveTime);

        //End
        yield return PrintText(8);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        //Start Boss
    }

    IEnumerator PrintText(int i)
    {
        Tuto.text = "";
        foreach (char c in Texts[i])
        {
            Tuto.text += c;
            yield return new WaitForSeconds(TimeBetweenLetters);
        }
    }

    private void PassToStep(int step)
    {
        for (int i = 0; i < GameManager.instance.nbOfPlayers; i++)
        {
            // Player Input step ++
            GameManager.instance.shipMgr.PlayerInputs[i].TutoStep = step;
        }
    }
}
