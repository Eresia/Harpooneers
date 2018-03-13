using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class BossManager : MonoBehaviour {

    public GameObject[] phases;

    [Header("Spawns")]
    public Transform center;
    public Transform north;
    public Transform east;
    public Transform south;
    public Transform west;

    [HideInInspector]
    public float width;
    [HideInInspector]
    public float height;

    public PhaseTransitionManager phaseTransitionMgr;

    public int phaseId = -1;

    public GameObject CurrentPhase
    {
        get { return currentPhase; }
        set
        {
            currentPhase = value;
            currentPhase.SetActive(true);
        }
    }
    private GameObject currentPhase;

    private void Awake()
    {
        phaseTransitionMgr.OnTransitionFinished = BeginPhase;

        SetupPhases();
    }

    private void Start()
    {
        SetupSpawns();

        // Begin the fight !
        NextPhase();
    }

    private void SetupSpawns()
    {
        center.position = GameManager.instance.boundaryMgr.ScreenToWorldPos(new Vector2(0.5f, 0.425f));
        south.position = GameManager.instance.boundaryMgr.ScreenToWorldPos(new Vector2(0.5f, 0f));
        north.position = GameManager.instance.boundaryMgr.ScreenToWorldPos(new Vector2(0.5f, 1f));
        east.position = GameManager.instance.boundaryMgr.ScreenToWorldPos(new Vector2(0f, 0.425f));
        west.position = GameManager.instance.boundaryMgr.ScreenToWorldPos(new Vector2(1f, 0.425f));

        width = Vector3.Distance(east.position, west.position);
        height = Vector3.Distance(north.position, south.position);
    }

    private void SetupPhases()
    {
        foreach(GameObject g in phases)
        {
            BossAI boss = g.GetComponent<BossAI>();
            boss.bossMgr = this;

            boss.OnPhaseFinished = NextPhase;
        }
    }

    /// <summary>
    /// Trigger the new phase. Transition then start.
    /// </summary>
    private void NextPhase()
    {
        if (currentPhase != null)
        {
            currentPhase.SetActive(false);
        }

        phaseId++;
        if (phaseId < phases.Length)
        {
            phaseTransitionMgr.NextPhase(phaseId);
        }

        // ALL PHASES BEATEN.
        else
        {
            GameManager.instance.GameFinished();
        }
    }

    private void BeginPhase()
    {
        Debug.Log("PHASE " + (phaseId+1) + " starts");

        CurrentPhase = phases[phaseId];
    }
}
