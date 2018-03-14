using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SpawnPattern : BossPattern {

    private SpawnState spawnState;
    private WhalePhaseAI whaleAI;

    public SpawnPattern(SpawnState spawnState)
    {
        this.spawnState = spawnState;
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        whaleAI = boss as WhalePhaseAI;
    }

    protected override void ExecutePattern() {
		whaleAI.StartCoroutine(Spawn());
	}

	private IEnumerator Spawn(){
		// Move the FX containers.
		Vector3 pos = boss.bossMgr.center.position;
        Quaternion rot = Quaternion.Euler(0f, 180f, 0f);
        whaleAI.FX.transform.position = pos;
        whaleAI.FX.transform.rotation = rot;

		whaleAI.WhaleTransform.position = pos - new Vector3(0, spawnState.depth, 0);
        whaleAI.WhaleTransform.rotation = rot;

		whaleAI.Whale.SetActive(true);

        whaleAI.spawningFX.Play();

		Tween tween = whaleAI.WhaleTransform.DOMove(pos - new Vector3(0, spawnState.hight, 0), spawnState.WaitBeforeSpawn);
        yield return new WaitWhile(tween.IsPlaying);

        whaleAI.spawningFX.Stop();

		OnPatternFinished();
	}

    protected override void OnStopPattern()
    {
        // Spawn isn't stoppable.
    }
}
