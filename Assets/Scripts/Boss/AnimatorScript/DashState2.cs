using UnityEngine;

public class DashState : BossState {

    [Header("Distance with spawn (screen offset)")]
    public float offsetX = 0.3f;
    public float offsetY = 0.1f;

    [Header("Preview")]
    public int TurnMin = 3;
    public int TurnMax = 5;
    
    [Header("Effect duration")]
    [Tooltip("Bubble duration before spawning")]
    public float WaitBeforeSpawn = 2f;
    public float WaitBeforeDash = 0.5f;
    
    public float translateSpeed = 20f;
    public float dashSpeed = 90f;
    public float whaleOffset = 18f;

    private DashPattern dashPattern;

    [HideInInspector]
    public float minX, maxX, minZ, maxZ;

	public override void Init(BossAI boss, Animator animator){

		base.Init(boss, animator);

        minX = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.xBottomLeft, GameManager.instance.boundaryMgr.trapezeData.xBottomRight, 0.5f - offsetX);
        maxX = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.xBottomLeft, GameManager.instance.boundaryMgr.trapezeData.xBottomRight, 0.5f + offsetX);
        minZ = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.zBottom, GameManager.instance.boundaryMgr.trapezeData.zTop, 0.5f - offsetY);
        maxZ = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.zBottom, GameManager.instance.boundaryMgr.trapezeData.zTop, 0.5f + offsetY);

        dashPattern = new DashPattern(boss, this);
	}

	// OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		BeginPattern(dashPattern);
	}

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called before OnStateIK is called on any state inside this state machine
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMachineEnter is called when entering a statemachine via its Entry Node
	//override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
	//
	//}

	// OnStateMachineExit is called when exiting a statemachine via its Exit Node
	//override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
	//
	//}
}
