﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class BackAttackPattern : BossPattern
{
    private BackAttackState state;
    private WhalePhaseAI whaleAI;

    private Transform target;

    public BackAttackPattern(BackAttackState state)
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
        boss.StartCoroutine(BackAttack());
    }

    private IEnumerator BackAttack()
    {
        for (int i = 0; i < state.attackAmount; i++)
        {
            target = GameManager.instance.shipMgr.ChoosePlayerToAttack();

            whaleAI.WhaleChildTransform.localPosition = target.position + whaleAI.WhaleTransform.up * -state.startHeight;
            whaleAI.WhaleChildTransform.localScale = Vector3.zero;

            // Apply Random rotation...
            whaleAI.WhaleChildTransform.localRotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);

            whaleAI.Whale.SetActive(true);

            whaleAI.WhaleChildTransform.DOScale(Vector3.one, state.emergingDuration);
            whaleAI.WhaleChildTransform.DOLocalMove(target.position, state.emergingDuration);

            whaleAI.bodyCollider.enabled = false;
            whaleAI.tailCollider.enabled = false;
            yield return new WaitForSeconds(state.emergingDuration - state.backAttackTime);

            whaleAI.WhaleAnimator.Play("BackAttack");
            // Splash FX
            whaleAI.whaleReferences.PlaySplashFX();

            yield return new WaitForSeconds(state.backAttackTime);

            // Splash FX
            whaleAI.whaleReferences.PlaySplashFX();

            whaleAI.bodyCollider.enabled = true;
            whaleAI.tailCollider.enabled = true;




            // Shockwave.
			Vector2 pos = GameManager.instance.ground.GetSeaPosition(target.position);
			GameManager.instance.ground.waveManager.CreateImpact(pos, 2f, 0f, 0.05f, 2f, 0.5f, 15f);
           

            yield return new WaitForSeconds(state.waitTimeAfterAttack);

            // Dive !!!
            yield return WhaleDive();
        }

        whaleAI.ResetWhaleTransform();

        OnPatternFinished();
    }

    protected override void OnStopPattern()
    {
        // Do nothing.
    }

    private IEnumerator WhaleDive()
    {
        Tween t = whaleAI.WhaleChildTransform.DOLocalMove(whaleAI.WhaleTransform.up * state.diveHeightEnd + whaleAI.WhaleTransform.forward * state.diveForwardEnd, state.divingDuration);
        whaleAI.WhaleChildTransform.DOLocalRotate(state.diveRotationEnd, state.divingDuration);
        whaleAI.WhaleChildTransform.DOScale(Vector3.zero, state.divingDuration);

        whaleAI.WhaleAnimator.Play("Dash");
        whaleAI.WhaleAnimator.SetBool("Swim", true);

        yield return new WaitWhile(() => (t.IsPlaying()));
    }
}
