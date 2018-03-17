using UnityEngine;
using System.Collections;
using DG.Tweening;


public class WhalePhaseAI : PhaseAI {
    
    [HideInInspector]
    public WhaleReferences whaleReferences;

    public GameObject whalePrefab;
    public Transform FXTransform;
    public ParticleSystem spawningFX;

    [Header("Patterns objects")]
    public Geyser geyserPrefab;

    public Geyser[] Geysers
    {
        get { return geysers; }
    }
    private Geyser[] geysers;

    public Transform WhaleTransform { get; private set; }
    public Transform WhaleChildTransform { get; private set; }
    public Animator WhaleAnimator { get; private set; }
    public GameObject Whale
    {
        get
        {
            return whale;
        }
    }
    private GameObject whale;
    
    [Header("Pattern config")]
    public int noHittablePatternCount;
    public int numberOfPatternsWithoutHit;

    public float damageMinToHaveScream = 10f;
    
	public AudioClip whale_scream;
	public AudioClip whale_spawn;

	public float endPhaseTime = 4f;

	public float resetDepth;

    protected override void Awake()
    {
        base.Awake();

        SetupWhale();
    }

    // Spawn and setup whale correctly.
    private void SetupWhale()
    {
        whale = Instantiate(whalePrefab);
        whale.SetActive(false);

        whaleReferences = whale.GetComponent<WhaleReferences>();

        WhaleTransform = whaleReferences.transform;
        WhaleChildTransform = whaleReferences.whaleChildTransform;
        WhaleAnimator = whaleReferences.whaleAnimator;

        geysers = new Geyser[GameManager.instance.nbOfPlayers];
        for (int i = 0; i < geysers.Length; i++)
        {
            geysers[i] = Instantiate(geyserPrefab);
        }

		whaleReferences.whaleBody.OnWhaleExplode = HitBoss;
        
        for (int i = 0; i < whaleReferences.hittableScripts.Length; i++)
        {
            whaleReferences.hittableScripts[i].hitCallback = HitBoss;
        }
    }

    public override void HitBoss(float damageAmount)
    {
        base.HitBoss(damageAmount);

        OnWhaleHit(damageAmount);
    }

    /// <summary>
    /// Specific action when whale is hitten.
    /// </summary>
    /// <param name="damageAmount"></param>
    private void OnWhaleHit(float damageAmount)
    {
        if (damageAmount >= damageMinToHaveScream)
        {
            GameManager.instance.audioManager.PlaySoundOneTime(whale_scream, 0.2f);
            GameManager.instance.camMgr.Shake();
        }
    }

    /// <summary>
    /// Stop the current pattern !
    /// </summary>
    public void StopPattern()
    {
        CurrentPattern.StopPattern();
    }

    // Reset the whale's transform.
    public void ResetWhaleTransform()
    {
        ResetWhaleTransform(resetDepth);
    }

	public void ResetWhaleTransform(float depth)
    {
        Whale.SetActive(false);

        WhaleTransform.rotation = Quaternion.identity;
        WhaleTransform.position = new Vector3(0, -depth, 0);
        WhaleTransform.localScale = Vector3.one;
        
        WhaleChildTransform.localRotation = Quaternion.identity;
        WhaleChildTransform.localPosition = Vector3.zero;
        WhaleChildTransform.localScale = Vector3.one;
    }

	public override IEnumerator OnPhaseFinishedCoroutine(){
		Vector3 pos = WhaleTransform.position;
		Vector3 target = pos;
		target.y = -20f;
		Tween tween = WhaleTransform.DOMove(target, 4f);
        yield return new WaitWhile(tween.IsPlaying);
	}
}
