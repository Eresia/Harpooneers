using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GeyserPattern : BossPattern {

    private GeyserState state;
    private WhalePhaseAI whaleAI;

    private bool isDiving;

    private Tween posTween;
    private Tween rotTween;
    private Tween locTween;

    public GeyserPattern(GeyserState state)
    {
        this.state = state;
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        whaleAI = boss as WhalePhaseAI;
    }

    protected override void ExecutePattern()
    {
		boss.StartCoroutine(SpawnGeysersThenWait());
        
        isDiving = false;
    }

    protected override void OnStopPattern()
    {
        // Stop all tweens.
        if (posTween.IsPlaying())
        {
            posTween.Kill();
            rotTween.Kill();
            locTween.Kill();
        }

        // Check if whale isn't already diving.
        if(!isDiving)
        {
            // Stop behaviour directly.
            boss.StopAllCoroutines();

            boss.StartCoroutine(WhaleDive());
        }
    }

    private void ChooseSpawn()
    {
        int spawnChoosen = Random.Range(0, 4);

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        switch (spawnChoosen)
        {
            case 0:
                pos = boss.bossMgr.north.position;
                rot = Quaternion.Euler(0f, 180f, 0f);
                break;

            case 1:
                pos = boss.bossMgr.west.position;
                rot = Quaternion.Euler(0f, -90f, 0f);
                break;

            case 2:
                pos = boss.bossMgr.south.position;
                rot = Quaternion.Euler(0f, 0f, 0f);
                break;

            case 3:
                pos = boss.bossMgr.east.position;
                rot = Quaternion.Euler(0f, 90f, 0f);
                break;
        }

        // Move the FX containers.
        whaleAI.FX.transform.position = pos;
        whaleAI.FX.transform.rotation = rot;

        whaleAI.WhaleTransform.position = pos;
        whaleAI.WhaleTransform.rotation = rot;
    }

    // Decide from which spawn the whale will dash.
    private IEnumerator SpawnGeysersThenWait()
	{
        // Geysers chase players.
        for (int i = 0; i < whaleAI.Geysers.Length; i++)
        {
            whaleAI.Geysers[i].transform.position = GameManager.instance.shipMgr.players[i].transform.GetChild(0).position;
            whaleAI.Geysers[i].playerTarget = GameManager.instance.shipMgr.players[i].transform.GetChild(0).gameObject;

            whaleAI.Geysers[i].gameObject.SetActive(true);
        }

        yield return new WaitWhile(() => (whaleAI.Geysers[0].isActiveAndEnabled));

        yield return new WaitForSeconds(state.delayAfterGeyserChase);
        
        ChooseSpawn();
        
        // Whale emerged
        Vector3 targetPos = whaleAI.WhaleTransform.position + whaleAI.WhaleTransform.forward * state.emergePosEnd.z + whaleAI.WhaleTransform.up * state.emergePosEnd.y;
        
        // Set original pos and rot 
        whaleAI.WhaleTransform.Translate(whaleAI.WhaleTransform.forward * state.emergePosStart.z + whaleAI.WhaleTransform.up * state.emergePosStart.y);
        whaleAI.WhaleChildTransform.localRotation = Quaternion.Euler(state.emergeRotationStart);
        whaleAI.Whale.SetActive(true);
        whaleAI.WhaleAnimator.SetBool("Swim", true);

        // Do emerging.
        posTween = whaleAI.WhaleTransform.DOLocalMove(targetPos, state.emergingDuration);
        rotTween = whaleAI.WhaleChildTransform.DOLocalRotate(state.emergeRotationEnd, state.emergingDuration);

        yield return new WaitForSeconds(state.emergingDuration);

        // Stop to swin -> Idle.
        whaleAI.WhaleAnimator.SetBool("Swim", false);

        // Boss is now vulnerable.
        whaleAI.EnableEyeCollisions(true);

        // Whale is emerged... WAIT

        yield return new WaitForSeconds(state.waitDuration);

        whaleAI.EnableEyeCollisions(false);

        // Whale dives.
        yield return WhaleDive();
	}

    private IEnumerator WhaleDive()
    {
        isDiving = true;
        
        posTween = whaleAI.WhaleTransform.DOLocalMove(whaleAI.WhaleTransform.up * state.diveHeightEnd + whaleAI.WhaleTransform.forward * state.diveForwardEnd, state.divingDuration);
        rotTween = whaleAI.WhaleChildTransform.DOLocalRotate(state.diveRotationEnd, state.divingDuration);
        locTween = whaleAI.WhaleChildTransform.DOScale(Vector3.zero, state.divingDuration);

        whaleAI.WhaleAnimator.Play("Dash");
        whaleAI.WhaleAnimator.SetBool("Swim", true);

        yield return new WaitWhile(() => (posTween.IsPlaying()));

        whaleAI.ResetWhaleTransform();

        OnPatternFinished();
    }
}
