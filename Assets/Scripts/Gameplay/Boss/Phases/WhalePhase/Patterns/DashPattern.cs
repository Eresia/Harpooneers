using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DashPattern : BossPattern {

    private DashState state;
    private WhalePhaseAI whaleAI;

    private Tweener dashTween;

    private bool isDashing;

    public DashPattern(DashState dashState)
    {
        this.state = dashState;
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        whaleAI = boss as WhalePhaseAI;

        // Callback when a bomb explodes.
        
    }

    protected override void ExecutePattern() {
        
        if(isDashing)
        {
            dashTween.Kill();
            dashTween = null;
        }

        if(state.diveOnExplode)
        {
            whaleAI.bodyCollider.GetComponent<WhaleBody>().OnWhaleExplode = OnStopPattern;
        }

        boss.StartCoroutine(SelectSpawnThenDash());
	}

    // Decide from which spawn the whale will dash.
    private IEnumerator SelectSpawnThenDash()
	{
        int spawnChoosen = Random.Range(0, 4);

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        bool horizontalDash = false;

        switch (spawnChoosen)
        {
            case 0:
                pos = boss.bossMgr.north.position;
                rot = Quaternion.Euler(0f, 180f, 0f);
                break;

            case 1:
                pos = boss.bossMgr.west.position;
                rot = Quaternion.Euler(0f, -90f, 0f);
                horizontalDash = true;
                break;

            case 2:
                pos = boss.bossMgr.south.position;
                rot = Quaternion.Euler(0f, 0f, 0f);
                break;

            case 3:
                pos = boss.bossMgr.east.position;
                rot = Quaternion.Euler(0f, 90f, 0f);
                horizontalDash = true;
                break;
        }

        // Move the FX containers.
        whaleAI.FX.transform.position = pos;
        whaleAI.FX.transform.rotation = rot;

        whaleAI.spawningFX.Play();

        yield return new WaitForSeconds(state.WaitBeforeSpawn);

        whaleAI.spawningFX.Stop();
        whaleAI.Whale.SetActive(true);

        whaleAI.WhaleTransform.position = pos;
        whaleAI.WhaleTransform.rotation = rot;

        yield return MoveAndDash(horizontalDash);

        whaleAI.ResetWhaleTransform();

        OnPatternFinished();
	}

    IEnumerator MoveAndDash(bool horizontalDash)
    {
        // Begin to move left ?
        bool _left;

        // Number of move before dash ?
        int move = Random.Range(state.TurnMin, state.TurnMax + 1);

        Vector3 destination;
        float moveDuration = 1f;

        _left = Random.Range(0, 1) > 0.5 ? true : false;

        // Move whale X times before dashing.
        for (int i = 0; i < move; i++)
        {
            destination = whaleAI.WhaleTransform.position;

            // Random the new destination.
            if (horizontalDash)
            {
                destination.z = Random.Range(state.minZ, state.maxZ);
            }
            else
            {
                destination.x = Random.Range(state.minX, state.maxX);
            }

            // Uniform movement.
            float dist = Vector3.Distance(whaleAI.WhaleTransform.position, destination);
            moveDuration = Mathf.Abs(dist / state.translateSpeed);

            dashTween = whaleAI.WhaleTransform.DOMove(destination, moveDuration);

            if(_left)
            {
                whaleAI.WhaleAnimator.Play("LeftDrift");
            }
            else
            {
                whaleAI.WhaleAnimator.Play("RightDrift");
            }

            yield return new WaitForSeconds(Mathf.Abs(moveDuration));

            _left = !_left;
        }
        
        yield return new WaitForSeconds(state.WaitBeforeDash);

        isDashing = true;

        float distance;
        if (horizontalDash)
        {
            distance = boss.bossMgr.width + state.whaleOffset;
        }
        else
        {
            distance = boss.bossMgr.height + state.whaleOffset;
        }

        destination = whaleAI.WhaleTransform.position + whaleAI.WhaleTransform.forward * distance;
        moveDuration = Mathf.Abs(distance / state.dashSpeed);

        dashTween = whaleAI.WhaleTransform.DOMove(destination, moveDuration).SetEase(Ease.InCubic);

        whaleAI.WhaleAnimator.Play("Dash");

        // Splash FX
        whaleAI.whaleReferences.PlaySplashFX();


        // TODO Generate wave on the sea along the dash
        // TODO song etc

        // Wait while the dash isn't finished.
        yield return new WaitWhile(() => (dashTween.IsPlaying()));

        isDashing = false;
    }

    protected override void OnStopPattern()
    {
        // Dive if explodes when dashing.

        if (isDashing)
        {
            boss.StopAllCoroutines();
            
            dashTween.Kill();
            dashTween = null;

            // Camera Shake
            GameManager.instance.camMgr.Shake();

            boss.StartCoroutine(WhaleDive());
        }
    }

    private IEnumerator WhaleDive()
    {
        // Splash FX
        whaleAI.whaleReferences.PlaySplashFX();

        Tween t = whaleAI.WhaleChildTransform.DOLocalMove(whaleAI.WhaleTransform.up * state.diveHeightEnd + whaleAI.WhaleTransform.forward * state.diveForwardEnd, state.divingDuration);
        whaleAI.WhaleChildTransform.DOLocalRotate(state.diveRotationEnd, state.divingDuration);
        whaleAI.WhaleChildTransform.DOScale(Vector3.zero, state.divingDuration);

        whaleAI.WhaleAnimator.Play("Dash");
        whaleAI.WhaleAnimator.SetBool("Swim", true);

        yield return new WaitWhile(() => (t.IsPlaying()));

        whaleAI.ResetWhaleTransform();

        OnPatternFinished();
    }
}
