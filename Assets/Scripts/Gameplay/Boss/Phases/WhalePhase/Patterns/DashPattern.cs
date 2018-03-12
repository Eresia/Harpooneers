using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DashPattern : BossPattern {

    private DashState dashState;
    private WhalePhaseAI whaleAI;

    private Tweener tween;

    public DashPattern(DashState dashState)
    {
        this.dashState = dashState;
    }

    public override void SetBoss(BossAI boss)
    {
        base.SetBoss(boss);

        whaleAI = boss as WhalePhaseAI;
    }

    protected override void ExecutePattern() {
        
        if(tween != null)
        {
            tween.Kill();
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

        yield return new WaitForSeconds(dashState.WaitBeforeSpawn);

        whaleAI.spawningFX.Stop();
        whaleAI.Whale.SetActive(true);

        whaleAI.WhaleTransform.position = pos;
        whaleAI.WhaleTransform.rotation = rot;

        yield return MoveAndDash(horizontalDash);

        OnPatternFinished();
	}

    IEnumerator MoveAndDash(bool horizontalDash)
    {
        // Begin to move left ?
        bool _left;
        _left = Random.Range(0, 1) > 0.5 ? true : false;

        // Number of move before dash ?
        int move = Random.Range(dashState.TurnMin, dashState.TurnMax + 1);

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
                destination.z = Random.Range(dashState.minZ, dashState.maxZ);
            }
            else
            {
                destination.x = Random.Range(dashState.minX, dashState.maxX);
            }

            // Uniform movement.
            float dist = Vector3.Distance(whaleAI.WhaleTransform.position, destination);
            moveDuration = Mathf.Abs(dist / dashState.translateSpeed);

            tween = whaleAI.WhaleTransform.DOMove(destination, moveDuration);

            yield return new WaitForSeconds(Mathf.Abs(moveDuration));

            _left = !_left;
        }
        
        yield return new WaitForSeconds(dashState.WaitBeforeDash);
        
        float distance;
        if (horizontalDash)
        {
            distance = boss.bossMgr.width + dashState.whaleOffset;
        }
        else
        {
            distance = boss.bossMgr.height + dashState.whaleOffset;
        }

        destination = whaleAI.WhaleTransform.position + whaleAI.WhaleTransform.forward * distance;
        moveDuration = Mathf.Abs(distance / dashState.dashSpeed);

        tween = whaleAI.WhaleTransform.DOMove(destination, moveDuration).SetEase(Ease.InCubic);

        // TODO Generate wave on the sea along the dash
        // TODO song etc

        // Wait while the dash isn't finished.
        yield return new WaitWhile(() => (tween.IsPlaying()));

        whaleAI.Whale.SetActive(false);
    }

    protected override void OnStopPattern()
    {
        // Dash isn't stoppable.
    }
}
