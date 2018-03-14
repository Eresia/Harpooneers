using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    public Text Tuto;
    public GameObject TutoRock;
    public Transform RockTarget;

    [Header("Values")]
    public float TimeBetweenLetters = 0.02f;
    public float IntroTime = 5f;
    public float MovementTime = 5f;
    public float WaveTime = 5f;
    public float RockMovementTime = 5f;

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

        PassToStep(1);

        //Movement
        //Unlock Move
        yield return PrintText(1);
        yield return new WaitForSeconds(MovementTime);
        yield return PrintText(2);
        yield return new WaitForSeconds(WaveTime);

        PassToStep(2);

        //Harpoon
        TutoRock.transform.DOMove(RockTarget.position, RockMovementTime);
        //Unlock Harpoon
        yield return PrintText(3);
        //Wait for Hit on Rock
        //Unlock Rope
        yield return PrintText(4);
        //Wait for Rope Size Change
        yield return PrintText(5);
        //wait for cut

        PassToStep(3);

        //Bombs
        //Unlock Bombs
        yield return PrintText(6);
        //Wait for explosion
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
