using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GeyserPattern : BossPattern {

    private GeyserState state;
    private WhalePhaseAI whaleAI;

    public GeyserPattern(GeyserState state)
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
		boss.StartCoroutine(SpawnGeyser());
	}

    // Decide from which spawn the whale will dash.
    private IEnumerator SpawnGeyser()
	{
        yield return new WaitForSeconds(2f);

        FinishPattern();
	}
}
