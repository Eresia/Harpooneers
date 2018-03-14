using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SpawnPattern : BossPattern {

    private SpawnState state;
    private WhalePhaseAI whaleAI;

    public SpawnPattern(SpawnState spawnState)
    {
        this.state = spawnState;
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
		Vector3 pos = boss.bossMgr.north.position + new Vector3(0, 0, -state.offset);
        Quaternion rot = Quaternion.Euler(0f, 180f, 0f);
        whaleAI.FX.transform.position = pos;
        whaleAI.FX.transform.rotation = rot;

		whaleAI.WhaleTransform.position = pos - new Vector3(0, state.beginDepth, 0);
        whaleAI.WhaleTransform.rotation = rot;

		whaleAI.Whale.SetActive(true);

		yield return new WaitForSeconds(2f);

        whaleAI.spawningFX.Play();

		Tween tween = whaleAI.WhaleTransform.DOMove(pos - new Vector3(0, 0, 0), state.WaitToSurface);
        yield return new WaitWhile(tween.IsPlaying);

        whaleAI.spawningFX.Stop();

		yield return new WaitForSeconds(state.WaitOnSurface);

		tween = whaleAI.WhaleTransform.DOMove(pos - new Vector3(0, state.endDepth, 0), state.WaitToSurface);
        yield return new WaitWhile(tween.IsPlaying);

		OnPatternFinished();
	}

    protected override void OnStopPattern()
    {
        // Spawn isn't stoppable.
    }
}
