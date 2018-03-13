using System.Collections;
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

    public override void SetBoss(BossAI boss)
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

            whaleAI.WhaleChildTransform.localPosition = target.position + whaleAI.WhaleTransform.up * state.startHeight;
            whaleAI.WhaleChildTransform.localScale = Vector3.zero;

            // Apply Random rotation...
            whaleAI.WhaleChildTransform.localRotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);

            whaleAI.Whale.SetActive(true);

            whaleAI.WhaleChildTransform.DOScale(Vector3.one, state.emergingDuration);
            whaleAI.WhaleChildTransform.DOLocalMove(target.position, state.emergingDuration);

            yield return new WaitForSeconds(state.emergingDuration - state.backAttackTime);

            whaleAI.WhaleAnimator.Play("BackAttack");

            yield return new WaitForSeconds(state.backAttackTime);

            // Shockwave.
            GameManager.instance.ground.CreateImpact(whaleAI.WhaleTransform.position);
            whaleAI.whaleReferences.PlaySplashFX();

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
