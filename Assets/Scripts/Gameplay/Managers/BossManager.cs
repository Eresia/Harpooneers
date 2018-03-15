using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BossManager : MonoBehaviour {

    public GameObject[] phases;

    [Header("Spawns")]

    public Vector3 bossAreaCenter;
    public Vector3 bossArea;
    [HideInInspector]
    public Transform center, north, east, south, west;

    [Header("UI")]
    public GameObject lifepointsBar;
    public Image lifepointsFilling;

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
        width = bossArea.x;
        height = bossArea.z;
        center.position = bossAreaCenter;

        west.position = bossAreaCenter + new Vector3(-width*0.5f, 0f, 0f);
        east.position = bossAreaCenter + new Vector3(width * 0.5f, 0f, 0f);
        north.position = bossAreaCenter + new Vector3(0f, 0f, height * 0.5f);
        south.position = bossAreaCenter + new Vector3(0f, 0f, -height * 0.5f);
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
        lifepointsFilling.fillAmount = currentPhase.Lifepoints / currentPhase.maxLifepoints; 
    }

    private void OnValidate()
    {
        SetupSpawns();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bossAreaCenter, bossArea);
    }
}
