using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI Tuto;
	public Image Frame;
    public TutoRock Rock;
    public Transform RockTarget;

    [Header("Values")]
	public float TimeBetweenLetters = 0.02f;
	public float StartTime = 3f;
	public float FrameSpawnTime = 1f;
    public float IntroTime = 5f;
    public float MovementTime = 5f;
    public float RockMovementTime = 5f;
	public float HarpoonTime = 5f;
	public float TutoEndTime = 3f;

    [Header("Texts")]
    public string[] Texts;

    private void Start()
    {
		Tuto.text = "";
        StartCoroutine(Progression());
        // Get all fishing boats to lock inputs.
    }

    IEnumerator Progression()
    {
		yield return new WaitForSeconds(StartTime);

		//UI
		Frame.DOFade(1f, FrameSpawnTime);
		yield return new WaitWhile(() => DOTween.IsTweening(Frame));

        //Intro
        yield return PrintText(0);
        yield return new WaitForSeconds(IntroTime);

        PassToStep(1);        //Unlock Move

        //Movement
        yield return PrintText(1);
        yield return new WaitForSeconds(MovementTime);
        yield return PrintText(2);
		//Start Waves --------------------------------------------------BASTIEN ICI------------------------------------------
        yield return new WaitForSeconds(MovementTime);

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
        yield return new WaitForSeconds(MovementTime);

        //End
        yield return PrintText(8);
        yield return new WaitForSeconds(MovementTime);

        //Start Boss
		Frame.DOFade(0f, FrameSpawnTime);
		Tuto.DOFade(0f, FrameSpawnTime);
		yield return new WaitWhile(() => DOTween.IsTweening(Frame));
		yield return new WaitForSeconds(TutoEndTime);

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
