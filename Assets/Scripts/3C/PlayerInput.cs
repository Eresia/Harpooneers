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
    public HarpoonLauncher harpoon;

    [Header("Bomb components")]
    public ExplosiveBarrel playerBomb;

    private Rigidbody _myRigidbody;
    private Player player; // Rewired player.

    public float timeBeforePause = 1f;
    private bool doPause; // Do the pause if the delay is repected.

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        _myRigidbody = GetComponent<Rigidbody>();

        // Register delegates for specific actions.
        player.AddInputEventDelegate(DropBomb, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Drop Bomb");
        player.AddInputEventDelegate(ResurrectAlly, UpdateLoopType.Update, InputActionEventType.ButtonPressed, "Resurrect");
        player.AddInputEventDelegate(TogglePause, UpdateLoopType.Update, "Toggle Pause");
        player.AddInputEventDelegate(ReleaseRope, UpdateLoopType.Update, "Release Rope");
        player.AddInputEventDelegate(PullingOnRope, UpdateLoopType.Update, "Pull On Rope");
    }

    private void Reset()
    {
        movement = GetComponent<MovementBehaviour>();
        harpoon = GetComponent<HarpoonLauncher>();
    }

    private void Update()
    {
        // TODO : Check if the player isn't dead.

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

            harpoon.LaunchHarpoon(harpoonDir.normalized);
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
        // TODO : Check if the player isn't dead.

        if (data.GetButtonDown() && !playerBomb.gameObject.activeSelf)
        {
            // Spawn the bomb behind the boat
            playerBomb.gameObject.SetActive(true);
            playerBomb.SpawnTheBomb(transform.position - 1.25f * transform.forward, _myRigidbody.velocity);
        }
    }

    private void ResurrectAlly(InputActionEventData data)
    {
        if (data.GetButtonDown())
        {
            Debug.Log("Resurrect Ally");
        }
    }

    private void ReleaseRope(InputActionEventData data)
    {
        // TODO : Check if the player isn't dead.

        if (data.GetButton())
        {
            harpoon.Release();
        }
    }

    private void PullingOnRope(InputActionEventData data)
    {
        // TODO : Check if the player isn't dead.

        if (data.GetButton())
        {
            harpoon.Pull();
        }
    }

    private void HandleCutRope()
    {
        // TODO : Check if the player isn't dead.

        if (player.GetButtonDown("Pull On Rope") && player.GetButton("Release Rope")
            || player.GetButton("Pull On Rope") && player.GetButtonDown("Release Rope")
            || player.GetButtonDown("Pull On Rope") && player.GetButtonDown("Release Rope")
            )
        {
            Debug.Log("Cut the rope");

            harpoon.Cut();
        }
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
    }
}