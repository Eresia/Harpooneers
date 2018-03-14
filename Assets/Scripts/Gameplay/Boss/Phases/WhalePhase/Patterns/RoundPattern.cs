using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RoundPattern : BossPattern {

    private RoundState roundState;
    private WhalePhaseAI whaleAI;

    public RoundPattern(RoundState dashState)
    {
        this.roundState = dashState;
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        whaleAI = boss as WhalePhaseAI;
    }

    protected override void ExecutePattern() {
		boss.StartCoroutine(Move());
	}

    private IEnumerator Move()
    {
        yield return null;
    }

    protected override void OnStopPattern()
    {
        // Dash isn't stoppable.
    }
}
