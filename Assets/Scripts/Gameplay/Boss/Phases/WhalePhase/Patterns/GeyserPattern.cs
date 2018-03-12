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

    protected override void StopPattern()
    {
        boss.StopCoroutine(SpawnGeyser());
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
    }

    // Decide from which spawn the whale will dash.
    private IEnumerator SpawnGeyser()
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

        // TODO Display shadow of whale.

        yield return new WaitForSeconds(state.timeBeforeSpawning);

        ChooseSpawn();

        // TODO Baleine emerge !

        whaleAI.Whale.SetActive(true);

        // Boss wait...
        yield return new WaitForSeconds(10f);

        FinishPattern();
	}


}
