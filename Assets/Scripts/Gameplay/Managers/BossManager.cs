using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BossManager : MonoBehaviour {

    public GameObject[] phases;

    [Header("Spawns")]
    public Transform center;
    public Transform north;
    public Transform east;
    public Transform south;
    public Transform west;

    [Header("UI")]
    public Image lifepointsBar;

    [HideInInspector]
    public float width;
    [HideInInspector]
    public float height;

    public PhaseTransitionManager phaseTransitionMgr;

    public int phaseId = -1;

    public GameObject CurrentPhaseGo
    {
        get { return currentPhaseGo; }
        set
        {
            currentPhaseGo = value;
            currentPhaseGo.SetActive(true);
        }
    }
    private GameObject currentPhaseGo;

    private PhaseAI currentPhase;

    private void Awake()
    {
        phaseTransitionMgr.OnTransitionFinished = BeginPhase;

        SetupPhases();

        DisplayLifeBar(false);
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
            PhaseAI boss = g.GetComponent<PhaseAI>();
            boss.bossMgr = this;

            boss.OnPhaseFinished = NextPhase;
        }
    }

    /// <summary>
    /// Trigger the new phase. Transition then start.
    /// </summary>
    private void NextPhase()
    {
        if (currentPhaseGo != null)
        {
            currentPhaseGo.SetActive(false);
        }

        phaseId++;

        // Transition then begin phase.
        if (phaseId < phases.Length)
        {
            phaseTransitionMgr.NextPhase(phaseId);
        }

        // All phases beaten.
        else
        {
            GameManager.instance.GameFinished();
        }
    }

    private void BeginPhase()
    {
        Debug.Log("PHASE " + (phaseId+1) + " starts");

        CurrentPhaseGo = phases[phaseId];
        currentPhase = currentPhaseGo.GetComponent<PhaseAI>();

        DisplayLifeBar(true);
        UpdateLifepoints();
    }

    public void DisplayLifeBar(bool enabled)
    {
        lifepointsBar.gameObject.SetActive(enabled);
    }

    public void UpdateLifepoints()
    {
        lifepointsBar.fillAmount = currentPhase.Lifepoints / currentPhase.maxLifepoints; 
    }
}
