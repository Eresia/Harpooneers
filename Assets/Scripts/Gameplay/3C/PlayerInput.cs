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

    private Player player; // Rewired player.

    private bool doPause; // Do the pause if the delay is repected.

    private int controllerDisconnected;

    public int TutoStep
    {
        get { return tutoStep; }
        set { tutoStep = value; }
    }
    private int tutoStep;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        playerMgr = GetComponent<PlayerManager>();

        // Main features
        player.AddInputEventDelegate(DropBomb, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Drop Bomb");
        player.AddInputEventDelegate(PullingRope, UpdateLoopType.Update, "Pull On Rope");

        // Utility
        player.AddInputEventDelegate(TogglePause, UpdateLoopType.Update, "Toggle Pause");
        player.AddInputEventDelegate(DisplayPlayerPosition, UpdateLoopType.Update, "Display Player");

        // CUT
        //player.AddInputEventDelegate(ResurrectAlly, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Resurrect");

        // Subscribe to events of controller connection
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;

        // Deactivate the player position indicator
        playerMgr.SetupIndicator(playerId);

		// GameManager.instance.audioManager.CreatePersistantSound (AudioManager.PossibleSound.PULL, pull_sound, 0.05f);
    }

    private void Reset()
    {
        //movement = GetComponent<MovementBehaviour>();
        harpoonLauncher = GetComponent<HarpoonLauncher>();
        playerMgr = GetComponent<PlayerManager>();

        bombLauncher = transform.parent.GetComponentInChildren<ExplosiveBarrel>();
    }

    private void Update()
    {
        if (playerMgr.IsDead)
        {
            return;
        }

        if(tutoStep < 1)
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

        if (tutoStep < 2)
        {
            return;
        }

        // Handle harpoon rotation.
        {
            float rotateX = player.GetAxis ("Rotate Horizontal");
            float rotateZ = player.GetAxis("Rotate Vertical");

            Vector3 harpoonDir = new Vector3(rotateX, 0f, rotateZ);
            
            //Debug.DrawRay(transform.position, harpoonDir.normalized * 2.5f, Color.red, 1f);

            harpoonLauncher.LaunchHarpoon(harpoonDir);
        }

        // CUT
        // HandleCutRope();
    }

    private void PullingRope(InputActionEventData data)
    {
        if (playerMgr.IsDead)
        {
            return;
        }

        if (data.GetButton())
        {
            harpoonLauncher.Pull();
        }
    }

    private void DropBomb(InputActionEventData data)
    {
        if (playerMgr.IsDead)
        {
            return;
        }

        if(tutoStep < 3)
        {
            return;
        }

        if (data.GetButton() && !bombLauncher.gameObject.activeSelf)
        {
            // Spawn the bomb behind the boat
            bombLauncher.gameObject.SetActive(true);
            bombLauncher.SpawnTheBomb(transform.position - bombLauncher.behindOffset * transform.forward, movement.physicMove.Velocity);
        }
    }

    // CUT - Now collide with the boat to rez an ally !
    /*
    private void ResurrectAlly(InputActionEventData data)
    {
        if (playerMgr.IsDead)
        {
            return;
        }

        if (data.GetButtonDown())
        {
            playerMgr.ResurrectFriend();
        }
    }
    */

    // CUT !
    /*
    private void ReleaseRope(InputActionEventData data)
    {
        if (playerMgr.IsDead)
        {
            return;
        }

        if (data.GetButton())
        {
            harpoonLauncher.Release();
        }
    }
    */

    // CUT !
    private void HandleCutRope()
    {
        if (playerMgr.IsDead)
        {
            return;
        }

        if (player.GetButtonDown("Pull On Rope") && player.GetButton("Release Rope")
            || player.GetButton("Pull On Rope") && player.GetButtonDown("Release Rope")
            || player.GetButtonDown("Pull On Rope") && player.GetButtonDown("Release Rope")
            )
        {
            harpoonLauncher.Cut();
        }
    }

    // UTILITY :
    private void TogglePause(InputActionEventData data)
    {
        // If game is paused : direct unpause.
        if (GameManager.instance.IsPause)
        {
            if (data.GetButtonDown())
            {
                GameManager.instance.UnPauseGame();
                doPause = false;
            }
        }

        // If game is unpaused.
        else
        {
            if (!doPause)
            {
                // Use this to pause after a delay.
                doPause = data.GetButtonTimePressed() > timeBeforePause;

                if (doPause)
                {
                    GameManager.instance.PauseGame(data.playerId);
                }
            }
        }
    }

    private void DisplayPlayerPosition(InputActionEventData data)
    {
        if (data.GetButtonDown())
        {
            playerMgr.FeedbackPlayerPos(true);
        }
        if (data.GetButtonUp())
        {
            playerMgr.FeedbackPlayerPos(false);
        }
    }
    
    // Controller connection - disconnection.

    // This function will be called when a controller is connected
    // You can get information about the controller that was connected via the args parameter
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        // Unpause the game if the game was stopped because one or more controllers has(ve) been disconnected.
        controllerDisconnected--;

        if (GameManager.instance.IsPause && controllerDisconnected == 0)
        {
            //GameManager.instance.PauseGame(-1);
            Time.timeScale = 0f;
        }
    }

    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        // Pause the game if at least one controller has been disconnected.

        if (!GameManager.instance.IsPause)
        {
            controllerDisconnected++;

            // GameManager.instance.PauseGame(-1);
            Time.timeScale = 1f;
        }
    }

    private void OnDestroy()
    {
        /*
        if(player != null)
        {
            // Free delegates.
            player.RemoveInputEventDelegate(DropBomb, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Drop Bomb");
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