using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhalePhaseAI : BossAI {

    public GameObject whalePrefab;
    public Transform FX;
    public ParticleSystem spawningFX;
    
    public Transform WhaleTransform { get; private set; }

    public GameObject Whale
    {
        get
        {
            return whale;
        }
    }
    private GameObject whale;

    protected override void Awake()
    {
        base.Awake();

        SpawnWhale();
    }

    private void SpawnWhale()
    {
        whale = Instantiate(whalePrefab);

        WhaleTransform = whale.GetComponent<Transform>();
    }
}
