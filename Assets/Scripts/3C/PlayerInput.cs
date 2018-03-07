using UnityEngine;
using Rewired;

/// <summary>
/// Handle Input associated to each player.
/// </summary>
[RequireComponent(typeof(MovementBehaviour))]
public class PlayerInput : MonoBehaviour
{
    [Tooltip("Rewired player id")]
    public int playerId = 0;

    [Header("Main components")]
    public MovementBehaviour movement;
    public PlayerManager playerMgr;
    public HarpoonLauncher harpoonLauncher;
    public ExplosiveBarrel bombLauncher;
    
    [Header("Other actions")]
    public float timeBeforePause = 1f;

    private Rigidbody _myRigidbody;
    
    private Player player; // Rewired player.

    private bool doPause; // Do the pause if the delay is repected.

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        _myRigidbody = GetComponent<Rigidbody>();
        playerMgr = GetComponent<PlayerManager>();

        // Register delegates for specific actions.
        player.AddInputEventDelegate(DropBomb, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Drop Bomb");
        player.AddInputEventDelegate(ResurrectAlly, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Resurrect");
        player.AddInputEventDelegate(TogglePause, UpdateLoopType.Update, "Toggle Pause");
        player.AddInputEventDelegate(ReleaseRope, UpdateLoopType.Update, "Release Rope");
        player.AddInputEventDelegate(PullingOnRope, UpdateLoopType.Update, "Pull On Rope");
        
        // Subscribe to events of controller connection
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }

    private void Reset()
    {
        movement = GetComponent<MovementBehaviour>();
        harpoonLauncher = GetComponent<HarpoonLauncher>();
        playerMgr = GetComponent<PlayerManager>();

        bombLauncher = transform.parent.GetComponentInChildren<ExplosiveBarrel>();
    }

    private void Update()
    {
        if (playerMgr.isDead)
        {
            return;
        }

        // Handle movement.
        {
            float moveX = player.GetAxis("Move Horizontal");
            float moveZ = player.GetAxis("Move Vertical");

            Vector3 moveDir = new Vector3(moveX, 0f, moveZ);

            movement.Move(moveDir.normalized);
        }

        // Handle harpoon rotation.
        {
            float rotateX = player.GetAxis("Rotate Horizontal");
            float rotateZ = player.GetAxis("Rotate Vertical");

            Vector3 harpoonDir = new Vector3(rotateX, 0f, rotateZ);

            //Debug.DrawRay(transform.position, harpoonDir.normalized * 2.5f, Color.red, 1f);

            harpoonLauncher.LaunchHarpoon(harpoonDir.normalized);
        }

        HandleCutRope();
    }

    private void TogglePause(InputActionEventData data)
    {
        // If game is unpaused.

        // Use this to pause after a delay.
        if (!doPause)
        {
            doPause = data.GetButtonTimePressed() > timeBeforePause;
        }

        if (data.GetButtonUp())
        {
            if(doPause)
            {
                Debug.Log("Toggle pause !");
            }
            
            doPause = false;
        }

        // If game is paused : direct unpause.
    }

    private void DropBomb(InputActionEventData data)
    {
        if(playerMgr.isDead)
        {
            return;
        }

        if (data.GetButtonDown() && !bombLauncher.gameObject.activeSelf)
        {
            // Spawn the bomb behind the boat
            bombLauncher.gameObject.SetActive(true);
            bombLauncher.SpawnTheBomb(transform.position - 1.25f * transform.forward, _myRigidbody.velocity);
        }
    }

    private void ResurrectAlly(InputActionEventData data)
    {
        if (playerMgr.isDead)
        {
            return;
        }

        if (data.GetButtonDown())
        {
            playerMgr.ResurrectFriend();
        }
    }

    private void ReleaseRope(InputActionEventData data)
    {
        if (playerMgr.isDead)
        {
            return;
        }

        if (data.GetButton())
        {
            harpoonLauncher.Release();
        }
    }

    private void PullingOnRope(InputActionEventData data)
    {
        if (playerMgr.isDead)
        {
            return;
        }

        if (data.GetButton())
        {
            harpoonLauncher.Pull();
        }
    }

    private void HandleCutRope()
    {
        if (playerMgr.isDead)
        {
            return;
        }

        if (player.GetButtonDown("Pull On Rope") && player.GetButton("Release Rope")
            || player.GetButton("Pull On Rope") && player.GetButtonDown("Release Rope")
            || player.GetButtonDown("Pull On Rope") && player.GetButtonDown("Release Rope")
            )
        {
            Debug.Log("Cut the rope");

            harpoonLauncher.Cut();
        }
    }

    // Controller connection - disconnection.

    // This function will be called when a controller is connected
    // You can get information about the controller that was connected via the args parameter
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        // TODO Unpause the game if the game was stopped because a controller has been disconnected.
    }

    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        // TODO Pause the game if a controller has been disconnected.
    }

    private void OnDestroy()
    {
        /*
        if(player != null)
        {
            // Free delegates.
            player.RemoveInputEventDelegate(DropBomb);
            player.RemoveInputEventDelegate(ResurrectAlly);
            player.RemoveInputEventDelegate(ReleaseRope);
            player.RemoveInputEventDelegate(PullingOnRope);
        }
        */

        // Unsubscribe from events
        ReInput.ControllerConnectedEvent -= OnControllerConnected;
        ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
    }
}